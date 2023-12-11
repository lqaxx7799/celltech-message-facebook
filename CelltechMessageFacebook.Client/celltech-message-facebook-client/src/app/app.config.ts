import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {provideHttpClient} from "@angular/common/http";
import {UserService} from "../services/user.service";
import {FacebookService} from "../services/facebook.service";
import {SignalrService} from "../services/signalr.service";
import {ConversationService} from "../services/conversation.service";
import {MessageService} from "../services/message.service";

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    UserService,
    FacebookService,
    SignalrService,
    ConversationService,
    MessageService,
  ],
};
