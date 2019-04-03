import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { LocalizationProject } from "../models/database-entities/localizationProject.type";
import { Observable } from "rxjs";
import { LocalizationProjectForSelectDTO } from "../models/DTO/localizationProjectForSelectDTO.type";
import { LocalizationProjectsLocales } from "../models/database-entities/localizationProjectLocales.type";
import { Locale } from "moment";
import { Guid } from "guid-typescript";
@Injectable()
export class ProjectsService {
  private controllerUrl: string = "api/Project/";

  constructor(private httpClient: HttpClient) {}

  get currentProjectId(): Guid {
    let id: string = sessionStorage.getItem("ProjectID");

    if (id == null || id == "") {
      return Guid.createEmpty();
    } else {
      return Guid.parse(sessionStorage.getItem("ProjectID"));
    }
  }
  set currentProjectId(id: Guid) {
    sessionStorage.setItem("ProjectID", id.toString());
  }

  get currentProjectName(): string {
    return sessionStorage.getItem("ProjectName");
  }
  set currentProjectName(name: string) {
    sessionStorage.setItem("ProjectName", name);
  }

  getProjects(): Observable<LocalizationProject[]> {
    console.log("getProject-->");
    return this.httpClient.post<LocalizationProject[]>(
      this.controllerUrl + "List",
      null
    );
  }

  getProject(id: Guid): Observable<LocalizationProject> {
    console.log("service projects = getProject=" + id);
    console.log("service projects = getProject===" + this.controllerUrl + id);
    return this.httpClient.post<LocalizationProject>(
      this.controllerUrl + id,
      id
    );
  }

  /**
   * Возвращает проект локализации с подробной иформацией из связанных данных.
   * @param id Идентификатор проекта локализации.
   */
  getProjectWithDetails(id: Guid): Observable<LocalizationProject> {
    return this.httpClient.post<LocalizationProject>(
      this.controllerUrl + "details" + "/" + id,
      id
    );
  }

  /** Возвращает список проектов локализации, назначенных на пользователя, содержащий только ID, Name */
  getLocalizationProjectForSelectDTOByUser(): Observable<LocalizationProjectForSelectDTO[]> {
    return this.httpClient.post<LocalizationProjectForSelectDTO[]>(
      this.controllerUrl + "forSelectByUser",
      null,
      {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      }
    );
  }

  create(project: LocalizationProject): Observable<Guid> {
    return this.httpClient.post<Guid>(this.controllerUrl + "create", project);
  }

  update(project: LocalizationProject): Observable<Object> {
    return this.httpClient.post(this.controllerUrl + "update", project);
  }

  delete(projectId: Guid): Observable<Object> {
    return this.httpClient.delete(this.controllerUrl + "delete/" + projectId);
  }

  /**
   xml tmx file
   * @param Id
   * @param project
   */
  private controllerUrlTmx: string = "api/ReadWriteFile/";
  async tmxFile(Id: Guid, project: LocalizationProject) {
    console.log("updateProject-->" + Id);
    console.log(project);
    project.id = Id;
    let asyncResult = await this.httpClient
      .post<LocalizationProject>(this.controllerUrlTmx + "createTmx", project)
      .toPromise();
    return asyncResult;
  }
  
}
