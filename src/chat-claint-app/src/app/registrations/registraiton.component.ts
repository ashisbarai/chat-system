import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Utils } from '../core/utils';
import { IUserInfo, UserInfo } from '../users/user-info';
import { UserService } from '../users/user.service';
import { MatSnackBar, MatSnackBarRef, SimpleSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
    selector: 'app-registration',
    templateUrl: 'registration.component.html',
    styleUrls: ['registration.component.scss']
})

export class RegistrationComponent implements OnInit {

    user = new UserInfo(0, '','','');
    error: string;

    constructor(private userService: UserService, private snackBar: MatSnackBar, private router: Router) { }

    ngOnInit() { }

    save(form: NgForm){

        this.userService.createUser(this.user).subscribe((user: IUserInfo) => {
            //this.resetModel();
            form.reset();
            this.openSnackBar(`${user.firstName} ${user.lastName} Created`, "");
            this.router.navigateByUrl('/');
        }, error => this.error = Utils.formatError(error));
      }
      openSnackBar(message: string, action: string) : MatSnackBarRef<SimpleSnackBar> {
        return this.snackBar.open(message, action, {
          duration: 1000,
        });
      }
      cancel(){
        this.router.navigateByUrl('/');
      }
}