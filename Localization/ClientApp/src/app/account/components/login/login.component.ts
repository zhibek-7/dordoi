import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, AbstractControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

import { UserService } from 'src/app/services/user.service';
import { User } from 'src/app/models/database-entities/user.type';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private router: Router, private userService: UserService) { }

  hide: boolean;
  formGroup: FormGroup;

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.hide = true;
    this.formGroup = new FormGroup(
      {
        "nameFormControl": new FormControl("",
          Validators.required),
        "passwordFormControl": new FormControl("",
          Validators.required)
      }
    );
  }

  submit()
  {
    if (this.formGroup.invalid)
    {
      return;
    }

    if (this.formGroup.valid)
    {
      let user = this.getUser();
      this.userService.login(user)
        .subscribe(user => this.checkExistUser(<User>user),
        error => console.error(error));
    }
  }

  getUser(): User {
    let user: User = new User();
    user.name = this.formGroup.controls.nameFormControl.value;
    user.email = this.formGroup.controls.nameFormControl.value;
    user.password = this.formGroup.controls.passwordFormControl.value;
    return user;
  }

  checkExistUser(user: User)
  {
    if (user == null)
    {
      this.formGroup.setErrors({ "login": true });
      return;
    }
    sessionStorage.setItem('currentUserID', user.id.toString());
    this.router.navigate(['/account/' + user.id]);
  }
}
