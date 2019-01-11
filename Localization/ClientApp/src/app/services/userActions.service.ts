import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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

  getProjectActionsList(project: string): Observable<UserAction[]> {
    return this.getActionsList()
      .pipe(
        map(actions => actions.filter(action => action.Project === project))
      );
  }

}
