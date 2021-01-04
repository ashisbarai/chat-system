import { Component, OnInit} from '@angular/core';
import { MatSnackBar, MatSnackBarRef, SimpleSnackBar } from '@angular/material/snack-bar';
import { ChatService } from 'src/app/chats/chat.service';
import { AuthService } from 'src/app/core/auth-service.service';
import { IUserInfo } from 'src/app/users/user-info';
import { UserService } from 'src/app/users/user.service';

@Component({
    selector: 'app-home',
    templateUrl: 'home.component.html',
    styleUrls: ['home.component.scss']
})

export class HomeComponent implements OnInit {
    constructor(private _authService: AuthService, private userService: UserService, 
      private chatService: ChatService, private snackBar: MatSnackBar) 
    {}
    displayedColumns: string[] = ['name', 'email', 'id'];
    users: IUserInfo[];
    userId: number;
    ngOnInit() { 
      this._authService.getAuthContext().subscribe((authContext)=>{
        this.userId = +authContext.claims.find(c=>c.type=='sub').value;
        this.getAllFriendsByUserId(this.userId);
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
      deleteAllChat(id, name){
        if(confirm('Do you want to delete all chats with ' + name + '?')){
          this.chatService.deleteUsersChatByUserId(this.userId, id).subscribe((res)=>{
            this.openSnackBar('Chat deleted successfully!', "");
          });
        }

      }
      openSnackBar(message: string, action: string) : MatSnackBarRef<SimpleSnackBar> {
        return this.snackBar.open(message, action, {
          duration: 1000,
        });
      }
}