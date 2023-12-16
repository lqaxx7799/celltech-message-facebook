import { Routes } from '@angular/router';
import {FeatureStartUpComponent} from "../features/feature-start-up/feature-start-up.component";
import {FeatureInboxComponent} from "../features/feature-inbox/feature-inbox.component";
import { FeatureShareComponent } from '../features/feature-share/feature-share.component';

export const routes: Routes = [
  {
    path: 'start-up',
    component: FeatureStartUpComponent,
  },
  {
    path: 'inbox/:conversationId',
    component: FeatureInboxComponent,
  },
  {
    path: 'inbox',
    component: FeatureInboxComponent,
  },
  {
    path: 'share',
    component: FeatureShareComponent,
  },
];
