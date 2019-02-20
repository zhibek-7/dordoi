import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';

import { HttpClientModule } from '@angular/common/http';
import { LocalizationProjectsLocales } from '../models/database-entities/localizationProjectLocales.type';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ProjectsLocalesService {

  private controllerUrl: string = "api/ProjectLocale/";

  constructor(private httpClient: HttpClient) { }


  get currentProjectId(): number {
    return +sessionStorage.getItem('ProjecID');
  }

  get currentProjectName(): string {
    return sessionStorage.getItem('ProjectName');
  }


  getProjectLocales(): Observable<LocalizationProjectsLocales[]> {
    return this.httpClient.post<LocalizationProjectsLocales[]>(this.controllerUrl + 'list', null, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    });
  }
  async addProjectLocales(project: LocalizationProjectsLocales) {
    console.log("addProject-->");
    console.log(project);

    console.log(this.controllerUrl + "add/{project}");
    console.log(this.controllerUrl + "AddProject");
    //return this.httpClient.get<Project>(this.controllerUrl + "add/{project}");

    let asyncResult = await this.httpClient.post<LocalizationProjectsLocales>(this.controllerUrl + "AddProject", project, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    }).toPromise();
    return asyncResult;


    //return this.httpClient.get<Project>(this.controllerUrl + 1);
  }


  //обновление языков
  async updateProjectLocales(Id: number, projectLocale: LocalizationProjectsLocales[]) {
    console.log("updateProject-->" + Id);
    console.log(projectLocale);
   // projectLocale.id_Localization_Project = Id;
    let asyncResult = await this.httpClient.post<LocalizationProjectsLocales>(this.controllerUrl + "edit/" + Id, projectLocale, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    }).toPromise();

    return asyncResult;
  }

}
