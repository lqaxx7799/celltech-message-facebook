import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import {SignalrService} from "../services/signalr.service";
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'celltech-message-facebook-client';
  hasSetUpSignalr = false;

  constructor(
    private readonly signalRService: SignalrService,
    private readonly userService: UserService,
  ) {}

  ngOnInit() {
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser) {
      this.userService.currentUser$.next(currentUser);
    }

    this.userService.currentUser$.subscribe({
      next: (user) => {
        if (user && !this.hasSetUpSignalr) {
          this.signalRService.startConnection(user.id);
          this.signalRService.addMessageReceivedListener();
          this.hasSetUpSignalr = true;
        }
      }
    });

  }
}
