using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using Confetti.Identity.Extensions;
using Confetti.Identity.Models;
using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Confetti.Identity.Infrastructure.Hosting
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class MessageCookie<TMessage> where TMessage : Message
    {
        static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
        };

        readonly HttpContext ctx;
        readonly IdentityServerOptions options;

        internal MessageCookie(HttpContext ctx, IdentityServerOptions options)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            if (options == null) throw new ArgumentNullException(nameof(options));
            
            this.ctx = ctx;
            this.options = options;
        }

        internal MessageCookie(HttpContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException(nameof(ctx));
            
            this.ctx = ctx;
        }

        string MessageType
        {
            get { return typeof(TMessage).Name; }
        }

        string Protect(IDataProtector protector, TMessage message)
        {
            var json = JsonConvert.SerializeObject(message, settings);
            // Logger.DebugFormat("Protecting message: {0}", json);

            return protector.Protect(json);
        }

        TMessage Unprotect(string data, IDataProtector protector)
        {
            var json = protector.Unprotect(data);
            var message = JsonConvert.DeserializeObject<TMessage>(json);
            return message;
        }

        string GetCookieName(string id = null)
        {
            // return String.Format("{0}{1}.{2}", 
            //     options.AuthenticationOptions.CookieOptions.Prefix, 
            //     MessageType, 
            //     id);
            return String.Format("{0}.{1}", MessageType, id);
        }
        
        string CookiePath
        {
            get
            {
                return ctx.GetIdentityServerBasePath().CleanUrlPath();
            }
        }

        private IEnumerable<string> GetCookieNames()
        {
            var key = GetCookieName();
            foreach (var cookie in ctx.Request.Cookies)
            {
                if (cookie.Key.StartsWith(key))
                {
                    yield return cookie.Key;
                }
            }
        }

        private string Protect(TMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            
            var protector = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>()
                .CreateProtector(typeof(MessageCookie<TMessage>).FullName);
            return Protect(protector, message);
        }

        private TMessage Unprotect(string data)
        {
            if (data == null) throw new ArgumentNullException("data");
            
            var protector = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>()
                .CreateProtector(typeof(MessageCookie<TMessage>).FullName);
            return Unprotect(data, protector);
        }

        private bool Secure
        {
            get
            {
                return
                    // options.AuthenticationOptions.CookieOptions.SecureMode == CookieSecureMode.Always || 
                    ctx.Request.Scheme == Uri.UriSchemeHttps;
            }
        }

        public string Write(TMessage message)
        {
            ClearOverflow();

            if (message == null) throw new ArgumentNullException("message");

            var id = CryptoRandom.CreateUniqueId();
            var name = GetCookieName(id);
            var data = Protect(message);

            ctx.Response.Cookies.Append(
                name,
                data,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Secure,
                    Path = CookiePath
                });
            return id;
        }

        public TMessage Read(string id)
        {
            if (String.IsNullOrWhiteSpace(id)) return null;

            var name = GetCookieName(id);
            return ReadByCookieName(name);
        }

        TMessage ReadByCookieName(string name)
        {
            var data = ctx.Request.Cookies[name];
            if (!String.IsNullOrWhiteSpace(data))
            {
                try
                {
                    return Unprotect(data);
                }
                catch (Exception ex)
                {
                    // Logger.WarnException("Error unprotecting cookie: {0}", ex, name);
                    ClearByCookieName(name);
                }
            }
            return null;
        }

        public void Clear(string id)
        {
            var name = GetCookieName(id);
            ClearByCookieName(name);
        }

        void ClearByCookieName(string name)
        {
            ctx.Response.Cookies.Append(
                name,
                ".",
                new CookieOptions
                {
                    Expires = DateTime.SpecifyKind(DateTimeOffset.UtcNow.DateTime, DateTimeKind.Utc).AddYears(-1),
                    HttpOnly = true,
                    Secure = Secure,
                    Path = CookiePath
                });
        }

        private long GetCookieRank(string name)
        {   
            // empty and invalid cookies are considered to be the oldest:
            var rank = DateTimeOffset.MinValue.Ticks;

            try
            {
                var message = ReadByCookieName(name);
                if (message != null)
                {   // valid cookies are ranked based on their creation time:
                    rank = message.Created;
                }
            }
            catch (CryptographicException e)
            {   
                // cookie was protected with a different key/algorithm
                // Logger.DebugFormat("Unable to decrypt cookie {0}: {1}", name, e.Message);
            }
            
            return rank;
        }

        private void ClearOverflow()
        {
            var names = GetCookieNames();
            // var toKeep = options.AuthenticationOptions.SignInMessageThreshold;
            var toKeep = 1;

            if (names.Count() >= (toKeep * 2))
            {
                // we have way too many -- delete them all
                foreach (var name in names)
                {
                    ClearByCookieName(name);
                }
            }
            else if (names.Count() >= toKeep)
            {
                var rankedCookieNames =
                    from name in names
                    let rank = GetCookieRank(name)
                    orderby rank descending
                    select name;

                var purge = rankedCookieNames.Skip(Math.Max(0, toKeep - 1));
                foreach (var name in purge)
                {
                    ClearByCookieName(name);
                }
            }
        }
    }
}