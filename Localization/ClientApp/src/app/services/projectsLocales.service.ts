import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";

//import { HttpClientModule } from '@angular/common/http';
import { LocalizationProjectsLocales } from "../models/database-entities/localizationProjectLocales.type";
import { Observable } from "rxjs";
//import { catchError } from 'rxjs/operators';
import { Guid } from "guid-typescript";

@Injectable()
export class ProjectsLocalesService {
  private controllerUrl: string = "api/ProjectLocale/";

  constructor(private httpClient: HttpClient) { }


  getByProjectId(projectId: Guid): Observable<LocalizationProjectsLocales[]> {
    return this.httpClient.post<LocalizationProjectsLocales[]>(
      this.controllerUrl + "listByProjectId/" + projectId,
      projectId
    );
  }

  /**
   * Назначение языков переводов на проект локализации.
   * @param projectId
   * @param projectLocales
   */
  editProjectLocales(projectId: Guid, projectLocales: LocalizationProjectsLocales[]): Observable<Object> {
    return this.httpClient.post(this.controllerUrl + "editProjectLocales/" + projectId, projectLocales);
  }

  getProjectLocales(): Observable<LocalizationProjectsLocales[]> {
    return this.httpClient.post<LocalizationProjectsLocales[]>(
      this.controllerUrl + "list",
      null
    );
  }
  async addProjectLocales(project: LocalizationProjectsLocales) {
    console.log("addProject-->");
    console.log(project);

    console.log(this.controllerUrl + "add/{project}");
    console.log(this.controllerUrl + "AddProject");

    let asyncResult = await this.httpClient
      .post<LocalizationProjectsLocales>(
        this.controllerUrl + "AddProject",
        project
      )
      .toPromise();
    return asyncResult;
  }

  //обновление языков
  async updateProjectLocales(
    Id: Guid,
    projectLocale: LocalizationProjectsLocales[]
  ) {
    console.log("updateProject-->" + Id);
    console.log(projectLocale);
    // projectLocale.id_Localization_Project = Id;
    let asyncResult = await this.httpClient
      .post<LocalizationProjectsLocales>(
        this.controllerUrl + "edit/" + Id,
        projectLocale
      )
      .toPromise();

    return asyncResult;
  }
}
