import {Injectable} from "@angular/core";
import * as signalR from "@microsoft/signalr";
import {BehaviorSubject} from "rxjs";

@Injectable()
export class SignalrService {
  public newMessage$ = new BehaviorSubject<any>(null);
  public newConversation$ = new BehaviorSubject<any>(null);

  private hubConnection: signalR.HubConnection;
  public startConnection = (userId: string) => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7297/hub?userId=' + userId)
      .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public addMessageReceivedListener = () => {
    this.hubConnection.on('messageReceived', (data) => {
      this.newMessage$.next(data);
    });
    this.hubConnection.on('newConversation', (data) => {
      this.newConversation$.next(data);
    });
  }
}
