import {AfterViewInit, Component, OnInit} from '@angular/core';
import {FormGroup, ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators} from "@angular/forms";
import {CommonModule, NgIf} from "@angular/common";

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
  form: UntypedFormGroup;

  constructor(private readonly formBuilder: UntypedFormBuilder) {}

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
      this.isFacebookConnected = true;
    }, { scope: 'pages_show_list,pages_manage_metadata,pages_messaging,pages_read_engagement' });
  }

  private buildStartUpForm() {
    this.form = this.formBuilder.group({
      userName: ['', Validators.required],
    })
  }
}
