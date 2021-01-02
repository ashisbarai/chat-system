import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/auth-service.service';

@Component({
  selector: 'app-unauthorized',
  templateUrl: 'unauthorized.component.html'
})

export class UnauthorizedComponent implements OnInit {
  constructor(private _authService: AuthService) { }

  ngOnInit() { }

  logout() {
    this._authService.logout();
  }
}