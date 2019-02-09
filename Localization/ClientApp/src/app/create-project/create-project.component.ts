import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ProjectsService } from '../services/projects.service';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';
import { LocalizationProject } from '../models/database-entities/localizationProject.type';

import { LocalizationProjectsLocales } from "src/app/models/database-entities/localizationProjectLocales.type";
import { Locale } from "src/app/models/database-entities/locale.type";

import { forEach } from "@angular/router/src/utils/collection";
import { promise } from "protractor";
import { Promise } from "q";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css'],
})
export class CreateProjectComponent implements OnInit {
  args = 'ascending';
  reverse = false;
  searchText = '';
  form: FormGroup;
  title = 'Создание проекта Crowdin';

  forms: Array<any> = [
    { labelName: 'Название проекта:', placeHolder: 'Обычно название вашего веб-сайта или приложения', ivalid_feedback: 'Это поле необходимо заполнить', id: 'exampleInputName', describedby: 'emailHelp' },
    { labelName: 'Адрес проекта:', placeHolder: 'https://crowdin.com/project/<identifier>', ivalid_feedback: 'Это поле необходимо заполнить', id: 'exampleInputAddress', describedby: 'addressHelp' },
  ];

options:Array<any>=[
    {labelName:'Public project',id:'gridRadios1', small:'Visible to anyone. You can restrict access to languages after the project is created.',inputName:'gridRadios', value:'checked'},
    {labelName:'Private project',id:'gridRadios2', small:'Visible only to the invited project members',inputName:'gridRadios', value:'option2'},];


//////////////////////////////////////////////////
  constructor(private projectsService: ProjectsService) { }
  currentProjectName = "";
  currentProjectId = Math.floor(Math.random() * 10000) + 1;;
  currentProjectDescription = "";
  currentProjectPublic = true;
  currentProjectFileTrue = false;
  currentSkipUntranslStrTrue = false;
  currentProjectExportTrue = false;
  currentProjectNotificationTrue = false;
  currentProjectdateOfCreation = Date.now();
  currentProjectID_SourceLocale = 1;
  currentProjectlastActivity = Date.now();
  currentProjectlogo = "";
  currentProjectid = 1;
  currentProjecturl = "";
  isChecked = true;

  public project: LocalizationProject;

  // this.searchText = this.projectsLcalesService.currentProjectName;
  selectedL: boolean;
  pjPublic: string;
  pjFileTrue = false;
  pjSkipUntranslStrTrue = false;
  pjExportTrue = false;
  pjNotificationTrue = false;
  selectedLang: number;
  //public project: LocalizationProject;

  settings_proj = new FormGroup({
    pjPublic: new FormControl(),
    //pjFileTrue: new FormControl(),
    pjExportTrue: new FormControl(),
    pjSkipUntranslStrTrue: new FormControl(),
    pjNotificationTrue: new FormControl(),
    selectedLang: new FormControl()
  });
  
  dropdownList = [];
  allProjLocales: LocalizationProjectsLocales[];
  allLocale = [];

  selectedItems = [];
  dropdownSettings = {};
  ngOnInit() {
   
    let allLangsPr = [];
    let allLocales = this.projectsService.getLocales().subscribe(projects => {
      this.allLocale = projects;
      this.allLocale.forEach((lang) => {
        allLangsPr.push({

          itemName: lang["name"],
          checked: false,
          id: lang["id"]

        });
      });

      this.dropdownList = allLangsPr;
    },
      error => console.error(error));

  }

  AddSelected(event, lang) {
    var target = event.target || event.srcElement || event.currentTarget;
    var idAttr = target.attributes.id;
    var itemNameAttr = target.attributes.name;
    var selectedAttr = target.attributes.checked;
    var value = idAttr.nodeValue;
    lang.checked = !lang.checked;
    if (lang.checked == true) {
      this.selectedItems.push({
        itemName: itemNameAttr.nodeValue,
        selected: lang.checked,
        id: idAttr.nodeValue
      });
    } else {
      const index = this.selectedItems.findIndex(list => list.id == lang.id); //Find the index of stored id
      this.selectedItems.splice(index, 1);
    }
  }

  AllSelected() {
    this.dropdownList.forEach(element => {
      element.checked = !element.checked;
      if (element.checked == true) {
        this.selectedItems.push({
          itemName: element.itemName,
          selected: element.checked,
          id: element.id
        });
      } else {
        const index = this.selectedItems.findIndex(
          list => list.id == element.id
        ); //Find the index of stored id
        this.selectedItems.splice(index, 1);
      }
    });
  }

  FiterByName() {
    this.dropdownList.forEach(element => {
      element.checked = !element.checked;
      if (element.checked == true) {
        this.selectedItems.push({
          itemName: element.itemName,
          selected: element.checked,
          id: element.id
        });
      } else {
        const index = this.selectedItems.findIndex(
          list => list.id == element.id
        ); //Find the index of stored id
        this.selectedItems.splice(index, 1);
      }
    });
  }


  idPrLocale: number;
  idLocale: number;
 
     //TODO вернуть как было.
    //this.projectService.addProject(newProject)subscribe(response => console.log(response));




  addProject(): void {
    let project: LocalizationProject = new LocalizationProject(
      this.currentProjectId,
      this.currentProjectName,
      this.currentProjectDescription,
      this.currentProjecturl,
      this.currentProjectPublic, //visibility
      Date.now,//date dateOfCreation
      // this.settings_proj.get('pjDescription').value,//date lastActivity
      this.selectedLang,
      this.pjFileTrue, //ableToDownload
      this.pjSkipUntranslStrTrue, //ableToLeftErrors
      //this.pjExportTrue,
      this.pjNotificationTrue,

      this.pjFileTrue, //ableToDownload
      this.pjSkipUntranslStrTrue, //ableToLeftErrors
      this.pjExportTrue,
      this.pjNotificationTrue,
      this.pjNotificationTrue
    ); // поменять на id реального пользователя, когда появится

    this.projectsService.addProject(project);
    //собирает добавленные языки в один массив

    let projectLocales: LocalizationProjectsLocales[] = [];
    this.selectedItems.forEach((lang) => {

      
        projectLocales.push({
          id_LocalizationProject: this.currentProjectId,
          id_Locale: lang.id,
          percentOfTranslation: 0,
          PercentOfConfirmed: 0
        });
    


    })

    //передает массив языков
    this.projectsService.addProjectLocales(projectLocales);
  
  }


  
}
