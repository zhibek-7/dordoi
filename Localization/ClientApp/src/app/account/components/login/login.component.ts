import { Component, OnInit } from "@angular/core";
import { FormControl, Validators, FormGroup } from "@angular/forms";
import { Router } from "@angular/router";

import { UserService } from "src/app/services/user.service";
import { AuthenticationService } from "src/app/services/authentication.service";
import { InvitationsService } from "src/app/services/invitations.service";
import { ParticipantsService } from "src/app/services/participants.service";

import { User } from "src/app/models/database-entities/user.type";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"]
})
export class LoginComponent implements OnInit {
  constructor(
    private router: Router,
    private userService: UserService,
    private authenticationService: AuthenticationService,
    private invitationsService: InvitationsService,
    private participantsService: ParticipantsService
  ) {}

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
      this.userService.login(user).subscribe(
        async response => {
          this.authenticationService.saveToken(response.token);

          if (this.invitationsService.currentInvitationId) {
            const currentInvitation = await this.invitationsService
              .getInvitationById(this.invitationsService.currentInvitationId)
              .toPromise();
            const currentUserProfile = await this.userService
              .getProfile()
              .toPromise();
            if (currentUserProfile.email == currentInvitation.email) {
              this.invitationsService
                .activateInvitation(this.invitationsService.currentInvitationId)
                .subscribe(
                  () => (this.invitationsService.currentInvitationId = null)
                );
            }
          }

          //this.userService.getProfile().subscribe();
          this.router.navigate(["/Profile"]);

          this.authenticationService.authorizeUser();
          this.authenticationService.setUserRole(response.role);
          this.authenticationService.setUserName(response.username);
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

  logOut() {
    this.authenticationService.logOut();
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
  //  this.router.navigate(["/account/profile"]);
  //}
}
