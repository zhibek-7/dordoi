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

  /**
   * Возвращает назначенные языки переводов на проект локализации.
   * @param projectId Идентификатор проекта локализации.
   */
  getByProjectId(projectId: Guid): Observable<LocalizationProjectsLocales[]> {
    return this.httpClient.post<LocalizationProjectsLocales[]>(
      this.controllerUrl + "listByProjectId/" + projectId,
      projectId
    );
  }

  /**
   * Переназначение языков переводов на проект локализации.
   * @param projectId Идентификатор проекта локализации.
   * @param projectLocales
   */
  editProjectLocales(projectId: Guid, projectLocales: LocalizationProjectsLocales[]): Observable<Object> {
    return this.httpClient.post(this.controllerUrl + "editProjectLocales/" + projectId, projectLocales);
  }

  /**
   * Назначение языков переводов на проект локализации.
   * @param projectLocales
   */
  createProjectLocales(projectLocales: LocalizationProjectsLocales[]): Observable<Object> {
    return this.httpClient.post(this.controllerUrl + "create", projectLocales);
  }

}
