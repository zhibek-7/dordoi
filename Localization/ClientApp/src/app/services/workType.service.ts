import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { WorkType } from "../models/database-entities/workType.type";
import { Observable } from 'rxjs';

@Injectable()
export class WorkTypeService {

  private url: String = "api/WorkType/";

  constructor(private httpClient: HttpClient) { }

  getWorkTypes(): Observable<WorkType[]> {
    return this.httpClient.get<WorkType[]>(this.url + 'list');
  }

  getWorkTypeByID(id: number): Observable<WorkType> {
    return this.httpClient.get<WorkType>(this.url + id.toString());
  }
}
