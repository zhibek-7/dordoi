import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Project } from '../models/Project';
import { Observable, of } from 'rxjs';

@Injectable()
export class ProjectsService {

  private controllerUrl: string = "api/Project/";

    private currnetProjectId = undefined;

    constructor(private httpClient: HttpClient) {}

    getCurrentProjectId(){
      // когда появится несколько проектов для тестирования, 
      //нужно будет присваивать явное значение текущего проекта(пока все только в одном проекте)
      return this.currnetProjectId = 0; 
    }

    getProjects(): Observable<Project[]> {
        return this.httpClient.get<Project[]>(this.controllerUrl + "List");
    }

    getProject(id: number): Observable<Project> {
        return this.httpClient.get<Project>(this.controllerUrl + id);
    }

   addProject(project: Project) {
    return this.httpClient.get<Project>(this.controllerUrl + "add/{project}");
     
    }

  updateProject(Id: number) {
      return this.httpClient.get<Project>(this.controllerUrl + "edit/{Id}");
    }
  deleteProject(Id: number) {
     return this.httpClient.get<Project>(this.controllerUrl + "delete/{Id}");
 
  }
}
