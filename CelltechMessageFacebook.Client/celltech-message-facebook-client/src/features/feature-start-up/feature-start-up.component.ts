import {AfterViewInit, Component, OnInit} from '@angular/core';
import {FormGroup, ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators} from "@angular/forms";
import {CommonModule, NgIf} from "@angular/common";
import {UserService} from "../../services/user.service";
import {FacebookService} from "../../services/facebook.service";

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
  ]
})
export class FeatureStartUpComponent implements OnInit, AfterViewInit {
  isFacebookConnected = false;
  facebookAuthResponse: any;
  form: UntypedFormGroup;

  facebookPages: any[] = [];

  constructor(
    private readonly formBuilder: UntypedFormBuilder,
    private readonly userService: UserService,
    private readonly facebookService: FacebookService
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
    }, { scope: 'pages_show_list,pages_manage_metadata,pages_messaging,pages_read_engagement' });
  }

  public onSubmit() {
    if (!this.form.valid) {
      return;
    }

    this.userService.signUp({
      ...this.form.value,
      authResponse: this.facebookAuthResponse.authResponse,
    })
      .subscribe({
        next: (user) => {
          localStorage.setItem('currentUser', user);
          this.facebookService.getPages(user.id)
            .subscribe({
              next: (pages) => {
                this.facebookPages = pages;
              },
            });
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
