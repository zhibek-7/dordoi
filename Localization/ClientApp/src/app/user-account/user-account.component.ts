import { Component, OnInit } from "@angular/core";
import { ProjectsService } from "../services/projects.service";
import { LocalizationProject } from "../models/database-entities/localizationProject.type";
import * as moment from "moment";
import { UserService } from "src/app/services/user.service";

moment.locale("ru");

@Component({
  selector: "app-user-account",
  templateUrl: "./user-account.component.html",
  styleUrls: ["./user-account.component.css"],
  providers: [ProjectsService, UserService]
})
export class UserAccountComponent implements OnInit {
  projectsArr: LocalizationProject[];
  columnsToDisplay = ["projectName", "changed", "settings", "projectMenu"];
  currentUserName = "";

  constructor(
    private projectsService: ProjectsService,
    private userService: UserService
  ) {}

  getPickedProject(pickedProject) {    
    sessionStorage.setItem("ProjectName", pickedProject.name_text);
    sessionStorage.setItem("ProjectID", pickedProject.id.toString());
  }

  ngOnInit() {    
    this.currentUserName = this.userService.currentUserName;

    this.projectsService.getProjects().subscribe(
      project => {
        this.projectsArr = project;
      },
      error => console.error(error)
    );    

    //TODO пока не нужно
    //this.projectsArr.forEach((element)=>{element.lastActivity = new Date();});
    // this.projectsArr = this.projectsService.getProjects() - метод возвращающий массив проетов. Отбор по юзеру не реализован
  }
}
