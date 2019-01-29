import { LocalizationProject } from "./../../../models/database-entities/localizationProject.type";
import { Component, OnInit } from "@angular/core";
import { ProjectsService } from "../../../services/projects.service";
import { FormControl, FormGroup } from "@angular/forms";

@Component({
  selector: "app-all-settings",
  templateUrl: "./all-settings.component.html",
  styleUrls: ["./all-settings.component.css"]
})
export class AllSettingsComponent implements OnInit {
  args = "ascending";
  reverse = false;

  constructor(private projectsService: ProjectsService) {}
  currentProjectName = "";
  currentProjectId = null;
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
  selectedLang = 1;
  public project: LocalizationProject;


  pjPublic: string;
  pjFileTrue= false;
  pjSkipUntranslStrTrue= false;
  pjExportTrue= false;
  pjNotificationTrue= false;
  //public project: LocalizationProject;

  settings_proj = new FormGroup({
    pjPublic: new FormControl(),
    //pjFileTrue: new FormControl(),
    pjExportTrue: new FormControl(),
    pjSkipUntranslStrTrue: new FormControl(),
    pjNotificationTrue: new FormControl()
  });


  searchText = "";
  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};
  ngOnInit() {
    this.dropdownList = [
      { itemName: "Ido", checked: false, id: "1" },
      { itemName: "Аварский", checked: false, id: "2" },
      { itemName: "Азербайджанский", checked: false, id: "3" },
      { itemName: "Авестийский", checked: false, id: "4" },
      { itemName: "Аймара", checked: false, id: "5" },
      { itemName: "Акан", checked: false, id: "6" },
      { itemName: "Албанский", checked: false, id: "7" },
      { itemName: "Амхарский", checked: false, id: "8" },
      { itemName: "Английский", checked: false, id: "9" },
      {
        itemName: "Английский (вверх ногами)",
        checked: false,
        id: "10"
      },
      { itemName: "Английский, Аравия", checked: false, id: "11" },
      { itemName: "Ангийский, Белиз", checked: false, id: "12" },
      { itemName: "Русский", checked: false, id: "13" }
    ];

    this.currentProjectName = this.projectsService.currentProjectName;
    this.currentProjectId = this.projectsService.currentProjectId;

    console.log("ProjectName=" + sessionStorage.getItem("ProjectName"));
    console.log("ProjecID=" + sessionStorage.getItem("ProjecID"));

    this.projectsService.getProject(this.currentProjectId).subscribe(
      project => {
        this.project = project;
        this.currentProjectId = project.id;
        this.currentProjectDescription = this.project.description;
        this.currentProjecturl = this.project.url;
        this.currentProjectPublic = this.project.visibility;
        this.pjFileTrue = this.project.ableToDownload;
        this.pjSkipUntranslStrTrue = this.project.ableToLeftErrors;
        this.pjExportTrue = this.project.export_only_approved_translations;
        this.pjNotificationTrue = this.project.notifyNew;
        if (this.currentProjectPublic == true) {
          this.pjPublic == "public";
        } else {
          this.pjPublic == "nopublic";
        }


       
        console.log(this.currentProjectId);
      },
      error => console.error(error)
    );

    this.isChecked = true;
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

  addProject() {
    //let newProject: LocalizationProject = new LocalizationProject(this.settings_proj.get('pjName').value, this.settings_proj.get('pjName').value, this.settings_proj.get('pjDescription').value);// поменять на id реального пользователя, когда появится
    // this.projectsService.addProject(newProject);
  }

  editProject(Id: number): void {

    if (this.pjPublic == "public") {
      this.currentProjectPublic = true;
    } else {
      this.currentProjectPublic = false;
    }

    console.log(this.currentProjectDescription);
    this.currentProjectDescription;
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

    Id = this.currentProjectId;
    this.projectsService.updateProject(Id, project);
  }



}
