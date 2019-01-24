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
  public project: LocalizationProject;

  settings_proj = new FormGroup({
    pjPublic: new FormControl(),
    pjFileTrue: new FormControl(),
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
      { itemName: "Ido", checked: false, id: "gridIdo" },
      { itemName: "Аварский", checked: false, id: "gridAvarski" },
      { itemName: "Азербайджанский", checked: false, id: "gridAzerbaidjan" },
      { itemName: "Авестийский", checked: false, id: "gridAvestiski" },
      { itemName: "Аймара", checked: false, id: "gridAimara" },
      { itemName: "Акан", checked: false, id: "gridAkan" },
      { itemName: "Албанский", checked: false, id: "gridAlbanski" },
      { itemName: "Амхарский", checked: false, id: "gridAmharskii" },
      { itemName: "Английский", checked: false, id: "gridEnglisch" },
      {
        itemName: "Английский (вверх ногами)",
        checked: false,
        id: "gridEnglVverhNogami"
      },
      { itemName: "Английский, Аравия", checked: false, id: "gridEnglAraviya" },
      { itemName: "Ангийский, Белиз", checked: false, id: "gridEnglBeliz" },
      { itemName: "Русский", checked: false, id: "gridRussia" }
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
    let newProject: LocalizationProject = new LocalizationProject(
      this.currentProjectId,
      this.settings_proj.get("pjName").value,
      this.settings_proj.get("pjDescription").value,
      // this.settings_proj.get('pjDescription').value,
      this.settings_proj.get("pjPublic").value, //visibility
      //  this.settings_proj.get('pjDescription').value,//date dateOfCreation
      // this.settings_proj.get('pjDescription').value,//date lastActivity
      this.settings_proj.get("pjFileTrue").value, //ableToDownload
      this.settings_proj.get("pjSkipUntranslStrTrue").value, //ableToLeftErrors
      this.settings_proj.get("pjExportTrue").value,
      this.settings_proj.get("pjPublic").value,
      this.settings_proj.get("pjExportTrue").value,
      this.settings_proj.get("pjExportTrue").value,
      this.settings_proj.get("pjExportTrue").value,
      this.settings_proj.get("pjExportTrue").value,
      this.settings_proj.get("pjExportTrue").value
    ); // поменять на id реального пользователя, когда появится
    Id = this.currentProjectId;
    // this.projectsService.updateProject(Id, newProject);
  }

  /*
visibility: boolean,
    dateOfCreation: Date,
    lastActivity: Date,
    ableToDownload: boolean,
    ableToLeftErrors: boolean,
    defaultString: boolean,
    notifyNew: boolean,
    notifyFinish: boolean,
    notifyConfirm: boolean,
    notifynewcomment: boolean,
    export_only_approved_translations: boolean,
    original_if_string_is_not_translated: boolean
*/
}
