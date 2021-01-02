import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/auth-service.service';

@Component({
    selector: 'app-website',
    templateUrl: 'website.component.html',
    styleUrls: ['website.component.scss']
})

export class WebsiteComponent implements OnInit {

    ngOnInit() { }

    constructor(private _authService: AuthService, private router: Router) {
      this._authService.isLoggedIn()
      this._authService.isLoggedIn().then(isLoggedIn => {
        if (isLoggedIn) {
          router.navigate(['home']);
        }
      });
    }
    login() {
      this._authService.login();
    }
}