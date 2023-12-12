import {Component, OnDestroy, OnInit} from '@angular/core';
import {ConversationService} from "../../services/conversation.service";
import {ChatComponent} from "./components/chat/chat.component";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {NgFor, NgIf} from "@angular/common";
import { UserService } from '../../services/user.service';
import { SignalrService } from '../../services/signalr.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-feature-inbox',
  templateUrl: 'feature-inbox.component.html',
  standalone: true,
  imports: [ChatComponent, NgIf, NgFor, RouterLink]
})
export class FeatureInboxComponent implements OnInit, OnDestroy {
  conversations: any[] = [];
  currentUser: any = null;
  conversationId: string | null = null;
  currentConversation: any = null;

  private destroy$ = new Subject<void>();

  constructor(
    private readonly conversationService: ConversationService,
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    private readonly signalrService: SignalrService,
  ) {}

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (!this.currentUser) {
      return;
    }
    this.activatedRoute.paramMap.subscribe(p => {
      const newConversationId = p.get('conversationId');

      if (this.conversationId !== newConversationId) {
        this.conversationId = newConversationId;
      }
    });

    this.conversationService.list({ userId: this.currentUser.id })
      .subscribe({
        next: (conversations) => {
          this.conversations = conversations;
        }
      });
    this.conversationId = this.activatedRoute.snapshot.paramMap.get('conversationId');
    if (this.conversationId) {
      this.conversationService.get({ id: this.conversationId })
        .subscribe({
          next: (conversation) => {
            this.currentConversation = conversation;
          }
        });
    }
    this.signalrService.newConversation$
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (newConversation) => {
          if (!newConversation) {
            return;
          }
          if (this.conversations.every(item => item.id !== newConversation.id)) {
            this.conversations = [...this.conversations, newConversation];
          }
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
