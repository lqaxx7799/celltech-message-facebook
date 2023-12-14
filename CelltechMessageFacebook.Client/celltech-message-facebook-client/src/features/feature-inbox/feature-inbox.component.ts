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
  currentCustomer: any = null;

  private destroy$ = new Subject<void>();

  constructor(
    private readonly conversationService: ConversationService,
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute,
    private readonly signalrService: SignalrService,
    private readonly userService: UserService
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
            this.userService.get(conversation.customerId)
              .subscribe({
                next: (customer) => {
                  this.currentCustomer = customer;
                }
              })
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
          const existingConversationIndex = this.conversations.findIndex(item => item.id === newConversation.id);
          if (existingConversationIndex === -1) {
            this.conversations = [...this.conversations, newConversation];
          } else {
            this.conversations[existingConversationIndex] = {
              ...this.conversations[existingConversationIndex],
              lastMessage: newConversation.lastMessage
            };
          }
        }
      });
  }

  logout() {
    localStorage.clear();
    this.router.navigate(['/start-up']).then();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
