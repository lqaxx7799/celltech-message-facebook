import {AfterViewInit, Component, OnInit} from '@angular/core';
import {
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators
} from "@angular/forms";
import {CommonModule, NgIf} from "@angular/common";
import {UserService} from "../../services/user.service";
import {FacebookService} from "../../services/facebook.service";
import {Router} from "@angular/router";

declare global {
  interface Window { FB: any; }
}


@Component({
  selector: 'app-feature-start-up',
  templateUrl: 'feature-start-up.component.html',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule,
    FormsModule,
  ]
})
export class FeatureStartUpComponent implements OnInit, AfterViewInit {
  isFacebookConnected = false;
  facebookAuthResponse: any;
  form: UntypedFormGroup;

  facebookPages: any[] = [];
  selectedFacebookPageIds: string[] = [];

  constructor(
    private readonly formBuilder: UntypedFormBuilder,
    private readonly userService: UserService,
    private readonly facebookService: FacebookService,
    private readonly router: Router
  ) {}

  ngOnInit() {
    this.buildStartUpForm();
  }

  ngAfterViewInit() {
    window.FB.init({
      appId: '611455386786924',
      xfbml: true,
      version: 'v17.0',
    });
  }

  public connectFacebook() {
    window.FB.login((response) =>
    {
      // login
      console.log(11111, response);
      this.facebookAuthResponse = response;
      this.isFacebookConnected = true;
      this.facebookService.getPages(response.authResponse.accessToken)
        .subscribe({
          next: (pages) => {
            console.log(11111, pages);
            this.facebookPages = pages.data;
          },
        });
    }, { scope: 'pages_show_list,pages_manage_metadata,pages_messaging,pages_read_engagement' });
  }

  public isPageSelected(pageId: any) {
    return this.selectedFacebookPageIds.includes(pageId);
  }

  public onChangePage(pageId: any, event: boolean) {
    if (event) {
      this.selectedFacebookPageIds = [...this.selectedFacebookPageIds, pageId];
    } else {
      this.selectedFacebookPageIds = this.selectedFacebookPageIds.filter(x => x !== pageId);
    }
  }

  public onSubmit() {
    if (!this.form.valid) {
      return;
    }

    this.userService.signUp({
      ...this.form.value,
      authResponse: this.facebookAuthResponse.authResponse,
      pages: this.selectedFacebookPageIds.map(pageId => {
        const page = this.facebookPages.find(page => page.id === pageId);
        return {
          pageName: page.name,
          pageId: page.id,
          accessToken: page.accessToken,
        };
      })
    })
      .subscribe({
        next: (user) => {
          localStorage.setItem('currentUser', user);
          this.router.navigate(['/inbox']).then();
        },
      });
  }

  private buildStartUpForm() {
    this.form = this.formBuilder.group({
      userName: ['', Validators.required],
      email: ['', Validators.required],
    });
  }
}
