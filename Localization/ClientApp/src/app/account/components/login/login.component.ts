import { Component, OnInit } from "@angular/core";
import { FormControl, Validators, FormGroup } from "@angular/forms";
import { Router } from "@angular/router";

import { UserService } from "src/app/services/user.service";
import { AuthenticationService } from "src/app/services/authentication.service";

import { User } from "src/app/models/database-entities/user.type";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"]
})
export class LoginComponent implements OnInit {
  constructor(private router: Router, 
              private userService: UserService,
              private authenticationService: AuthenticationService) {}
  
  hidePassword: boolean;
  formGroup: FormGroup;

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.hidePassword = true;
    this.formGroup = new FormGroup({
      nameFormControl: new FormControl("", Validators.required),
      passwordFormControl: new FormControl("", Validators.required)
    });
  }

  submit() {
    if (this.formGroup.invalid) {
      return;
    }

    if (this.formGroup.valid) {
      let user = this.getUser();
      this.userService
        .login(user)
        .subscribe( response => {    
            this.authenticationService.saveToken(response.token);     
          // this.userService.getProfile().subscribe(); 
            this.router.navigate(["/account/" + user.name_text]);
          },
        error => {
          this.formGroup.setErrors({ login: true });
            //alert("Авторизация не пройдена");
          }
          
          // user => this.checkExistUser(<User>user),
          // error => console.error(error)
        );
    }
  }

  logOut(){
    this.authenticationService.deleteToken();
  }

  //getUserInfo(){
  //  this.userService.getProfile().subscribe();
  //}

  getUser(): User {
    let user: User = new User();
    user.name_text = this.formGroup.controls.nameFormControl.value;
    user.email = this.formGroup.controls.nameFormControl.value;
    user.password_text = this.formGroup.controls.passwordFormControl.value;
    return user;
  }

  //checkExistUser(user: User) {
  //  if (user == null) {
  //    this.formGroup.setErrors({ login: true });
  //    return;
  //  }
  //  this.router.navigate(["/account/" + user.id]);
  //}
}
