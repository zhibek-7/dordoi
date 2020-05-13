import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { LocalizationProject } from "../models/database-entities/localizationProject.type";
import { Observable } from "rxjs";
import { LocalizationProjectForSelectDTO } from "../models/DTO/localizationProjectForSelectDTO.type";
import { fund } from "../models/database-entities/fund.type";
import { Guid } from "guid-typescript";
import { individual } from "../models/database-entities/individual.type";
@Injectable()
export class IndividualService {
  formData: individual;
  private controllerUrl: string = "api/Individual/";

  constructor(private httpClient: HttpClient) { }

  get currentIndivId(): Guid {
    let id: string = sessionStorage.getItem("IndivID");

    if (id == null || id == "") {
      return Guid.createEmpty();
    } else {
      return Guid.parse(sessionStorage.getItem("IndivID"));
    }
  }
  set currentIndivId(id: Guid) {
    sessionStorage.setItem("IndivID", id.toString());
  }

  get currentIndivName_first(): string {
    return sessionStorage.getItem("name_text_first");
  }
  set currentIndivName_first(name: string) {
    sessionStorage.setItem("name_text_first", name);
  }

  get currentIndivDescription_first(): string {
    return sessionStorage.getItem("Indiv_description_first");
  }
  set currentIndivDescription_first(description: string) {
    sessionStorage.setItem("Indiv_description_first", description);
  }




  get currentIndivName_second(): string {
    return sessionStorage.getItem("Indiv_text_second");
  }
  set currentIndivName_second(name: string) {
    sessionStorage.setItem("Indiv_text_second", name);
  }

  get currentIndivDescription_second(): string {
    return sessionStorage.getItem("Indiv_description_second");
  }
  set currentIndivDescription_second(description: string) {
    sessionStorage.setItem("Indiv_description_second", description);
  }





  get currentIndivName_third(): string {
    return sessionStorage.getItem("Indiv_text_third");
  }
  set currentIndivName_third(name: string) {
    sessionStorage.setItem("Indiv_text_third", name);
  }

  get currentIndivDescription_third(): string {
    return sessionStorage.getItem("Indiv_description_third");
  }
  set currentIndivDescription_third(description: string) {
    sessionStorage.setItem("Indiv_description_third", description);
  }




  get currentIndivName_fourth(): string {
    return sessionStorage.getItem("Indiv_text_fourth");
  }
  set currentIndivName_fourth(name: string) {
    sessionStorage.setItem("Indiv_text_fourth", name);
  }

  get currentIndivDescription_fourth(): string {
    return sessionStorage.getItem("Indiv_description_fourth");
  }
  set currentIndivDescription_fourth(description: string) {
    sessionStorage.setItem("Indiv_description_fourth", description);
  }









  get currentIndivName_fifth(): string {
    return sessionStorage.getItem("Indiv_text_fifth");
  }
  set currentIndivName_fifth(name: string) {
    sessionStorage.setItem("Indiv_text_fifth", name);
  }

  get currentIndivDescription_fifth(): string {
    return sessionStorage.getItem("Indiv_description_fifth");
  }
  set currentIndivDescription_fifth(description: string) {
    sessionStorage.setItem("Indiv_description_fifth", description);
  }








  getIndivids(): Observable<individual[]> {
    console.log("getFund-->");
    return this.httpClient.post<individual[]>(
      this.controllerUrl + "List",
      null
    );
  }

  getIndivid(id: Guid): Observable<individual> {
    console.log("service individs = getIndivid=" + id);
    console.log("service individs = getIndivid===" + this.controllerUrl + id);
    return this.httpClient.post<individual>(
      this.controllerUrl + id,
      id
    );
  }

  /**
   * Возвращает проект локализации с подробной иформацией из связанных данных.
   * @param id Идентификатор проекта локализации.
   */
  getIndividWithDetails(id: Guid): Observable<individual> {
    return this.httpClient.post<individual>(
      this.controllerUrl + "details" + "/" + id,
      id
    );
  }

  /** Возвращает список проектов локализации, назначенных на пользователя, содержащий только ID, Name */
  getFundForSelectDTOByUser(): Observable<LocalizationProjectForSelectDTO[]> {
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


  create(individ: individual): Observable<Guid> {
    return this.httpClient.post<Guid>(this.controllerUrl + "Create", individual);
  }

  update(individ: individual): Observable<Object> {
    return this.httpClient.post(this.controllerUrl + "update", individual);
  } 

  delete(individId: Guid): Observable<Object> {
    return this.httpClient.delete(this.controllerUrl + "delete/" + individId);
  }


}
