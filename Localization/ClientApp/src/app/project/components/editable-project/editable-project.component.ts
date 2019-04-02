import { Component, OnInit, ElementRef, Input, Output, EventEmitter } from "@angular/core";
import { FormGroup, FormControl, Validators, AbstractControl } from "@angular/forms";

//import { Router } from "@angular/router";
import { Guid } from "guid-typescript";

import { ProjectsService } from "../../../services/projects.service";
import { LocalizationProject } from "../../../models/database-entities/localizationProject.type";
import { LocalizationProjectsLocales } from "src/app/models/database-entities/localizationProjectLocales.type";
//import { LocalizationProjectForSelectDTO } from "src/app/models/DTO/localizationProjectForSelectDTO.type";


import { Locale } from "src/app/models/database-entities/locale.type";
import { LanguageService } from "src/app/services/languages.service";

import { Selectable } from "src/app/shared/models/selectable.model";

import { ProjectsLocalesService } from "src/app/services/projectsLocales.service";

@Component({
  selector: 'app-editable-project',
  templateUrl: './editable-project.component.html',
  styleUrls: ['./editable-project.component.css']
})
export class EditableProjectComponent implements OnInit {
  @Input()
  project: LocalizationProject;

  @Output()
  editedSubmitted = new EventEmitter<[LocalizationProject, LocalizationProjectsLocales[]]>();


  allLocales: Locale[] = [];
  availableLocales: Selectable<Locale>[] = [];
  //availableLocalizationProjects: Selectable<LocalizationProjectForSelectDTO>[] = [];
  allProjectLocales: LocalizationProjectsLocales[] = [];

  loaded: boolean = false;

  form: FormGroup;

  urlPattern: string = "(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";

  constructor(
    private projectsService: ProjectsService,
    private languageService: LanguageService,
    private projectsLocalesService: ProjectsLocalesService
  ) { }


  ngOnInit() {
    this.allProjectLocales = [];

    this.loadLanguages();
  }

  initForm() {
    this.form = new FormGroup({
      nameFormControl: new FormControl(this.project.name_text, Validators.required),
      descriptionFormControl: new FormControl(this.project.description),

      urlFormControl: new FormControl(this.project.url,
        [
          Validators.required,
          Validators.pattern(
            this.urlPattern
          )
        ]),
      visibilityFormControl: new FormControl(
        this.project.visibility == true
          ? "true"
          : this.project.visibility == false
            ? "false"
            : "null"
      ),

      ableToLeftErrorsFormControl: new FormControl(this.project.able_To_Left_Errors),
      originalIfStringIsNotTranslatedFormControl: new FormControl(this.project.original_if_string_is_not_translated),
      exportOnlyApprovedTranslationsFormControl: new FormControl(this.project.export_only_approved_translations),
      ableTranslatorsChangeTermsInGlossariesFormControl: new FormControl(this.project.able_translators_change_terms_in_glossaries),

      notifyNewFormControl: new FormControl(this.project.notify_New),
      notifyFinishFormControl: new FormControl(this.project.notify_Finish),
      notifyConfirmFormControl: new FormControl(this.project.notify_Confirm),
      notifyNewCommentFormControl: new FormControl(this.project.notify_new_comment),

      defaultStringFormControl: new FormControl(this.project.default_String),

      iDSourceLocaleFormControl: new FormControl(this.project.iD_Source_Locale, Validators.required)

    });
  }
  
  async submit() {
    this.getProject();
    this.editedSubmitted.emit([this.project, this.allProjectLocales]);
  }

  getProject() {
    this.project.name_text = this.form.controls.nameFormControl.value;
    this.project.description = this.form.controls.descriptionFormControl.value;
    this.project.url = this.form.controls.urlFormControl.value;
    this.project.visibility = this.form.controls.visibilityFormControl.value == "null" ? null : this.form.controls.visibilityFormControl.value;
    this.project.able_To_Left_Errors = this.form.controls.ableToLeftErrorsFormControl.value;
    this.project.original_if_string_is_not_translated = this.form.controls.originalIfStringIsNotTranslatedFormControl.value;
    this.project.export_only_approved_translations = this.form.controls.exportOnlyApprovedTranslationsFormControl.value;
    this.project.able_translators_change_terms_in_glossaries = this.form.controls.ableTranslatorsChangeTermsInGlossariesFormControl.value;
    this.project.notify_New = this.form.controls.notifyNewFormControl.value;
    this.project.notify_Finish = this.form.controls.notifyFinishFormControl.value;
    this.project.notify_Confirm = this.form.controls.notifyConfirmFormControl.value;
    this.project.notify_new_comment = this.form.controls.notifyNewCommentFormControl.value;
    this.project.default_String = this.form.controls.defaultStringFormControl.value;
    this.project.iD_Source_Locale = this.form.controls.iDSourceLocaleFormControl.value;
  }

  //#region Работа с Locales

  loadLanguages() {
    this.languageService.getLanguageList().subscribe(
      locale => {
        this.allLocales = locale;

        //this.loadProject();

        this.loadProjectLocales();
        this.initForm();
      },
      error => console.error(error)
    );
  }

  loadProjectLocales() {
    this.projectsLocalesService.getByProjectId(this.projectsService.currentProjectId).subscribe(
      projectLocales => {
        this.allProjectLocales = projectLocales;
        this.loadAvailableLanguages();
      },
      error => console.error(error)
    );
  }

  loadAvailableLanguages() {
    this.availableLocales = this.allLocales.map(
      local =>
        new Selectable<Locale>(
          local,
          this.allProjectLocales.some(selectedLocale => selectedLocale.iD_Locale == local.id)
        )
    );

    this.loaded = true;
  }


  setSelectedLocales(newSelection: Locale[]) {
    let selectedLocales = newSelection.map(t => new LocalizationProjectsLocales(this.project.id, t.id));

    //Оставляет те что хранятся в БД, которые выбраны, что бы не потерять значения других полей
    this.allProjectLocales = this.allProjectLocales.filter(t => selectedLocales.some(n => n.iD_Locale == t.iD_Locale));

    //Добавляет только новые выбранные,
    selectedLocales = selectedLocales.filter(t => !this.allProjectLocales.some(n => n.iD_Locale == t.iD_Locale));
    this.allProjectLocales.push(...selectedLocales);
  }

  //#endregion



  //Нужен для тестирования
  editTmx(Id: Guid): void {
    this.project.last_Activity = Date.now;

    this.projectsService.tmxFile(this.project.id, this.project);
  }
}
