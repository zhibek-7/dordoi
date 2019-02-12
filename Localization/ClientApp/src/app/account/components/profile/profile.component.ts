import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { Validators, FormGroup, FormControl, AbstractControl } from '@angular/forms';

import { Locale } from 'src/app/models/database-entities/locale.type';
import { LanguageService } from 'src/app/services/languages.service';


import { UserProfile } from 'src/app/models/DTO/userProfile.type';
import { Selectable } from 'src/app/shared/models/selectable.model';
import { Router } from '@angular/router';

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
  profileFormGroup: FormGroup;
  passwordChangeFormGroup: FormGroup;

  availableLocales: Selectable<Locale>[] = [];

  user: UserProfile;

  timeZones: [number, string];

  ngOnInit() {
    this.initTimeZones();
    // this.userService.getProfile()
    //   .subscribe(user =>
    //   {
    //     // this.user = user;  
    //     this.initForm();
    //     this.loadAvailableLanguages();
    //     console.log(this.user);
    //   },
    //   error => console.error(error));
  
  }

  initTimeZones() {

  }

  initForm() {
    this.hide = true;
    this.isUniqueEmailConfirmed = false;

    this.passwordChangeFormGroup = new FormGroup(
      {
        "passwordCurrentFormControl": new FormControl(""),
        "passwordNewFormControl": new FormControl("",
          Validators.pattern("(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{8,}")),
        "passwordNewConfirmFormControl": new FormControl("")
      });

    this.profileFormGroup = new FormGroup(
      {
        "emailFormControl": new FormControl(this.user.email,
          [
            Validators.required,
            Validators.email
          ]),
        "fullNameFormControl": new FormControl(this.user.fullName),
        "photoFormControl": new FormControl(this.user.photo),
        "timeZoneFormControl": new FormControl(this.user.timeZone),
        "aboutMeFormControl": new FormControl(this.user.aboutMe),
        "genderFormControl": new FormControl(this.user.gender == true ? "true" : this.user.gender == false ? "false" : "null"),

        
        //"localesIdsFormControl": new FormControl(""),
        //"localesIdIsNativeFormControl": new FormControl("")
      });
  }

  deleteUser() {
    this.userService.delete()
      .subscribe(() => {
        this.router.navigate(['/']);
      },
        error => console.error(error));
  }


  //#region Работа с Locales

  loadAvailableLanguages() {
    //let userLocalesIdIsNative: number[] = this.user.localesIdIsNative.map(([item1, item2]): number => { return item1; });
    this.languageService.getLanguageList()
      .subscribe(locale =>
      {
        console.log(locale);
        this.availableLocales = locale
          .map(local =>
            new Selectable<Locale>(
              local,
              this.user.localesIds.some(selectedLocaleId => selectedLocaleId == local.id)
              //userLocalesIdIsNative.some(selectedLocaleId => selectedLocaleId == local.id)
            ));
      },
        error => console.error(error));
  }

  setSelectedLocales(newSelection: Locale[]) {
    this.user.localesIds = newSelection.map(t => t.id);
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


  getUser(): UserProfile {
    let user: UserProfile = new UserProfile();
    user.id = this.user.id;

    user.email = this.profileFormGroup.controls.emailFormControl.value;
    user.fullName = this.profileFormGroup.controls.fullNameFormControl.value;
    user.aboutMe = this.profileFormGroup.controls.aboutMeFormControl.value;

    user.timeZone = this.profileFormGroup.controls.timeZoneFormControl.value;

    //user.gender = this.formGroup.value;
    return user;
  }

  //#region Смена пароля
  passwordChange()
  {
    this.passwordsValidation();

    if (this.passwordChangeFormGroup.invalid)
      return;
  }


  passwordsValidation()//event: any
  {
    let passwordCurrent = <AbstractControl>this.passwordChangeFormGroup.controls.passwordCurrentFormControl;
    passwordCurrent.value ? null : passwordCurrent.setErrors({ "required": true });

    let passwordNew = <AbstractControl>this.passwordChangeFormGroup.controls.passwordNewFormControl;
    passwordNew.value ? null : passwordNew.setErrors({ "required": true });

    let passwordNewConfirm = <AbstractControl>this.passwordChangeFormGroup.controls.passwordNewConfirmFormControl;
    passwordNewConfirm.value ? null : passwordNewConfirm.setErrors({ "required": true });    
    return passwordNew.value === passwordNewConfirm.value ? null : passwordNewConfirm.setErrors({ "passwordNewConfirm": true });
  }
  //#endregion
}
