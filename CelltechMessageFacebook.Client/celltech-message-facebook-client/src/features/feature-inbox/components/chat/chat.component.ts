import {Component, Input, OnDestroy, OnInit} from "@angular/core";
import {SignalrService} from "../../../../services/signalr.service";
import {Subject, takeUntil} from "rxjs";
import {MessageService} from "../../../../services/message.service";
import {NgForOf} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {sendMessage} from "@microsoft/signalr/dist/esm/Utils";

@Component({
  selector: 'app-chat',
  standalone: true,
  templateUrl: 'chat.component.html',
  imports: [
    NgForOf,
    FormsModule
  ]
})
export class ChatComponent implements OnInit, OnDestroy {
  messages: any[] = [];
  chatMessage = '';
  currentUser: any | null = null;

  @Input() set conversationId(value: string) {
    this._conversationId = value;
    this.messageService.list({
      conversationId: value
    })
      .subscribe({
        next: (messages) => {
          this.messages = messages;
        }
      });
  }
  _conversationId: string;

  constructor(
    private readonly signalrService: SignalrService,
    private readonly messageService: MessageService
  ) {}

  private destroy$ = new Subject<void>();

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.signalrService.newMessage$.pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (message) => {
          if (message.conversationId === this._conversationId) {
            this.messages = [...this.messages, message];
          }
        }
      });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public sendMessage() {
    this.messageService.reply({
      content: this.chatMessage,
      senderId: this.currentUser?.id,
      conversationId: this._conversationId,
    })
      .subscribe({
        next: (message) => {
          this.messages = [...this.messages, message];
        },
      });
  }
}
