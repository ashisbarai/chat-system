import { Injectable } from '@angular/core';
import { AuthService } from './auth-service.service';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class AuthGuardService {

  constructor(private _authService: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean>|boolean {
    return this._authService.isLoggedIn().then(isLoggedIn => {
      if (isLoggedIn) {
          return true;
      }
      sessionStorage.setItem('redirectUrl', state.url);
      this._authService.login();
      return false;
  });
  }
}
