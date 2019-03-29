import { Component, OnInit } from '@angular/core';
import { FormGroup,
         FormControl,
         Validators,
         AbstractControl } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { LanguageService } from 'src/app/services/languages.service';
import { UserProfile } from 'src/app/models/DTO/userProfile.type';
import { Locale } from 'src/app/models/database-entities/locale.type';

@Component({
  selector: 'app-new-profile',
  templateUrl: './new-profile.component.html',
  styleUrls: ['./new-profile.component.css']
})
export class NewProfileComponent implements OnInit {
  imageUrl = '';
  public = false;
  user: UserProfile;
  profileFormGroup: FormGroup;
  selectedCurrentLanguage;
  selectedTranslateLanguage;
  languages: Locale[] = [];
  switch = false;
  switch2 = false;

  constructor(private languagesService: LanguageService, private userService: UserService) {}

  ngOnInit() {
    this.imageUrl = '../../assets/svg/011-user.svg';
    this.userService.getProfile().subscribe(
      user => {
        this.user = user;
        this.initForm();
        if (user.photo) {
          this.imageUrl = user.photo;
        }
      },
      error => console.error(error)
    );
    this.loadLanguages();
  }

  loadLanguages() {
    this.languagesService.getLanguageList().subscribe({
      next: response => {
        this.languages = response;
      },
      error: err => console.log(err)
    });
  }

  save() {
    if (this.profileFormGroup.invalid) {
      return;
    }

    if (this.profileFormGroup.valid) {
      const user = this.getUser();
      this.userService
        .toSaveEditedProfile(user)
        .subscribe(() => {}, error => console.error(error));
    }
  }

  getUser(): UserProfile {
    this.user.full_name = this.profileFormGroup.controls.fullNameFormControl.value;
    this.user.email = this.profileFormGroup.controls.emailFormControl.value;
    this.user.about_me = this.profileFormGroup.controls.aboutMeFormControl.value;
    return this.user;
  }

  initForm() {
    this.profileFormGroup = new FormGroup({
      emailFormControl: new FormControl(this.user.email, [
        Validators.required,
        Validators.email
      ]),
      fullNameFormControl: new FormControl(this.user.full_name, [
        Validators.required
      ]),
      aboutMeFormControl: new FormControl(this.user.about_me),
      messengersFormControl: new FormControl(),
    });
  }

  isUniqueEmail() {
    const controlEmail = <AbstractControl>(
      this.profileFormGroup.controls.emailFormControl
    );
    if (controlEmail.errors == null) {
      this.userService
        .isUniqueEmail(this.profileFormGroup.controls.emailFormControl.value)
        .subscribe(
          unique => {
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

  isUniqueName() {
    const controlUserName = <AbstractControl>(
      this.profileFormGroup.controls.fullNameFormControl
    );
    if (controlUserName.errors == null) {
      this.userService
        .isUniqueLogin(this.profileFormGroup.controls.fullNameFormControl.value)
        .subscribe(
          unique => {
            unique == true
              ? null
              : controlUserName.setErrors({ isUniqueFullName: true });
          },
          error => {
            console.error(error);
          }
        );
    }
  }

  deleteImage() {
    this.user.photo = null;
    this.imageUrl = '../../assets/svg/011-user.svg';
  }

  // Загрузка файлов
  uploadFile(files): void {
    console.log(files);
  }
}
