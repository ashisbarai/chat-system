import * as signalR from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { MessageDto } from './message';
import { AuthService } from '../core/auth-service.service';
import { Constants } from '../constants';


@Injectable()


export class ChatService {

    private  connection: any = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:44391/chatsocket", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
     .configureLogging(signalR.LogLevel.Information)
    .build();
    readonly POST_URL = "https://localhost:44391/api/chat/send"
 
   private receivedMessageObject: MessageDto = new MessageDto();
   private sharedObj = new Subject<MessageDto>();
 
   constructor(private http: HttpClient, private authService: AuthService) { 
     this.connection.onclose(async () => {
       await this.start();
     });
    this.connection.on("ReceiveOne", (receiver, sender, message) => { this.mapReceivedMessage(receiver, sender, message); });
    this.start();                 
   }

 
   // Strart the connection
   public async start() {
     try {
       await this.connection.start();
       console.log("connected");
       this.connection.invoke('getConnectionId', +this.authService.authContext.claims.find((a)=>a.type == 'sub').value)
       .then(function (connectionId) {
          sessionStorage.setItem('conectionId', connectionId);
           // Send the connectionId to controller
       }).catch(err => console.error(err.toString()));
     } catch (err) {
       console.log(err);
       setTimeout(() => this.start(), 5000);
     } 
   }
 
   private mapReceivedMessage(receiver: number, sender: number, message: string): void {
     this.receivedMessageObject.receiver = receiver;
     this.receivedMessageObject.sender = sender;
     this.receivedMessageObject.message = message;
     this.sharedObj.next(this.receivedMessageObject);
  }
 
   /* ****************************** Public Mehods **************************************** */
 
   // Calls the controller method
   public broadcastMessage(msgDto: MessageDto) {
       console.log(this.authService.authContext.claims.find((a)=>a.type == 'sub').value);
     //this.http.post(this.POST_URL, msgDto).subscribe(data => console.log(data));

     var connectionId = sessionStorage.getItem('conectionId');
     this.connection.invoke("Send", msgDto.sender, msgDto.receiver, msgDto.message);

     // this.connection.invoke("SendMessage1", msgDto.user, msgDto.msgText).catch(err => console.error(err));    // This can invoke the server method named as "SendMethod1" directly.
   }
 
   public retrieveMappedObject(): Observable<MessageDto> {
     return this.sharedObj.asObservable();
   }
   getUsersChatByUserId(userId: number, friendId: number): Observable<MessageDto[]> {
    return this.http.get<MessageDto[]>(Constants.apiRoot + 'GetUsersChatByUserId/' + userId + '/' + friendId);
  }
 
 }