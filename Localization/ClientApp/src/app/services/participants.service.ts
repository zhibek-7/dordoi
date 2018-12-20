import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Participant } from 'src/app/models/Participants/participant.type';

@Injectable()
export class ParticipantsService {

  private url: string = "api/participants/";

  constructor(private httpClient: HttpClient) { }

  getParticipantsByProjectId(
    projectId: number,
    search: string,
    roleIds: number[],
    localeIds: number[],
    limit: number,
    offset: number,
    sortBy: string[],
    sortAscending: boolean,
  ): Observable<HttpResponse<Participant[]>> {
    let params = new HttpParams();
    if (limit) {
      params = params.set('limit', limit.toString());
    }
    if (search && search != '') {
      params = params.set('search', search);
    }
    if (roleIds && roleIds.length > 0) {
      roleIds.forEach(roleId => params = params.append('roleIds', roleId.toString()));
    }
    if (localeIds && localeIds.length > 0) {
      localeIds.forEach(localeId => params = params.append('localeIds', localeId.toString()));
    }
    if (offset) {
      params = params.set('offset', offset.toString());
    }
    if (sortBy && sortBy.length > 0) {
      sortBy.forEach(sortByItem => params = params.append('sortBy', sortByItem));
      if (sortAscending !== undefined) {
        params = params.set('sortAscending', sortAscending.toString());
      }
    }
    return this.httpClient.get<Participant[]>(this.url + 'byProjectId/' + projectId,
      {
        params: params,
        observe: 'response'
      });
  }

  deleteParticipant(projectId: number, participantUserId: number): Observable<Object> {
    return this.httpClient.delete(this.url + 'byProjectId/' + projectId + '/' + participantUserId);
  }

}
