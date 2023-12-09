import { Routes } from '@angular/router';
import {FeatureStartUpComponent} from "../features/feature-start-up/feature-start-up.component";
import {FeatureInboxComponent} from "../features/feature-inbox/feature-inbox.component";

export const routes: Routes = [
  {
    path: 'start-up',
    component: FeatureStartUpComponent,
  },
  {
    path: 'inbox',
    component: FeatureInboxComponent,
  },
];
