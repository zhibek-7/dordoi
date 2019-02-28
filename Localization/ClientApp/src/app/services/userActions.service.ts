import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders  } from '@angular/common/http';
import { UserAction } from '../models/database-entities/userAction.type';
import { Observable, from } from 'rxjs';
import { filter, map } from 'rxjs/operators';

@Injectable()
export class UserActionsService {

  private url: string = "api/UserAction/";

  constructor(private httpClient: HttpClient) { }

  getActionsList(): Observable<UserAction[]> {
    return this.httpClient.post<UserAction[]>(this.url + "List", null);
  }

  getUserActionsByProjectId(
    projectId: number,
    workTypeId: number,
    userId: number,
    localeId: number,
    limit: number,
    offset: number,
    sortBy?: string[],
    sortAscending?: boolean,
  ): Observable<HttpResponse<UserAction[]>> {
    const url = `${this.url}/List/byProjectId/${projectId}`;
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
    return this.httpClient
      .post<UserAction[]>(
        url,
        body,
        {
          observe: 'response',
        });
  }

  getProjectActionsList(project: string): Observable<UserAction[]> {
    return this.getActionsList()
      .pipe(
        map(actions => actions.filter(action => action.project_name === project))
      );
  }

}
