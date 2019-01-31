import { Component, OnInit } from '@angular/core';
import { UserLoginService } from '../services/user-login.service';
import { FormBuilder, FormGroup, FormGroupDirective, NgForm, FormControl, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { User } from '../user-account/user';

@Component({
  selector: 'app-user-login',

  templateUrl: './user-login.component.html',
  styles: [`
        input.ng-touched.ng-invalid {border:solid red 2px;}
        input.ng-touched.ng-valid {border:solid green 2px;}
    `],
  providers: [UserLoginService]
})

export class UserLoginComponent implements OnInit {
  user: User = new User();
  loginForm: FormGroup;
  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.loginForm = this.fb.group({
      userLogin: ['', Validators.required],
      userPassword: ['', Validators.required]
    });
  }
}
