import { Component, OnInit } from "@angular/core";
import {
  FormControl,
  Validators,
  AbstractControl,
  FormGroup
} from "@angular/forms";
import { Router } from "@angular/router";

import { UserService } from "src/app/services/user.service";
import { User } from "src/app/models/database-entities/user.type";

import { AuthenticationService } from "src/app/services/authentication.service";
import { InvitationsService } from "src/app/services/invitations.service";

@Component({
  selector: "app-registration",
  templateUrl: "./registration.component.html",
  styleUrls: ["./registration.component.css"]
})
export class RegistrationComponent implements OnInit {
  constructor(
    private router: Router,
    private userService: UserService,
    private authenticationService: AuthenticationService,
    private invitationsService: InvitationsService
  ) {}

  hidePassword: boolean;
  formGroup: FormGroup;
  isUniqueEmailConfirmed: boolean;
  isUniqueLoginConfirmed: boolean;

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.isUniqueEmailConfirmed = false;
    this.isUniqueLoginConfirmed = false;

    this.hidePassword = true;
    this.formGroup = new FormGroup({
      emailFormControl: new FormControl("", [
        Validators.required,
        Validators.email
      ]),
      nameFormControl: new FormControl("", [
        Validators.required,
        Validators.pattern("^[a-zA-Z0-9._]+$")
      ]),
      passwordFormControl: new FormControl("", [
        Validators.required,
        Validators.pattern(
          "(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{8,}"
        )
      ]),
      passwordConfirmFormControl: new FormControl("", Validators.required),
      agreeFormControl: new FormControl("", Validators.requiredTrue)
    });
  }

  submit() {
    if (this.formGroup.invalid) {
      return;
    }

    if (
      this.formGroup.valid &&
      this.isUniqueEmailConfirmed &&
      this.isUniqueLoginConfirmed
    ) {
      let user = this.getUser();

      //      this.userService.createUser(user).subscribe(
      //        newId => {
      //          this.router.navigate(["/account"]);
      //        },
      //        error => console.error(error)
      //      );
      this.userService.createUser(user).subscribe(
        async response => {
          this.authenticationService.saveToken(response.token);

          if (this.invitationsService.currentInvitationId) {
            const currentInvitation = await this.invitationsService
              .getInvitationById(this.invitationsService.currentInvitationId)
              .toPromise();
            const currentUserProfile = await this.userService
              .getProfile()
              .toPromise();
            if (
              currentInvitation != null &&
              currentUserProfile != null &&
              currentUserProfile.email == currentInvitation.email
            ) {
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
      );
    }
  }

  /**
   * Проверка уникальности email.
   * @param event
   */
  isUniqueEmail(event: any) {
    let controlEmail = <AbstractControl>(
      this.formGroup.controls.emailFormControl
    );
    if (controlEmail.errors == null) {
      this.userService
        .isUniqueEmail(this.formGroup.controls.emailFormControl.value)
        .subscribe(
          unique => {
            this.isUniqueEmailConfirmed = unique;
            unique == true
              ? null
              : controlEmail.setErrors({ isUniqueEmail: true });
          },
          error => {
            console.error(error);
          }
        );
    }
  }

  /**
   * Проверка уникальности имени пользователя (логина).
   * @param event
   */
  isUniqueLogin(event: any) {
    let controlLogin = <AbstractControl>this.formGroup.controls.nameFormControl;
    if (controlLogin.errors == null) {
      this.userService
        .isUniqueLogin(this.formGroup.controls.nameFormControl.value)
        .subscribe(
          unique => {
            this.isUniqueLoginConfirmed = unique;
            unique == true
              ? null
              : controlLogin.setErrors({ isUniqueLogin: true });
          },
          error => {
            console.error(error);
          }
        );
    }
  }

  /**
   * Проверка совпадения введенного пароля и введеного подтверждения пароля.
   * @param event
   */
  confirm(event: any) {
    let password = this.formGroup.controls.passwordFormControl.value;
    let passwordConfirm = this.formGroup.controls.passwordConfirmFormControl
      .value;
    let controlPasswordConfirm = <AbstractControl>(
      this.formGroup.controls.passwordConfirmFormControl
    );
    return password === passwordConfirm
      ? null
      : controlPasswordConfirm.setErrors({ passwordConfirmFormControl: true });
  }

  /** Получение введенных данных с формы. */
  getUser(): User {
    let user: User = new User();
    user.name_text = this.formGroup.controls.nameFormControl.value;
    user.email = this.formGroup.controls.emailFormControl.value;
    user.password_text = this.formGroup.controls.passwordFormControl.value;
    return user;
  }
}
