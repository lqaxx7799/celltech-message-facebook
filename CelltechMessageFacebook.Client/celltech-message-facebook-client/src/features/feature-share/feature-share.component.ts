import { AfterViewInit, Component } from "@angular/core";

declare global {
  interface Window { FB: any; }
}

@Component({
  selector: 'app-feature-share',
  templateUrl: 'feature-share.component.html',
  standalone: true,
})
export class FeatureShareComponent implements AfterViewInit {
  constructor() {}

  ngAfterViewInit() {
    window.FB.init({
      appId: '611455386786924',
      xfbml: true,
      version: 'v17.0',
    });
  }

  openShareDialog() {
    window.FB.ui({
      method: 'share',
      href: 'https://developers.facebook.com/docs/',
    }, (response) => {
      console.log(1111111, response);
    });
  }
}