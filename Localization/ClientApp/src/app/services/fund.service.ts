import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { LocalizationProject } from "../models/database-entities/localizationProject.type";
import { Observable } from "rxjs";
import { LocalizationProjectForSelectDTO } from "../models/DTO/localizationProjectForSelectDTO.type";
import { fund } from "../models/database-entities/fund.type";
import { Guid } from "guid-typescript";
@Injectable()
export class FundService {
  private controllerUrl: string = "api/Fund/";

  constructor(private httpClient: HttpClient) { }

  get currentFundId(): Guid {
    let id: string = sessionStorage.getItem("id");

    if (id == null || id == "") {
      return Guid.createEmpty();
    } else {
      return Guid.parse(sessionStorage.getItem("id"));
    }
  }
  set currentFundId(id: Guid) {
    sessionStorage.setItem("FundID", id.toString());
  }

  get currentFundName(): string {
    return sessionStorage.getItem("Fund_text");
  }
  set currentFundName(name: string) {
    sessionStorage.setItem("Fund_text", name);
  }

  get currentFundDescription(): string {
    return sessionStorage.getItem("Fund_description");
  }
  set currentFundDescription(description: string) {
    sessionStorage.setItem("Fund_description", description);
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






  create(fund: fund): Observable<Guid> {
    return this.httpClient.post<Guid>(this.controllerUrl + "Create", fund);
  }







  update(fund: fund): Observable<Object> {
    return this.httpClient.post(this.controllerUrl + "update", fund);
  } 

  delete(fundId: Guid): Observable<Object> {
    return this.httpClient.delete(this.controllerUrl + "delete/" + fundId);
  }

  ///**
  // xml tmx file
  // * @param Id
  // * @param fund
  // */
  //private controllerUrlTmx: string = "api/ReadWriteFile/";
  //async tmxFile(Id: Guid, fund: fund) {
  //  console.log("updateProject-->" + Id);
  //  console.log(fund);
  //  fund.id = Id;
  //  let asyncResult = await this.httpClient
  //    .post<LocalizationProject>(this.controllerUrlTmx + "createTmx", fund)
  //    .toPromise();
  //  return asyncResult;
  //}

}
