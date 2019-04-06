import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { ProjectsService } from "../services/projects.service";
import { AuthenticationService } from "../services/authentication.service";
import { Guid } from "guid-typescript";

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
  name: string = null;
  projectid: string = null;
  projectVisibility = false;

  userAuthorized: boolean = false;
  currentUserName: string;
  constructor(
    private projectService: ProjectsService,
    private authenticationService: AuthenticationService,
    private router: Router,
    public usersService: UserService
  ) {
    this.currentUserName = this.usersService.currentUserName;
  }

  ngOnInit() {
    this.getProject();
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

    this.projectService.currentProjectId = currentProject.id;
    this.projectService.currentProjectName = currentProject.name_text;
    this.getProject();
  }

  projectcleaning() {
    this.projectService.currentProjectId = Guid.createEmpty();
    this.projectService.currentProjectName = "";
  }

  getProject() {
    this.name = sessionStorage.getItem("ProjectName");
    this.projectid = sessionStorage.getItem("ProjectID");
  }

  getProjectName() {
    return sessionStorage.getItem("ProjectName");
  }

  getProjectId() {
    return sessionStorage.getItem("ProjectID");
  }

  logOut() {
    this.authenticationService.logOut();
    this.name = "";
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
