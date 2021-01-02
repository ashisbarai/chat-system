import { Injectable } from '@angular/core';
import { UserManager, User, WebStorageStateStore } from 'oidc-client';
import { Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AuthContext } from '../model/auth-context';
import { AppconfigService } from '../appconfig.service';

@Injectable()
export class AuthService {
  private _userManager: UserManager;
  private _user: User;
  private _loginChangedSubject = new Subject<boolean>();

  loginChanged = this._loginChangedSubject.asObservable();
  authContext: AuthContext;

  constructor(private _httpClient: HttpClient, private _appconfigService: AppconfigService) {
    const stsSettings = {
      authority: _appconfigService.stsAuthority,
      client_id: _appconfigService.clientId,
      redirect_uri: `${_appconfigService.clientRoot}signin-callback`,
      scope: 'openid profile chat-api',
      response_type: 'code',
      post_logout_redirect_uri: `${_appconfigService.clientRoot}signout-callback`,
      automaticSilentRenew: true,
      silent_redirect_uri: `${_appconfigService.clientRoot}assets/silent-callback.html`,
      userStore: new WebStorageStateStore({store: localStorage})
    };
    this._userManager = new UserManager(stsSettings);
    this._userManager.events.addAccessTokenExpired(_ => {
      this._loginChangedSubject.next(false);
    });
    this._userManager.events.addUserLoaded(user => {
      if (this._user !== user) {
        this._user = user;
        this.loadSecurityContext();
        this._loginChangedSubject.next(!!user && !user.expired);
      }
    });
  }

  login() {
    return this._userManager.signinRedirect();
  }

  isLoggedIn(): Promise<boolean> {
    return this._userManager.getUser().then(user => {
      const userCurrent = !!user && !user.expired;
      if (this._user !== user) {
        this._loginChangedSubject.next(userCurrent);
      }
      if (userCurrent && !this.authContext) {
        this.loadSecurityContext();
      }
      this._user = user;
      return userCurrent;
    });
  }

  getAuthContext(){
    return this._httpClient
    .get<AuthContext>(`${this._appconfigService.apiRoot}GetAuthContext`);
  }

  completeLogin() {
    return this._userManager.signinRedirectCallback().then(user => {
      this._user = user;
      this._loginChangedSubject.next(!!user && !user.expired);
      return user;
    });
  }

  logout() {
    this._userManager.signoutRedirect();
  }

  completeLogout() {
    this._user = null;
    this._loginChangedSubject.next(false);
    return this._userManager.signoutRedirectCallback();
  }

  getAccessToken() {
    return this._userManager.getUser().then(user => {
      if (!!user && !user.expired) {
        return user.access_token;
      }
      else {
        return null;
      }
    });
  }

  loadSecurityContext() {
    this._httpClient
      .get<AuthContext>(`${this._appconfigService.apiRoot}GetAuthContext`)
      .subscribe(
        context => {
          this.authContext = new AuthContext();
          this.authContext.claims = context.claims;
        },
        error => console.error(error)
      );
  }

}
