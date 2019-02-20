import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse, HttpHeaders  } from '@angular/common/http';
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
    roleShort: string[]
  ): Observable<HttpResponse<Participant[]>> {
    let body: any = {};
    if (limit) {
      body.limit = limit;
    }
    if (search && search != '') {
      body.search = search;
    }
    if (roleIds && roleIds.length > 0) {
      body.roleIds = roleIds;
    }
    if (localeIds && localeIds.length > 0) {
      body.localeIds = localeIds;
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
    if (roleShort && roleShort.length > 0) {
      body.role_Short = roleShort;
    }
    return this.httpClient.post<Participant[]>(this.url + 'byProjectId/' + projectId + '/list',
      body,
      {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken")), observe: 'response'
    });
  }

  deleteParticipant(projectId: number, participantUserId: number): Observable<Object> {
    return this.httpClient.delete(this.url + 'byProjectId/' + projectId + '/' + participantUserId, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    });
  }

  addParticipant(projectId: number, userId: number, roleId: number): Observable<Object> {
    return this.httpClient.post(this.url + projectId + '/' + userId + '/' + roleId, null, {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))      
    });
  }

}
