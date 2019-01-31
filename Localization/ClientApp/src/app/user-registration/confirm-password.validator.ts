import { AbstractControl } from '@angular/forms';

export class ConfirmPasswordValidator {
  static MatchPassword(control: AbstractControl) {
    let password = control.get('userPassword').value;
    let confPassword = control.get('confirmPassword').value;
    if (password != confPassword) {
      control.get('confirmPassword').setErrors({ ConfirmPassword: true });
    }
    else {
      return null
    }
  } 
}

