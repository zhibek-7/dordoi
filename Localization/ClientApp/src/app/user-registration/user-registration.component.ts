import { Component, OnInit } from '@angular/core';
import { UserRegistrationService } from '../services/user-registration.service';
import { FormGroup, FormGroupDirective, NgForm, FormControl, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { User } from '../user-account/user';

@Component({
  selector: 'app-user-registration',
  
  templateUrl: './user-registration.component.html',
  styles: [`
        input.ng-touched.ng-invalid {border:solid red 2px;}
        input.ng-touched.ng-valid {border:solid green 2px;}
    `],
  providers: [UserRegistrationService]
})

export class UserRegistrationComponent implements OnInit {
  hide = true;
  regForm: FormGroup;
  constructor() {
    this.regForm = new FormGroup({
      "userEmailFormContol" : new FormControl(),
      "userNameFormControl" : new FormControl(),
      "userPasswordFormControl" : new FormControl(),
      "confirmPasswordFormControl" : new FormControl()
    });
  }

  ngOnInit() {

  }
  //ngOnInit() {
  //  this.userEmailFormContol = new FormControl('', [Validators.required,
  //    Validators.pattern("[a-zA-Z_]+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}")]);
  //  this.userNameFormControl = new FormControl('', Validators.required);
  //}  
}

function equalPasswordValidator(password: any): ValidatorFn {
  return (control: AbstractControl): { [key: string]: boolean } | null => {
    if (control.value !== undefined && (control.value !== password.value)) {
      return { 'passwordConfirm': true };
    }
    return null;
  }
}


