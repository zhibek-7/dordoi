import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormControl, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';

import { Selectable } from 'src/app/shared/models/selectable.model';

import { Locale } from 'src/app/models/database-entities/locale.type';
import { LanguageService } from 'src/app/services/languages.service';

import { TimeZone } from 'src/app/models/database-entities/timeZone.type';

import { UserService } from 'src/app/services/user.service';

import { UserProfile } from 'src/app/models/DTO/userProfile.type';
import { userPasswordChange } from '../../models/userPasswordChange.model';


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(private router: Router,
    private userService: UserService,
    private languageService: LanguageService) { }

  hide: boolean;
  isUniqueEmailConfirmed: boolean;
  isPasswordChanged: boolean;
  isDeleteUserError: boolean;
  profileFormGroup: FormGroup;
  passwordChangeFormGroup: FormGroup;

  availableLocales: Selectable<Locale>[] = [];

  user: UserProfile;
  imageUrl;

  timeZones: [number, string];


  ngOnInit() {
    this.userService.getProfile()
      .subscribe(user =>
      {
        this.user = user;
        this.initForm();
        this.loadAvailableLanguages();
        //this.initTimeZones();
        //
        console.log(this.user);
        //
      },
      error => console.error(error));
  }

  initForm() {
    this.hide = true;
    this.isUniqueEmailConfirmed = false;
    this.isPasswordChanged = false;
    this.isDeleteUserError = false;
    this.imageUrl = this.user.photo ? this.user.photo.toString() : "../../assets/images/user-picture.png";
    console.log("this.imageUrl:" + this.imageUrl);

    this.passwordChangeFormGroup = new FormGroup(
      {
        "passwordCurrentFormControl": new FormControl("", Validators.required),
        "passwordNewFormControl": new FormControl("",
          [
            Validators.required,
            Validators.pattern("(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{8,}")
          ]),
        "passwordNewConfirmFormControl": new FormControl("", Validators.required)
      });

    this.profileFormGroup = new FormGroup(
      {
        "emailFormControl": new FormControl(this.user.email,
          [
            Validators.required,
            Validators.email
          ]),
        "fullNameFormControl": new FormControl(this.user.full_name),
        "photoFormControl": new FormControl(this.user.photo),
        //"timeZoneFormControl": new FormControl(this.user.id_time_zones),
        "aboutMeFormControl": new FormControl(this.user.about_me),
        "genderFormControl": new FormControl(this.user.gender == true ? "true" : this.user.gender == false ? "false" : "null")
      });
  }

  deleteUser() {
    this.userService.delete()
      .subscribe(result => {
        result
          ? this.router.navigate(['/'])
          : this.isDeleteUserError = true;
        },
      error => {
        this.isDeleteUserError = true;
        console.error(error);
      });
  }

  //initTimeZones() { }

  setSelectedTimeZone(setTimeZoneId: number) {
    this.user.id_time_zones = setTimeZoneId;
    console.log("ProfileComponent. setSelectedTimeZone: ");
    console.log(this.user.id_time_zones);
  }

  //#region Работа с Locales

  loadAvailableLanguages() {
    //let userLocalesIdIsNative: number[] = this.user.localesIdIsNative.map(([item1, item2]): number => { return item1; });
    this.languageService.getLanguageList()
      .subscribe(locale =>
      {
        this.availableLocales = locale
          .map(local =>
            new Selectable<Locale>(
              local,
              this.user.locales_ids ? this.user.locales_ids.some(selectedLocaleId => selectedLocaleId == local.id) : false
              //userLocalesIdIsNative.some(selectedLocaleId => selectedLocaleId == local.id)
            ));
      },
        error => console.error(error));
  }

  setSelectedLocales(newSelection: Locale[]) {
    this.user.locales_ids = newSelection.map(t => t.id);
  }

  //#endregion

  submit()
  {
    if (this.profileFormGroup.invalid)
    {
      return;
    }

    if (this.profileFormGroup.valid)// && this.isUniqueEmailConfirmed
    {
      let user = this.getUser();
      //
      console.log("submit():");
      console.log(this.user);
      //
      this.userService.toSaveEditedProfile(user)
        .subscribe(() => { console.log("Save"); }, error => console.error(error));
    }
  }

  isUniqueEmail(event: any) {
    let controlEmail = <AbstractControl>this.profileFormGroup.controls.emailFormControl;
    if (controlEmail.errors == null) {
      this.userService.isUniqueEmail(this.profileFormGroup.controls.emailFormControl.value)
        .subscribe(unique => {
          this.isUniqueEmailConfirmed = unique;
          unique == true ? null : controlEmail.setErrors({ "isUniqueEmail": true });
        },
          error => {
            console.error(error);
          });
    }
  }

  deleteImage() {
    //this.profileFormGroup.controls.photoFormControl.setValue = null;
    this.user.photo = null;
  }

  getUser(): UserProfile {
    this.user.email = this.profileFormGroup.controls.emailFormControl.value;
    this.user.full_name = this.profileFormGroup.controls.fullNameFormControl.value;
    this.user.gender = this.profileFormGroup.controls.genderFormControl.value;
    this.user.about_me = this.profileFormGroup.controls.aboutMeFormControl.value;

    //this.user.id_time_zones = this.profileFormGroup.controls.timeZoneFormControl.value;


    return this.user;
  }

  //#region Смена пароля
  passwordChange()
  {
    //this.passwordsValidation();

    if (this.passwordChangeFormGroup.invalid)
      return;


    if (this.passwordChangeFormGroup.valid) {

      let passwordCurrent = <AbstractControl>this.passwordChangeFormGroup.controls.passwordCurrentFormControl;
      let passwordNew = <AbstractControl>this.passwordChangeFormGroup.controls.passwordNewFormControl;
      let user = new userPasswordChange();
      user.passwordCurrent = passwordCurrent.value;
      user.passwordNew = passwordNew.value;

      this.userService.passwordChange(user)
        .subscribe(result => {
          if (result)
          {
            this.isPasswordChanged = result;
            this.passwordChangeFormGroup.reset();
          }
          else
          {
            passwordCurrent.setErrors({ "passwordCurrentInvalid": !result });
          }
        },
        error => console.error(error));

    }
  }


  confirmPassword(event: any) //passwordsValidation()
  {
    //let passwordCurrent = <AbstractControl>this.passwordChangeFormGroup.controls.passwordCurrentFormControl;
    //passwordCurrent.value ? null : passwordCurrent.setErrors({ "required": true });

    let passwordNew = <AbstractControl>this.passwordChangeFormGroup.controls.passwordNewFormControl;
    //passwordNew.value ? null : passwordNew.setErrors({ "required": true });

    let passwordNewConfirm = <AbstractControl>this.passwordChangeFormGroup.controls.passwordNewConfirmFormControl;
    //passwordNewConfirm.value ? null : passwordNewConfirm.setErrors({ "required": true });
    passwordNew.value === passwordNewConfirm.value ? null : passwordNewConfirm.setErrors({ "passwordNewConfirm": true });
  }
  //#endregion
}
