import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { LocalizationProject } from "../models/database-entities/localizationProject.type";
import { Observable } from "rxjs";
import { LocalizationProjectForSelectDTO } from "../models/DTO/localizationProjectForSelectDTO.type";
import { fund } from "../models/database-entities/fund.type";
import { Guid } from "guid-typescript";
@Injectable()
export class FundService {
  formData: fund;
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



  getFunds(): Observable<fund[]> {
    console.log("getFund-->");
    return this.httpClient.post<fund[]>(
      this.controllerUrl + "List",
      null
    );
  }

  getFund(id: Guid): Observable<fund> {
    console.log("service funds = getFund=" + id);
    console.log("service funds = getFund===" + this.controllerUrl + id);
    return this.httpClient.post<fund>(
      this.controllerUrl + id,
      id
    );
  }

  /**
   * Возвращает проект локализации с подробной иформацией из связанных данных.
   * @param id Идентификатор проекта локализации.
   */
  getFundWithDetails(id: Guid): Observable<fund> {
    return this.httpClient.post<fund>(
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


  create(fund: fund): Observable<Guid> {
    return this.httpClient.post<Guid>(this.controllerUrl + "Create", fund);
  }

  update(fund: fund): Observable<Object> {
    return this.httpClient.post(this.controllerUrl + "update", fund);
  } 

  delete(fundId: Guid): Observable<Object> {
    return this.httpClient.delete(this.controllerUrl + "delete/" + fundId);
  }


}
