import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { LocalizationProject } from "../models/database-entities/localizationProject.type";
import { Observable } from "rxjs";
import { LocalizationProjectForSelectDTO } from "../models/DTO/localizationProjectForSelectDTO.type";
import { LocalizationProjectsLocales } from "../models/database-entities/localizationProjectLocales.type";
import { Locale } from "moment";

@Injectable()
export class ProjectsService {
  private controllerUrl: string = "api/Project/";

  constructor(private httpClient: HttpClient) {}

  get currentProjectId(): number {
    return +sessionStorage.getItem("ProjectID");
  }

  get currentProjectName(): string {
    return sessionStorage.getItem("ProjectName");
  }

  getProjects(): Observable<LocalizationProject[]> {
    console.log("getProject-->");
    return this.httpClient.post<LocalizationProject[]>(
      this.controllerUrl + "List",
      null
    );
  }

  getProject(id: number): Observable<LocalizationProject> {
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
  getProjectWithDetails(id: number): Observable<LocalizationProject> {
    return this.httpClient.post<LocalizationProject>(
      this.controllerUrl + "details",
      id
    );
  }

  /**
   * /добавление проекта
   * @param project
   */
  newProject(project: LocalizationProject): Observable<LocalizationProject> {
    return this.httpClient.post<LocalizationProject>(
      this.controllerUrl + "newProject",
      project
    );
  }

  /**
   * /добавление проекта
   * @param project
   */
  async addProject(project: LocalizationProject) {
    console.log("addProject-->");
    console.log(project);

    console.log(this.controllerUrl + "add/{project}");
    console.log(this.controllerUrl + "AddProject");

    let asyncResult = await this.httpClient
      .post<LocalizationProject>(this.controllerUrl + "AddProject", project)
      .toPromise();
    return asyncResult;
  }
  async updateProject(Id: number, project: LocalizationProject) {
    console.log("updateProject-->" + Id);
    console.log(project);
    project.id = Id;
    let asyncResult = await this.httpClient
      .post<LocalizationProject>(this.controllerUrl + "edit/" + Id, project)
      .toPromise();

    return asyncResult;
  }

  //удаление проекта
  async deleteProject(Id: number) {
    console.log("updateProject-->" + Id);
    let asyncResult = await this.httpClient
      .post<LocalizationProject>(this.controllerUrl + "delete/" + Id, Id)
      .toPromise();
    return asyncResult;
  }

  //удаление языков
  deleteProjectLocalesById(
    Id: number
  ): Observable<LocalizationProjectsLocales> {
    console.log("updateProject-->" + Id);
    let asyncResult = this.httpClient.post<LocalizationProjectsLocales>(
      this.controllerUrl + "deleteLocales/" + Id,
      Id
    );
    return asyncResult;
  }

  /** Возвращает список проектов локализации, назначенных на пользователя, содержащий только ID, Name */
  getLocalizationProjectForSelectDTOByUser(): Observable<
    LocalizationProjectForSelectDTO[]
  > {
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

  async addProjectLocales(projectLocales: LocalizationProjectsLocales[]) {
    console.log("addProject-->");
    console.log(projectLocales);

    console.log(this.controllerUrl + "add/{project}");
    console.log(this.controllerUrl + "AddProject");

    let asyncResult = await this.httpClient
      .post<LocalizationProjectsLocales[]>(
        this.controllerUrl + "AddProjectLocale",
        projectLocales
      )
      .toPromise();
    return asyncResult;
  }

  //обновление языков
  async updateProjectLocales(
    Id: number,
    projectLocale: LocalizationProjectsLocales[]
  ) {
    console.log("updateProject-->" + Id);
    console.log(projectLocale);
    // projectLocale.id_Localization_Project = Id;
    let asyncResult = await this.httpClient
      .post<LocalizationProjectsLocales>(
        this.controllerUrl + "editProjectLocale/" + Id,
        projectLocale
      )
      .toPromise();

    return asyncResult;
  }

  /**
     newProject(project: LocalizationProject): Observable<LocalizationProject> {
    return this.httpClient.post<LocalizationProject>(this.controllerUrl + "newProject", project);
  }

   * @param projectLocales
   */

  //удаление языков
  deleteProjectLocales(
    projectLocales: LocalizationProjectsLocales[]
  ): Observable<LocalizationProjectsLocales[]> {
    console.log("addProject-->");
    console.log(projectLocales);
    return this.httpClient.post<LocalizationProjectsLocales[]>(
      this.controllerUrl + "deleteProjectLocale",
      projectLocales
    );
  }

  //все языки 1 проекта
  getProjectLocales(Id: number): Observable<LocalizationProjectsLocales[]> {
    return this.httpClient.post<LocalizationProjectsLocales[]>(
      this.controllerUrl + "ListProjectLocales/" + Id,
      Id
    ); //.pipe(map(listOfPojLocales => listOfPojLocales.map(singleProjLoc => new LocalizationProjectsLocales(singleProjLoc.id_LocalizationProject, singleProjLoc.id_Locale, singleProjLoc.percentOfTranslation, singleProjLoc.PercentOfConfirmed))));;
  }

  getLocales(): Observable<Locale[]> {
    return this.httpClient.post<Locale[]>(
      this.controllerUrl + "ListLocales",
      null
    );
  }

  /**
   xml tmx file
   * @param Id
   * @param project
   */
  private controllerUrlTmx: string = "api/ReadWriteFile/";
  async tmxFile(Id: number, project: LocalizationProject) {
    console.log("updateProject-->" + Id);
    console.log(project);
    project.id = Id;
    let asyncResult = await this.httpClient
      .post<LocalizationProject>(this.controllerUrlTmx + "createTmx", project)
      .toPromise();
    return asyncResult;
  }
}
