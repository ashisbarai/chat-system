import { BreakpointObserver, BreakpointState } from '@angular/cdk/layout';
import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild} from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatService } from 'src/app/chats/chat.service';
import { MessageDto } from 'src/app/chats/message';
import { AuthService } from 'src/app/core/auth-service.service';
import { IUserInfo } from '../users/user-info';
import { UserService } from '../users/user.service';
const SMALL_WIDTH_BREAKPOINT = 720;

@Component({
    selector: 'app-chat',
    templateUrl: 'chat.component.html',
    styleUrls: ['chat.component.scss']
})

export class ChatComponent implements OnInit {
  public isScreenSmall: boolean;
  @ViewChild('chatContainer') private chatContainer: ElementRef;
  @ViewChild(MatSidenav) sidenav: MatSidenav;
  

    constructor(private _authService: AuthService, private chatService: ChatService, 
      private activeRoute: ActivatedRoute, private userService: UserService, 
      private breakpointObserver: BreakpointObserver, private router: Router) {
     }
    toUserId: number;
    toUserName: string;
    userId: number;
    userName: string;
    users: IUserInfo[];
    ngOnInit() { 
      this.breakpointObserver
      .observe([ `(max-width: ${SMALL_WIDTH_BREAKPOINT}px)` ])
      .subscribe((state: BreakpointState) => {
        this.isScreenSmall = state.matches;
      });

      this.router.events.subscribe(() => {
        if (this.isScreenSmall) {
          this.sidenav.close();
        }
      });

        this.activeRoute.params.subscribe(params => {
            this.toUserId = +params['id'];
            this.userService.getUserById(this.toUserId).subscribe((user)=>{
                this.toUserName = user.fullName;
                this._authService.getAuthContext().subscribe((authContext)=>{
                  this.userId = +authContext.claims.find(c=>c.type=='sub').value;
                  this.userName = authContext.claims.find(c=>c.type=='given_name').value + ' ' + authContext.claims.find(c=>c.type=='family_name').value;
                  this.getUsersChatByUserId(this.userId, this.toUserId);
                  this.scrollDown(this.chatContainer);
                  this.getAllFriendsByUserId(this.userId);
                });
            });
         });
        this.chatService.retrieveMappedObject().subscribe( (receivedObj: MessageDto) => { this.addToInbox(receivedObj);});
    }
toggle(){
  this.sidenav.toggle();
}
getAllFriendsByUserId(userId: number){
  this.userService.getAllFriendsByUserId(userId).subscribe((users)=>{
    this.users = users;
  });
}
  msgDto: MessageDto = new MessageDto();
  cahtList: MessageDto[] = [];
  getName(id: number){
    return id == this.userId ? this.userName : this.toUserName;
  }
  getUsersChatByUserId(userId: number, friendId: number){
     this.chatService.getUsersChatByUserId(userId, friendId).subscribe((messages)=>{
       this.cahtList = messages;
     })
  }
  send(): void {
    if(this.msgDto) {
      if(this.msgDto.message.length == 0){
        return;
      } else {
        this.msgDto.userId = this.userId;
        this.msgDto.friendId = this.toUserId;
        this.chatService.sendMessage(this.msgDto);
        this.msgDto.message = '';
      }
    }
  }

  addToInbox(obj: MessageDto) {
    let newObj = new MessageDto();
    newObj.userId = obj.userId;
    newObj.friendId = obj.friendId;
    newObj.message = obj.message;
    newObj.id = obj.id;
    newObj.messageType = obj.messageType;
    newObj.createdOn = obj.createdOn;
    this.cahtList.push(newObj);

    console.log(newObj);
    this.scrollDown(this.chatContainer);
  }
  scrollDown(container){
    setTimeout(function(){
      container.nativeElement.scrollTop = container.nativeElement.scrollHeight;
    },100);
  }
    logout(){
        this._authService.logout();
    }
    deleteChat(id: number){
      if(confirm('Are you sure?')){
        this.chatService.deleteChatById(id).subscribe((res)=>{
          var index = this.cahtList.findIndex(c=>c.id == id);
          if(index > -1){
            this.cahtList.splice(index, 1);
          }
        });
      }
    }
}