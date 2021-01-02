import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/auth-service.service';

@Component({
  selector: 'app-signin-callback',
  template: `<div></div>`
})

export class SigninRedirectCallbackComponent implements OnInit {
  constructor(private _authService: AuthService,
              private _router: Router) { }

  ngOnInit() {
    this._authService.completeLogin().then(user => {
      var redirectUrl = sessionStorage.getItem("redirectUrl");
      if(!!redirectUrl && redirectUrl != 'null'){
      this._router.navigateByUrl(redirectUrl);
      }else{
        this._router.navigate(['home'], { replaceUrl: true });
      }
    })
  }
}