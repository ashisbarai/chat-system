import { Component, OnInit} from '@angular/core';
import { AuthService } from 'src/app/core/auth-service.service';
import { IUserInfo } from 'src/app/users/user-info';
import { UserService } from 'src/app/users/user.service';

@Component({
    selector: 'app-home',
    templateUrl: 'home.component.html',
    styleUrls: ['home.component.scss']
})

export class HomeComponent implements OnInit {
    constructor(private _authService: AuthService, private userService: UserService) 
    {}
    displayedColumns: string[] = ['name', 'email', 'id'];
    users: IUserInfo[];
    ngOnInit() { 
      this._authService.getAuthContext().subscribe((authContext)=>{
        this.getAllFriendsByUserId(+authContext.claims.find(c=>c.type=='sub').value);
      });
    }
    getAllFriendsByUserId(userId: number){
      this.userService.getAllFriendsByUserId(userId).subscribe((users)=>{
        this.users = users;
      });
    }
    logout(){
        this._authService.logout();
      }
    
}