import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalizationProject } from '../models/database-entities/localizationProject.type';
import { Observable, of } from 'rxjs';

@Injectable()
export class ProjectsService {

  private controllerUrl: string = "api/Project/";

    constructor(private httpClient: HttpClient) {}

    get currentProjectId(): number {
        return +sessionStorage.getItem('ProjecID');
    }

    get currentProjectName(): string {
        return +sessionStorage.getItem('ProjectName');
    }



    getProjects(): Observable<LocalizationProject[]> {
      console.log("getProject-->");
    return this.httpClient.get<LocalizationProject[]>(this.controllerUrl + "List");
    }

  getProject(id: number): Observable<LocalizationProject> {
      console.log("getProject="+id);
      console.log("getProject==="+this.controllerUrl + id);
    return this.httpClient.get<LocalizationProject>(this.controllerUrl + id);
    }

  async addProject(project: LocalizationProject) {
    console.log("addProject-->");
    console.log(project);

    console.log(this.controllerUrl + "add/{project}");
    console.log(this.controllerUrl + "AddProject");
    //return this.httpClient.get<Project>(this.controllerUrl + "add/{project}");

    let asyncResult = await this.httpClient.post<LocalizationProject>(this.controllerUrl + "AddProject", project).toPromise();
    return asyncResult;


    //return this.httpClient.get<Project>(this.controllerUrl + 1);
    }

  updateProject(Id: number) {
    console.log("updateProject-->" +Id);
    return this.httpClient.get<LocalizationProject>(this.controllerUrl + "edit/{Id}");
    }
  deleteProject(Id: number) {
    console.log("deleteProject-->" +Id);
    return this.httpClient.get<LocalizationProject>(this.controllerUrl + "delete/{Id}");

  }
}
