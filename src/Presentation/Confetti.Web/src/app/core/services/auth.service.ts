import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { from, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';

export function getClientSettings(): UserManagerSettings {
  return {
    authority: 'http://localhost:5004',
    client_id: 'ConfettiSpa',
    redirect_uri: 'http://localhost:4200/signin-oidc',
    silent_redirect_uri: 'http://localhost:4200/silent-refresh.html',
    post_logout_redirect_uri: 'http://localhost:4200/',
    response_type: 'id_token token',
    // response_type: 'code',
    scope: 'openid profile confetti_api account email roles',
    // automaticSilentRenew: true,
    filterProtocolClaims: true,
    loadUserInfo: true
    // acr_values: '0',
    // response_mode: 'json'
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private manager = new UserManager(getClientSettings());
  // private manager: UserManager;

  constructor(
    private router: Router
  ) {}

  public getUser(): Observable<User> {
    return from(this.manager.getUser());
  }

  public signInRedirect(): Observable<void> {
    return from(this.manager.signinRedirect()).pipe(
      tap(() => {
        sessionStorage.setItem('oidc.returnUrl', this.router.url);
      })
    );
  }

  public signInSilent(): Observable<User> {
    return from(this.manager.signinSilent());
  }

  public signInRedirectCallback(): Observable<User> {
    return from(this.manager.signinRedirectCallback()).pipe(
      tap(() => {
        const returnUrl = sessionStorage.getItem('oidc.returnUrl');
        if (returnUrl) {
          sessionStorage.removeItem('oidc.returnUrl');
          this.router.navigate([returnUrl]);
        }
        this.router.navigate(['/']);
      })
    );
  }
}
