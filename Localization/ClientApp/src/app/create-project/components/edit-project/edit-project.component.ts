import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { ProjectsService } from "../../../services/projects.service";
import { LocalizationProject } from "../../../models/database-entities/localizationProject.type";
import { LocalizationProjectsLocales } from "src/app/models/database-entities/localizationProjectLocales.type";
import { Router } from "@angular/router";
import { Guid } from "guid-typescript";


import { Locale } from "src/app/models/database-entities/locale.type";
import { LanguageService } from "src/app/services/languages.service";
import { LocalizationProjectForSelectDTO } from "src/app/models/DTO/localizationProjectForSelectDTO.type";
import { Selectable } from "src/app/shared/models/selectable.model";
import { ProjectsLocalesService } from "src/app/services/projectsLocales.service";

@Component({
  selector: 'app-edit-project',
  templateUrl: './edit-project.component.html',
  styleUrls: ['./edit-project.component.css']
})
export class EditProjectComponent implements OnInit {

  public project: LocalizationProject;

  allLocales: Locale[] = [];
  availableLocales: Selectable<Locale>[] = [];
  availableLocalizationProjects: Selectable<LocalizationProjectForSelectDTO>[] = [];
  allProjectLocales: LocalizationProjectsLocales[] = [];

  loaded: boolean = false;
  
  form: FormGroup;
  settings_proj = new FormGroup({
    pjPublic: new FormControl(),
    pjFileTrue: new FormControl(),
    pjExportTrue: new FormControl(),
    pjSkipUntranslStrTrue: new FormControl(),
    pjNotificationTrue: new FormControl(),
    selectedLang: new FormControl()
  });
  
  //isChecked = true;

  forms: Array<any> = [
    {
      labelName: "Название проекта:",
      placeHolder: "Обычно название вашего веб-сайта или приложения",
      ivalid_feedback: "Это поле необходимо заполнить",
      id: "exampleInputName",
      describedby: "emailHelp"
    },
    {
      labelName: "Адрес проекта:",
      placeHolder: "https://localhost.com/project/<identifier>",
      ivalid_feedback: "Это поле необходимо заполнить",
      id: "exampleInputAddress",
      describedby: "addressHelp"
    }
  ];

  options: Array<any> = [
    {
      labelName: "Public project",
      id: "gridRadios1",
      small:
        "Visible to anyone. You can restrict access to languages after the project is created.",
      inputName: "gridRadios",
      value: "checked"
    },
    {
      labelName: "Private project",
      id: "gridRadios2",
      small: "Visible only to the invited project members",
      inputName: "gridRadios",
      value: "option2"
    }
  ];

  //////////////////////////////////////////////////
  constructor(
    private router: Router,
    private projectsService: ProjectsService,
    private languageService: LanguageService,
    private projectsLocalesService: ProjectsLocalesService
  ) { }
  

  ngOnInit() {
    this.loadLanguages();
    this.loadProject();
  }
  
  loadProject() {
    this.projectsService.getProject(this.projectsService.currentProjectId).subscribe(
      project => {
        this.project = project;
        this.loadProjectLocales();
      },
      error => console.error(error)
    );
  }

  async save() {

    this.projectsService.update(this.project).subscribe(
      result => {},
      error => console.error(error)
    );
    
    this.projectsLocalesService.editProjectLocales(this.project.id, this.allProjectLocales).subscribe(
      result => {},
      error => console.error(error)
    );
  }

  editTmx(Id: Guid): void {
    this.project.last_Activity = Date.now;
    
    this.projectsService.tmxFile(this.project.id, this.project);
  }

  //#region Работа с Locales

  loadLanguages() {
    this.languageService.getLanguageList().subscribe(
      locale => {
        this.allLocales = locale;
      },
      error => console.error(error)
    );
  }

  loadProjectLocales() {
    this.projectsLocalesService.getByProjectId(this.projectsService.currentProjectId).subscribe(
      projectLocales => {
        this.allProjectLocales = projectLocales;

        this.loadAvailableLanguages();
        //this.availableLocales = this.allLocales.map(
        //  local =>
        //  new Selectable<Locale>(
        //    local,
        //    projectLocales.some(selectedLocale => selectedLocale.iD_Locale == local.id)
        //  )
        //);
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
    //this.allProjectLocales = newSelection.map(t => new LocalizationProjectsLocales(this.project.id, t.id));

    let selectedLocales = newSelection.map(t => new LocalizationProjectsLocales(this.project.id, t.id));

    //Оставляет те что хранятся в БД, которые выбраны, что бы не потерять значения других полей
    this.allProjectLocales = this.allProjectLocales.filter(t => selectedLocales.some(n => n.iD_Locale == t.iD_Locale));

    //Добавляет только новые выбранные,
    selectedLocales = selectedLocales.filter(t => !this.allProjectLocales.some(n => n.iD_Locale == t.iD_Locale));
    this.allProjectLocales.push(...selectedLocales);
  }

  //#endregion
}
