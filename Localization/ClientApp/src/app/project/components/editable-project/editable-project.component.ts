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
   //this.allProjectLocales = [];

    this.loadLanguages();
  }

  initForm() {
    this.form = new FormGroup({
      nameFormControl: new FormControl(this.project.name_text, Validators.required),
      descriptionFormControl: new FormControl(this.project.description),
   });
  }
  
  async submit() {
    this.getProject();
    this.editedSubmitted.emit([this.project, this.allProjectLocales]);
  }

  getProject() {
    this.project.name_text = this.form.controls.nameFormControl.value;
    this.project.description = this.form.controls.descriptionFormControl.value;
    
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
    if (this.project.id) {
      this.projectsLocalesService.getByProjectId(this.project.id).subscribe(
        projectLocales => {
          this.allProjectLocales = projectLocales;
          this.loadAvailableLanguages();
        },
        error => console.error(error)
      );
    } else {
      this.loadAvailableLanguages();
    }
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




  //#endregion



  //Нужен для тестирования
  editTmx(Id: Guid): void {
    this.project.last_Activity = Date.now;

    this.projectsService.tmxFile(this.project.id, this.project);
  }
}
