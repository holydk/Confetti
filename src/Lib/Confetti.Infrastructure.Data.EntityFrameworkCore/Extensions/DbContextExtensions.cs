using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Confetti.Infrastructure.Data.EntityFrameworkCore.Mapping;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Represents database context extensions
    /// </summary>
    public static class DbContextExtensions
    {
        #region Utilities

        /// <summary>
        /// Get SQL commands from the script
        /// </summary>
        /// <param name="sql">SQL script</param>
        /// <returns>List of commands</returns>
        private static IList<string> GetCommandsFromScript(string sql)
        {
            var commands = new List<string>();

            //origin from the Microsoft.EntityFrameworkCore.Migrations.SqlServerMigrationsSqlGenerator.Generate method
            sql = Regex.Replace(sql, @"\\\r?\n", string.Empty);
            var batches = Regex.Split(sql, @"^\s*(GO[ \t]+[0-9]+|GO)(?:\s+|$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            for (var i = 0; i < batches.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(batches[i]) || batches[i].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                    continue;

                var count = 1;
                if (i != batches.Length - 1 && batches[i + 1].StartsWith("GO", StringComparison.OrdinalIgnoreCase))
                {
                    var match = Regex.Match(batches[i + 1], "([0-9]+)");
                    if (match.Success)
                        count = int.Parse(match.Value);
                }

                var builder = new StringBuilder();
                for (var j = 0; j < count; j++)
                {
                    builder.Append(batches[i]);
                    if (i == batches.Length - 1)
                        builder.AppendLine();
                }

                commands.Add(builder.ToString());
            }

            return commands;
        }

        /// <summary>
        /// Modify the input SQL query by adding passed parameters
        /// </summary>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>Modified raw SQL query</returns>
        private static string CreateSqlWithParameters(string sql, params object[] parameters)
        {
            //add parameters to sql
            for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
            {
                if (!(parameters[i] is DbParameter parameter))
                    continue;

                sql = $"{sql}{(i > 0 ? "," : string.Empty)} @{parameter.ParameterName}";

                //whether parameter is output
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                    sql = $"{sql} output";
            }

            return sql;
        }

        private static TService GetService<TService>(this IInfrastructure<IServiceProvider> databaseFacade)
        {
            if (databaseFacade is null)
            {
                throw new ArgumentNullException(nameof(databaseFacade));
            }

            var service = (TService)databaseFacade.Instance.GetService(typeof(TService));
            if (service == null)
            {
                throw new InvalidOperationException($"Service {typeof(TService).Name} not found.");
            }

            return service;
        }
            
        #endregion

        #region Methods

        public static IEnumerable<IMappingConfiguration> GetMappings<TContext>(this TContext context)
            where TContext : DbContext
        {
            return GetService<IMappingProvider>(context.Database).GetMappings();
        }

        /// <summary>
        /// Creates a LINQ query for the entity based on a raw SQL query
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">The values to be assigned to parameters</param>
        /// <returns>An IQueryable representing the raw SQL query</returns>
        public static IQueryable<TEntity> EntityFromSql<TEntity>(this DbContext context, string sql, params object[] parameters) where TEntity : class
        {
            return context.Set<TEntity>().FromSql(CreateSqlWithParameters(sql, parameters), parameters);
        }

        /// <summary>
        /// Executes the given SQL against the database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="doNotEnsureTransaction">true - the transaction creation is not ensured; false - the transaction creation is ensured.</param>
        /// <param name="timeout">The timeout to use for command. Note that the command timeout is distinct from the connection timeout, which is commonly set on the database connection string</param>
        /// <param name="parameters">Parameters to use with the SQL</param>
        /// <returns>The number of rows affected</returns>
        public static int ExecuteSqlCommand(this DbContext context, RawSqlString sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            //set specific command timeout
            var previousTimeout = context.Database.GetCommandTimeout();
            context.Database.SetCommandTimeout(timeout);

            var result = 0;
            if (!doNotEnsureTransaction)
            {
                //use with transaction
                using (var transaction = context.Database.BeginTransaction())
                {
                    result = context.Database.ExecuteSqlCommand(sql, parameters);
                    transaction.Commit();
                }
            }
            else
                result = context.Database.ExecuteSqlCommand(sql, parameters);
            
            //return previous timeout back
            context.Database.SetCommandTimeout(previousTimeout);
            
            return result;
        }

        /// <summary>
        /// Execute commands from the SQL script against the context database
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="sql">SQL script</param>
        public static void ExecuteSqlScript(this DbContext context, string sql)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var sqlCommands = GetCommandsFromScript(sql);
            foreach (var command in sqlCommands)
                context.ExecuteSqlCommand(command);
        }

        /// <summary>
        /// Execute commands from a file with SQL script against the context database
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="filePath">Path to the file</param>
        public static void ExecuteSqlScriptFromFile(this DbContext context, string filePath)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!File.Exists(filePath))
                return;

            context.ExecuteSqlScript(File.ReadAllText(filePath));
        }
            
        #endregion
    }
}