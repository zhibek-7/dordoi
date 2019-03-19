import { Component, OnInit } from "@angular/core";
import {
  Validators,
  FormGroup,
  FormControl,
  AbstractControl
} from "@angular/forms";

import { Router } from "@angular/router";
import { DropzoneConfigInterface } from "ngx-dropzone-wrapper";
import { Selectable } from "src/app/shared/models/selectable.model";
import { Locale } from "src/app/models/database-entities/locale.type";
import { LanguageService } from "src/app/services/languages.service";
import { UserService } from "src/app/services/user.service";
import { UserProfile } from "src/app/models/DTO/userProfile.type";
import { userPasswordChange } from "../../models/userPasswordChange.model";
import { AppConfigService } from "src/services/app-config.service";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.css"]
})
export class ProfileComponent implements OnInit {
  constructor(
    private router: Router,
    private userService: UserService,
    private languageService: LanguageService,
    private appConfigService: AppConfigService
  ) {}

  hidePassword: boolean;
  isUniqueEmailConfirmed: boolean;
  isPasswordChanged: boolean;
  isDeleteUserError: boolean;
  profileFormGroup: FormGroup;
  passwordChangeFormGroup: FormGroup;

  availableLocales: Selectable<Locale>[] = [];

  user: UserProfile;
  imageUrl;
  imageBody;

  timeZones: [number, string];

  public config: DropzoneConfigInterface = {
    url: `${this.appConfigService.config.host.protocol}://${this.appConfigService.config.host.name}/api/User/Photo`,
    paramName: "file",
    headers: { Authorization: `Bearer ${sessionStorage.getItem("userToken")}` },
    method: "post",
    maxFiles: 1,
    maxFilesize: 1,
    acceptedFiles: "image/*",
    clickable: true,
    autoReset: 1,
    errorReset: 10,
    cancelReset: 10,
    previewsContainer: false
  };

  //#region init

  ngOnInit() {
    this.userService.getProfile().subscribe(
      user => {
        this.user = user;
        this.initForm();
        this.loadAvailableLanguages();
      },
      error => console.error(error)
    );
  }

  initForm() {
    this.hidePassword = true;
    this.isUniqueEmailConfirmed = false;
    this.isPasswordChanged = false;
    this.isDeleteUserError = false;
    this.imageUrl = "../../assets/svg/011-user.svg";

    this.passwordChangeFormGroup = new FormGroup({
      passwordCurrentFormControl: new FormControl("", Validators.required),
      passwordNewFormControl: new FormControl("", [
        Validators.required,
        Validators.pattern(
          "(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{8,}"
        )
      ]),
      passwordNewConfirmFormControl: new FormControl("", Validators.required)
    });

    this.profileFormGroup = new FormGroup({
      emailFormControl: new FormControl(this.user.email, [
        Validators.required,
        Validators.email
      ]),
      fullNameFormControl: new FormControl(this.user.full_name),
      photoFormControl: new FormControl(this.user.photo),
      aboutMeFormControl: new FormControl(this.user.about_me),
      genderFormControl: new FormControl(
        this.user.gender == true
          ? "true"
          : this.user.gender == false
          ? "false"
          : "null"
      )
    });
  }

  //#endregion

  /** Получение введенных данных с формы */
  getUser(): UserProfile {
    this.user.email = this.profileFormGroup.controls.emailFormControl.value;
    this.user.full_name = this.profileFormGroup.controls.fullNameFormControl.value;
    this.user.gender =
      this.profileFormGroup.controls.genderFormControl.value == "null"
        ? null
        : this.profileFormGroup.controls.genderFormControl.value;
    this.user.about_me = this.profileFormGroup.controls.aboutMeFormControl.value;

    return this.user;
  }

  /** Сохранение изменений в учетной записи */
  submit() {
    if (this.profileFormGroup.invalid) {
      return;
    }

    if (this.profileFormGroup.valid) {
      // && this.isUniqueEmailConfirmed
      let user = this.getUser();
      this.userService
        .toSaveEditedProfile(user)
        .subscribe(() => {}, error => console.error(error));
    }
  }

  /** Удаление учетной записи */
  deleteUser() {
    this.userService.delete().subscribe(
      result => {
        result ? this.router.navigate(["/"]) : (this.isDeleteUserError = true);
      },
      error => {
        this.isDeleteUserError = true;
        console.error(error);
      }
    );
  }

  /**
   * Получение выбранного часового пояса
   * @param setTimeZoneId идентификатор выбранного часового пояса
   */
  setSelectedTimeZone(setTimeZoneId: number) {
    this.user.id_time_zones = setTimeZoneId;
  }

  //#region Работа с Locales

  loadAvailableLanguages() {
    this.languageService.getLanguageList().subscribe(
      locale => {
        locale.forEach(
          local =>
            (local.isNative = this.user.locales_id_is_native
              ? this.user.locales_id_is_native.some(
                  t => t.item1 == local.id && t.item2
                )
              : false)
        );

        this.availableLocales = locale.map(
          local =>
            new Selectable<Locale>(
              local,
              this.user.locales_id_is_native
                ? this.user.locales_id_is_native.some(
                    selectedLocale => selectedLocale.item1 == local.id
                  )
                : false
            )
        );
      },
      error => console.error(error)
    );
  }

  /**
   * Получение выбранных языков переводов с отмечеными родными языками
   * @param newSelection
   */
  setSelectedLocales(newSelection: Locale[]) {
    this.user.locales_id_is_native = newSelection.map(
      (t): { item1: number; item2: boolean } => {
        return { item1: t.id, item2: t.isNative };
      }
    );
  }

  //#endregion

  /**
   * Проверка уникальности введенного email
   * @param event
   */
  isUniqueEmail(event: any) {
    let controlEmail = <AbstractControl>(
      this.profileFormGroup.controls.emailFormControl
    );
    if (controlEmail.errors == null) {
      this.userService
        .isUniqueEmail(this.profileFormGroup.controls.emailFormControl.value)
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

  //#region Работа с фото

  /**
   * Инициализация dropzone.
   * @param args
   */
  public onUploadInit(args: any): void {
    //console.log('onUploadInit:', args);
  }

  /**
   * Ошибка загрузки изображения.
   * @param args
   */
  public onUploadError(args: any): void {
    //console.log('onUploadError:', args);
    let messageError =
      "Ошибка загрузки изображения. Попробуйте заново. Вы можете загрузить " +
      this.config.maxFiles +
      " изображение размером до " +
      this.config.maxFilesize +
      " Мб. ";
    alert(messageError);
  }

  /**
   * Изображение загружено.
   * @param args
   */
  public onUploadSuccess(args: any): void {
    //console.log('onUploadSuccess:', args);
    this.imageUrl = args[0].dataURL;
    //обрезаем дополнительную информацию о изображении и оставляем только byte[]
    this.user.photo = args[0].dataURL.match(".*base64,(.*)")[1];
  }

  /** Удаление фотографии. */
  deleteImage() {
    this.user.photo = null;
    this.imageUrl = "../../assets/svg/011-user.svg";
  }

  //#endregion

  //#region Смена пароля

  /** Проверка введеного текущего пароля. Сохранение нового пароля. */
  passwordChange() {
    if (this.passwordChangeFormGroup.invalid) return;

    if (this.passwordChangeFormGroup.valid) {
      let passwordCurrent = <AbstractControl>(
        this.passwordChangeFormGroup.controls.passwordCurrentFormControl
      );
      let passwordNew = <AbstractControl>(
        this.passwordChangeFormGroup.controls.passwordNewFormControl
      );
      let user = new userPasswordChange();
      user.passwordCurrent = passwordCurrent.value;
      user.passwordNew = passwordNew.value;

      this.userService.passwordChange(user).subscribe(
        result => {
          if (result) {
            this.isPasswordChanged = result;
            this.passwordChangeFormGroup.reset();
          } else {
            // Введен неверный текущий пароль.
            passwordCurrent.setErrors({ passwordCurrentInvalid: !result });
          }
        },
        error => console.error(error)
      );
    }
  }

  /**
   * Проверка совпадения введенного пароля и введеного подтверждения пароля.
   * @param event
   */
  confirmPassword(event: any) {
    let passwordNew = <AbstractControl>(
      this.passwordChangeFormGroup.controls.passwordNewFormControl
    );
    let passwordNewConfirm = <AbstractControl>(
      this.passwordChangeFormGroup.controls.passwordNewConfirmFormControl
    );

    passwordNew.value === passwordNewConfirm.value
      ? null
      : passwordNewConfirm.setErrors({ passwordNewConfirm: true });
  }

  //#endregion
}
