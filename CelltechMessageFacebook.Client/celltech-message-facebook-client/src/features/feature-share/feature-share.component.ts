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
      media: 'https://th.bing.com/th/id/OIG.ey_KYrwhZnirAkSgDhmg',
      // href: 'https://vuonan.com.vn/hoa-don-ban-le?order=af3effc6-db1b-4410-9b6f-64ff968efc8b&session=517dfa80-ff97-4851-82c3-1e5504a9e225',
      quote: 'hehe',
      caption: 'Ok?'
    }, (response) => {
      console.log(1111111, response);
    });
  }

  openFeedDialog() {
    window.FB.ui({
      method: 'feed',
      // href: 'https://google.com',
      media: 'https://www.seiu1000.org/sites/main/files/main-images/camera_lense_0.jpeg'
    }, (response) => {
      console.log(1111111, response);
    });
  }

  connectFacebookUser() {
    window.FB.getLoginStatus((response) => {
      console.log(111112, response);
    });
  }
}