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
  file: File;

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
      media: 'https://pe-images.s3.amazonaws.com/basics/cc/image-size-resolution/resize-images-for-print/image-cropped-8x10.jpg',
      // href: 'https://vuonan.com.vn/hoa-don-ban-le?order=af3effc6-db1b-4410-9b6f-64ff968efc8b&session=517dfa80-ff97-4851-82c3-1e5504a9e225',
      quote: 'hehe',
      caption: 'Ok?'
    }, (response) => {
      console.log(1111111, response);
    });
  }

  openFeedDialog() {
    // window.FB.ui({
    //   method: 'feed',
    //   // href: 'https://google.com',
    //   media: this.file
    //   // media: 'https://www.seiu1000.org/sites/main/files/main-images/camera_lense_0.jpeg'
    // }, (response) => {
    //   console.log(1111111, response);
    // });
     // Dynamically gather and set the FB share data. 
     var FBDesc      = 'Your custom description';
     var FBTitle     = 'Your custom title';
     var FBLink      = 'https://vuonan.com.vn/hoa-don-ban-le?order=af3effc6-db1b-4410-9b6f-64ff968efc8b&session=517dfa80-ff97-4851-82c3-1e5504a9e225';
     var FBPic       = 'https://pe-images.s3.amazonaws.com/basics/cc/image-size-resolution/resize-images-for-print/image-cropped-8x10.jpg';

     // Open FB share popup
    window.FB.ui({
         method: 'share_open_graph',
         action_type: 'og.shares',
         display: "popup",
         action_properties: JSON.stringify({
             object: {
                 'og:url': FBLink,
                 'og:title': FBTitle,
                 'og:description': FBDesc,
                 'og:image': FBPic
             }
         })
     },
     function (response) {
     // Action after response
     })
  }

  connectFacebookUser() {
    window.FB.getLoginStatus((response) => {
      console.log(111112, response);
    });
  }

  onUploadFile(event: Event) {
    this.file = (event.target as HTMLInputElement).files[0];
  }
}