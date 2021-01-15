import * as signalR from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { MessageDto } from './message';
import { AuthService } from '../core/auth-service.service';
import { Constants } from '../constants';


@Injectable()


export class ChatService {

    private  connection: any;
    readonly POST_URL = "https://localhost:44391/api/SendMessage"
 
   private receivedMessageObject: MessageDto = new MessageDto();
   private sharedObj = new Subject<MessageDto>();
 
   constructor(private http: HttpClient, private authService: AuthService) { 

    this.buildConnection();
      this.connection.onclose(async () => {
        await this.start();
      });
     this.connection.on("ReceiveOne", (messageInfo: MessageDto) => { this.mapReceivedMessage(messageInfo); });
     this.start();             
   }

   private buildConnection() {
    return this.connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:44391/chatsocket", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => {
          return this.authService.getAccessToken();
        }
      })
     .configureLogging(signalR.LogLevel.Information)
    .build();
  }

 
   // Strart the connection
   public async start() {
     try {
       await this.connection.start();
       //console.log("connected");
       //this.connection.invoke('getConnectionId', +this.authService.authContext.claims.find((a)=>a.type == 'sub').value)
       //.then(function (connectionId) {
          //sessionStorage.setItem('conectionId', connectionId);
           // Send the connectionId to controller
       //}).catch(err => console.error(err.toString()));
     } catch (err) {
       console.log(err);
       setTimeout(() => this.start(), 5000);
     } 
   }
 
   private mapReceivedMessage(messageInfo: MessageDto): void {
     this.receivedMessageObject.id = messageInfo.id;
     this.receivedMessageObject.createdOn = messageInfo.createdOn;
     this.receivedMessageObject.userId = messageInfo.userId;
     this.receivedMessageObject.friendId = messageInfo.userId;
     this.receivedMessageObject.message = messageInfo.message;
     this.receivedMessageObject.messageType = messageInfo.messageType;
     //console.log(this.receivedMessageObject);
     this.sharedObj.next(this.receivedMessageObject);
  }
 
   /* ****************************** Public Mehods **************************************** */
 
   // Calls the controller method
   public sendMessage(msgDto: MessageDto) {
       //console.log(this.authService.authContext.claims.find((a)=>a.type == 'sub').value);
     //this.http.post(this.POST_URL, msgDto).subscribe(data => console.log(data));

     //var connectionId = sessionStorage.getItem('conectionId');
     this.connection.invoke("Send", msgDto.userId, msgDto.friendId, msgDto.message);

     // this.connection.invoke("SendMessage1", msgDto.user, msgDto.msgText).catch(err => console.error(err));    // This can invoke the server method named as "SendMethod1" directly.
   }
 
   public retrieveMappedObject(): Observable<MessageDto> {
     return this.sharedObj.asObservable();
   }
   getUsersChatByUserId(userId: number, friendId: number): Observable<MessageDto[]> {
    return this.http.get<MessageDto[]>(Constants.apiRoot + 'GetUsersChatByUserId/' + userId + '/' + friendId);
  }
  deleteChatById(id: number): any {
    return this.http.delete<any>(Constants.apiRoot + 'DeleteChatById/' + id);
  }
  deleteUsersChatByUserId(userId: number, friendId: number): any {
    return this.http.delete<any>(Constants.apiRoot + 'DeleteUsersChatByUserId/' + userId + '/' + friendId);
  }
 }