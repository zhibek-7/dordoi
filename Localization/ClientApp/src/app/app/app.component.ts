import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { ProjectsService } from "../services/projects.service";
import { AuthenticationService } from "../services/authentication.service";

import { LocalizationProject } from "../models/database-entities/localizationProject.type";
import { UserService } from "src/app/services/user.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
  providers: [ProjectsService, UserService]
})
export class AppComponent implements OnInit {
  projects: LocalizationProject[];
  currentProject: LocalizationProject;
  name = "";
  projectVisibility = false;

  userAuthorized: boolean = false;
  currentUserName: string;

  constructor(
    private projectService: ProjectsService,
    private authenticationService: AuthenticationService,
    private router: Router,
    public usersService: UserService
  ) {
    //this.currentUserID = this.usersService.currentUserId;
    this.currentUserName = this.usersService.currentUserName;

    /// const count = 1;
    //if (count === 0) {
    //  router.navigate(["/dashboard"]);
    //}
  }

  ngOnInit() {
    // this.getProjects();
    console.log("app ngOnInit ---!!!!!!!!!");
    console.log("ProjectName ==" + sessionStorage.getItem("ProjectName"));
    var role = sessionStorage.getItem("userRole");

    /*
    if (
      role=="Менеджер" ||
      role=="Владелец проекта" ||
      role=="Программист"
    ) {
      this.projectVisibility = true;
    }
    */
    this.name = sessionStorage.getItem("ProjectName");
  }

  createNewProject() {
    console.log("app createNewProject ---!!!!!!!!!");
  }

  getProjects() {
    this.checkAuthorization();
    this.projectService.getProjects().subscribe(
      projects => {
        this.projects = projects;
      },
      error => console.error(error)
    );
  }

  getCurrentProject(currentProject: LocalizationProject) {
    this.currentProject = currentProject;

    //this.name_text = currentProject.name_text;
    console.log(
      "ProjectName  currentProject.name_text ==" + currentProject.name_text
    );
    this.name = this.projectService.currentProjectName;
    console.log(
      "ProjectName projectsService.currentProjectName ==" + this.name
    );

    sessionStorage.setItem("ProjectName", currentProject.name_text);
    sessionStorage.setItem("ProjectID", currentProject.id.toString());
  }

  logOut() {
    this.authenticationService.logOut();
  }

  checkAuthorization() {
    this.authenticationService.checkUserAuthorisationAsync().subscribe(
      response => {
        this.userAuthorized = response;
      },
      error => {
        console.log("Ошибка: " + error);
        alert("Необходимо авторизироваться");
        this.userAuthorized = false;
        this.router.navigate(["account"]);
      }
    );
  }
}
