import { Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatService } from 'src/app/chats/chat.service';
import { MessageDto } from 'src/app/chats/message';
import { AuthService } from 'src/app/core/auth-service.service';
import { UserService } from '../users/user.service';

@Component({
    selector: 'app-chat',
    templateUrl: 'chat.component.html',
    styleUrls: ['chat.component.scss']
})

export class ChatComponent implements OnInit {
  @ViewChild('chatContainer') private chatContainer: ElementRef;
  
    constructor(private _authService: AuthService, private chatService: ChatService, 
      private activeRoute: ActivatedRoute, private userService: UserService) {
     }
    toUserId: number;
    toUserName: string;
    userId: number;
    userName: string;
    ngOnInit() { 
        this.activeRoute.params.subscribe(params => {
            this.toUserId = +params['id'];
            this.userService.getUserById(this.toUserId).subscribe((user)=>{
                this.toUserName = user.fullName;
                this._authService.getAuthContext().subscribe((authContext)=>{
                  this.userId = +authContext.claims.find(c=>c.type=='sub').value;
                  this.userName = authContext.claims.find(c=>c.type=='given_name').value + ' ' + authContext.claims.find(c=>c.type=='family_name').value;
                  this.getUsersChatByUserId(this.userId, this.toUserId);
                  this.scrollDown(this.chatContainer);
                });
            });
         });
        this.chatService.retrieveMappedObject().subscribe( (receivedObj: MessageDto) => { this.addToInbox(receivedObj);});
    }

  msgDto: MessageDto = new MessageDto();
  msgInboxArray: MessageDto[] = [];
  getName(id: number){
    return id == this.userId ? this.userName : this.toUserName;
  }
  getUsersChatByUserId(userId: number, friendId: number){
     this.chatService.getUsersChatByUserId(userId, friendId).subscribe((messages)=>{
       this.msgInboxArray = messages;
     })
  }
  send(): void {
    if(this.msgDto) {
      if(this.msgDto.message.length == 0){
        return;
      } else {
        this.msgDto.sender = this.userId;
        this.msgDto.receiver = this.toUserId;
        this.chatService.broadcastMessage(this.msgDto);
        this.msgDto.message = '';
      }
    }
  }

  addToInbox(obj: MessageDto) {
    let newObj = new MessageDto();
    newObj.sender = obj.sender;
    newObj.receiver = obj.receiver;
    newObj.message = obj.message;
    this.msgInboxArray.push(newObj);
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
    
}