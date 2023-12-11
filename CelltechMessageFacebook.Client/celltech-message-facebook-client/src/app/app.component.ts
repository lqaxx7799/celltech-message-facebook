import {Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import {SignalrService} from "../services/signalr.service";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'celltech-message-facebook-client';

  constructor(private readonly signalRService: SignalrService) {}

  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.addMessageReceivedListener();
  }
}
