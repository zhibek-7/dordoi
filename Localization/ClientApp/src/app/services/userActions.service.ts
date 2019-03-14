import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse, HttpHeaders } from "@angular/common/http";
import { UserAction } from "../models/database-entities/userAction.type";
import { Observable } from "rxjs";

@Injectable()
export class UserActionsService {
  private url: string = "api/UserAction/";

  constructor(private httpClient: HttpClient) {}

  getActionsList(projectId: number): Observable<UserAction[]> {
    console.log("..projectId=" + projectId);
    const formData = new FormData();
    formData.append("project", "" + projectId);
    return this.httpClient.post<UserAction[]>(this.url + "List", formData);
  }

  getUserActionsByProjectId(
    projectId: number,
    workTypeId: number,
    userId: number,
    localeId: number,
    limit: number,
    offset: number,
    sortBy?: string[],
    sortAscending?: boolean
  ): Observable<HttpResponse<UserAction[]>> {
    const url = `${this.url}List/byProjectId/${projectId}`;
    let body: any = {};
    if (workTypeId) {
      body.workTypeId = workTypeId;
    }
    if (userId) {
      body.userId = userId;
    }
    if (localeId) {
      body.localeId = localeId;
    }
    if (limit) {
      body.limit = limit;
    }
    if (offset) {
      body.offset = offset;
    }
    if (sortBy && sortBy.length > 0) {
      body.sortBy = sortBy;
      if (sortAscending !== undefined) {
        body.sortAscending = sortAscending;
      }
    }
    console.log("..url=" + url);

    return this.httpClient.post<UserAction[]>(url, body, {
      headers: new HttpHeaders().set(
        "Authorization",
        "Bearer " + sessionStorage.getItem("userToken")
      ),
      observe: "response"
    });
  }

  /*getProjectActionsList(project: string): Observable<UserAction[]> {
    return this.getActionsList().pipe(
      map(actions => actions.filter(action => action.project_name === project))
    );
  }
  */
}
