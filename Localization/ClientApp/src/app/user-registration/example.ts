import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroupDirective, NgForm, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';

@Component({
  selector: 'app-new-account',
  templateUrl: './new-account.component.html',
  styleUrls: ['./new-account.component.scss']
})
export class NewAccountComponent implements OnInit {
  hide = true;
  usernameFormControl = new FormControl();
  passwordFormControl = new FormControl();
  passwordConfirmFormControl = new FormControl();

  ngOnInit() {
    this.usernameFormControl = new FormControl('', [
      Validators.required,
      Validators.pattern('^[a-zA-Z0-9\.]+$')
    ]);
    this.passwordFormControl = new FormControl('', [
      Validators.required,
      Validators.pattern('(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{8,}')
    ]);
    this.passwordConfirmFormControl = new FormControl('', [
      Validators.required,
      passwordConfirmValidator(this.passwordFormControl)
    ]);

  }
  constructor() { }
  matcher = new MyErrorStateMatcher();


}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

function passwordConfirmValidator(password: any): ValidatorFn {
  return (control: AbstractControl): { [key: string]: boolean } | null => {
    if (control.value !== undefined && (control.value !== password.value)) {
      return { 'passwordConfirm': true };
    }
    return null;
  };
}
