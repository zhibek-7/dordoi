import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Project } from '../models/Project';
import { Observable, of } from 'rxjs';

@Injectable()
export class ProjectsService {

  private controllerUrl: string = "api/Project/";

    constructor(private httpClient: HttpClient) {}

    get currentProjectId(): number {
        return +sessionStorage.getItem('ProjecID');
    }

    getProjects(): Observable<Project[]> {
      console.log("getProject-->");
        return this.httpClient.get<Project[]>(this.controllerUrl + "List");
    }

    getProject(id: number): Observable<Project> {
      console.log("getProject="+id);
      console.log("getProject==="+this.controllerUrl + id);
        return this.httpClient.get<Project>(this.controllerUrl + id);
    }

    async addProject(project: Project) {
    console.log("addProject-->");
    console.log(project);

    console.log(this.controllerUrl + "add/{project}");
    console.log(this.controllerUrl + "AddProject");
    //return this.httpClient.get<Project>(this.controllerUrl + "add/{project}");

    let asyncResult = await this.httpClient.post<Project>(this.controllerUrl + "AddProject", project).toPromise();
    return asyncResult;


    //return this.httpClient.get<Project>(this.controllerUrl + 1);
    }

  updateProject(Id: number) {
    console.log("updateProject-->" +Id);
      return this.httpClient.get<Project>(this.controllerUrl + "edit/{Id}");
    }
  deleteProject(Id: number) {
    console.log("deleteProject-->" +Id);
     return this.httpClient.get<Project>(this.controllerUrl + "delete/{Id}");

  }
}
