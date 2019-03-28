import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { ProjectsService } from "../../../services/projects.service";
import { LocalizationProject } from "../../../models/database-entities/localizationProject.type";
import { LocalizationProjectsLocales } from "src/app/models/database-entities/localizationProjectLocales.type";
import { Router } from "@angular/router";
import { Guid } from "guid-typescript";

@Component({
  selector: 'app-edit-project',
  templateUrl: './edit-project.component.html',
  styleUrls: ['./edit-project.component.css']
})
export class EditProjectComponent implements OnInit {
  args = "ascending";
  reverse = false;
  searchText = "";
  form: FormGroup;
  //title = "Создание проекта";

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
    private projectsService: ProjectsService
  ) { }
  isChecked = true;

  public project: LocalizationProject;
  
  selectedL: boolean;

  settings_proj = new FormGroup({
    pjPublic: new FormControl(),
    pjFileTrue: new FormControl(),
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
    this.projectsService.getProject(this.projectsService.currentProjectId).subscribe(
      project => {
        this.project = project;
        console.log("this.project: ", this.project);
      },
      error => console.error(error)
    );

    let allLangsPr = [];
    this.projectsService.getLocales().subscribe(
      projects => {
        this.allLocale = projects;
        this.allLocale.forEach(lang => {
          allLangsPr.push({
            itemName: lang["name_text"],
            checked: false,
            id: lang["id"]
          });
        });

        this.dropdownList = allLangsPr;
      },
      error => console.error(error)
    );
  }

  AddSelected(event, lang) {
    var target = event.target || event.srcElement || event.currentTarget;
    var idAttr = target.attributes.id;
    var itemNameAttr = target.attributes.name;

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


  save(): void {
    this.project.date_Of_Creation = Date.now;

  }

  editTmx(Id: Guid): void {
    this.project.date_Of_Creation = Date.now;
    
    this.projectsService.tmxFile(this.project.id, this.project);
  }
}
