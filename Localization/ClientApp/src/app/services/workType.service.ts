import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse, HttpHeaders  } from '@angular/common/http';
import { WorkType } from "../models/database-entities/workType.type";
import { Observable } from 'rxjs';

@Injectable()
export class WorkTypeService {

  private url: string = "api/WorkType/";

  constructor(private httpClient: HttpClient) { }

  getWorkTypes(): Observable<WorkType[]> {
    return this.httpClient.post<WorkType[]>(this.url + 'list', null);
  }

  getWorkTypeByID(id: number): Observable<WorkType> {
    return this.httpClient.post<WorkType>(this.url + id.toString(), id);
  }
}
