import { LocalizationProject } from "./../../../models/database-entities/localizationProject.type";
import { Component, OnInit } from "@angular/core";
import { ProjectsService } from "../../../services/projects.service";
//import { ProjectsLocalesService } from "../../../services/projectsLocales.service";
import { FormControl, FormGroup } from "@angular/forms";
import { LocalizationProjectsLocales } from "src/app/models/database-entities/localizationProjectLocales.type";
import { Guid } from 'guid-typescript';
import { Router } from "@angular/router";

@Component({
  selector: "app-all-settings",
  templateUrl: "./all-settings.component.html",
  styleUrls: ["./all-settings.component.css"]
})
export class AllSettingsComponent implements OnInit {
  args = "ascending";
  reverse = false;

  constructor(
    private router: Router,
    private projectsService: ProjectsService
  ) {}
  currentProjectName = "";
  currentProjectId = null;
  currentProjectDescription = "";
  currentProjectPublic = true;
  currentProjectFileTrue = false;
  currentSkipUntranslStrTrue = false;
  currentProjectExportTrue = false;
  currentProjectNotificationTrue = false;
  currentProjectdateOfCreation = Date.now();
  currentProjectID_SourceLocale = null;
  currentProjectlastActivity = Date.now();
  currentProjectlogo = "";
  currentProjectid = null;
  currentProjecturl = "";
  isChecked = true;

  public project: LocalizationProject;

  // this.searchText = this.projectsLcalesService.currentProjectName;
  selectedL: boolean;
  pjPublic: string;
  pjFileTrue = false;
  pjSkipUntranslStrTrue = true;
  pjExportTrue = false;
  pjNotificationTrue = false;
  pjAllLeft = false;
  selectedLang: Guid;
  //public project: LocalizationProject;

  settings_proj = new FormGroup({
    pjPublic: new FormControl(),
    //pjFileTrue: new FormControl(),
    pjExportTrue: new FormControl(),
    pjSkipUntranslStrTrue: new FormControl(),
    pjNotificationTrue: new FormControl()
    // selectedLang: new FormControl()
  });

  searchText = "";
  dropdownList = [];
  allProjLocales: LocalizationProjectsLocales[];
  allLocale = [];

  selectedItems = [];
  dropdownSettings = {};
  ngOnInit() {
    //вывод всех языков проекта

    this.selectedL = false;
    this.currentProjectName = this.projectsService.currentProjectName;
    this.currentProjectId = this.projectsService.currentProjectId;

    let allLangsPr = [];
    let selectedLangs = [];

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

        this.projectsService.getProjectLocales(this.currentProjectId).subscribe(
          projectsL => {
            this.allProjLocales = projectsL;

            this.dropdownList.forEach(lang => {
              const index = projectsL.findIndex(
                list => list["iD_Locale"] == lang.id
              );

              if (index != -1) {
                selectedLangs.push({
                  itemName: lang.itemName,
                  selected: true,
                  id: lang.id
                });

                lang.checked = true;
                this.selectedL = true;
              } else {
                lang.checked = false;
                this.selectedL = false;
              }
            });
          },
          error => console.error(error)
        );
      },
      error => console.error(error)
    );

    //console.log("ProjectName=" + sessionStorage.getItem("ProjectName"));
    //console.log("ProjectID=" + sessionStorage.getItem("ProjectID"));

    this.projectsService.getProject(this.currentProjectId).subscribe(
      project => {
        this.project = project;
        this.project.able_To_Left_Errors = true;
        this.currentProjectId = project.id;
        this.currentProjectDescription = this.project.description;
        this.currentProjecturl = this.project.url;
        this.currentProjectPublic = this.project.visibility;
        this.pjFileTrue = this.project.able_To_Download;
        this.pjSkipUntranslStrTrue = this.project.able_To_Left_Errors;
        this.pjExportTrue = this.project.export_only_approved_translations;
        this.pjNotificationTrue = this.project.notify_New;
        this.selectedLang = project["iD_Source_Locale"];
        this.selectedItems = selectedLangs;

        if (this.currentProjectPublic == true) {
          this.pjPublic = "public";
        } else {
          this.pjPublic = "nopublic";
        }

        this.selectedL = false;
        this.dropdownList = this.dropdownList;

        console.log(this.selectedItems);
      },
      error => console.error(error)
    );

    this.isChecked = true;
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

  idPrLocale: Guid;
  idLocale: Guid;
  editProject(Id: Guid): void {
    if (this.pjPublic == "public") {
      this.currentProjectPublic = true;
    } else {
      this.currentProjectPublic = false;
    }
    console.log(this.currentProjectDescription);

    //удаляет добавленные языки в один массив
    let DeleteprojectLocales: LocalizationProjectsLocales[] = [];
    this.allProjLocales.forEach(lang => {
      //const index = this.selectedItems.findIndex(
      //  list => list.id == lang["iD_Locale"]
      //);
      //if (index!=-1) {
      DeleteprojectLocales.push({
        iD_Localization_Project: this.currentProjectId,
        iD_Locale: lang["iD_Locale"],
        percent_Of_Translation: 0,
        percent_Of_Confirmed: 0
      });
      //}
    });

    //передает массив языков
    this.projectsService
      .deleteProjectLocales(DeleteprojectLocales)
      .subscribe(pr => {
        //собирает добавленные языки в один массив
        let projectLocales: LocalizationProjectsLocales[] = [];
        this.selectedItems.forEach(lang => {
          //const index = this.allProjLocales.findIndex(
          //  list => list["iD_Locale"] == lang.id
          //);

          //if (index == -1) {
          projectLocales.push({
            iD_Localization_Project: this.currentProjectId,
            iD_Locale: lang.id,
            percent_Of_Translation: 0,
            percent_Of_Confirmed: 0
          });
          //} else {

          //}
        });
        //передает массив языков
        this.projectsService.addProjectLocales(projectLocales);
      });

    let project: LocalizationProject = new LocalizationProject(
      this.currentProjectId, //id
      this.currentProjectName, //name
      this.currentProjectDescription, //description
      this.currentProjecturl, //url
      this.currentProjectPublic, //visibility
      Date.now, //date dateOfCreation
      // this.settings_proj.get('pjDescription').value,//date lastActivity
      this.selectedLang, //id_SourceLocale
      this.pjFileTrue, //ableToDownload
      this.pjSkipUntranslStrTrue, //ableToLeftErrors

      this.pjNotificationTrue, //notifyNew

      this.pjAllLeft, //notifyFinish
      this.pjAllLeft, //notifyConfirm
      this.pjAllLeft, //notifynewcomment
      this.pjExportTrue, //export_only_approved_translations
      this.pjAllLeft //original_if_string_is_not_translated
, false
      //
    ); // поменять на id реального пользователя, когда появится

    Id = this.currentProjectId;
    this.projectsService.updateProject(Id, project);

    this.router.navigate(["/Projects/" + Id]);
  }

  editTmx(Id: Guid): void {
    let project: LocalizationProject = new LocalizationProject(
      this.currentProjectId, //id
      this.currentProjectName, //name
      this.currentProjectDescription, //description
      this.currentProjecturl, //url
      this.currentProjectPublic, //visibility
      Date.now, //date dateOfCreation
      // this.settings_proj.get('pjDescription').value,//date lastActivity
      this.selectedLang, //id_SourceLocale
      this.pjFileTrue, //ableToDownload
      this.pjSkipUntranslStrTrue, //ableToLeftErrors
      this.pjNotificationTrue, //notifyNew
      this.pjAllLeft, //notifyFinish
      this.pjAllLeft, //notifyConfirm
      this.pjAllLeft, //notifynewcomment
      this.pjExportTrue, //export_only_approved_translations
      this.pjAllLeft //original_if_string_is_not_translated
      , false
    ); // поменять на id реального пользователя, когда появится
    Id = this.currentProjectId;
    this.projectsService.tmxFile(Id, project);
  }
}
