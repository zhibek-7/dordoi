import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TimeZone } from '../models/database-entities/timeZone.type';

@Injectable()
export class TimeZoneService {

  private url: string = 'api/TimeZone/';

  constructor(private httpClient: HttpClient) {}

  /** Возвращает список часовых поясов */
  getAll(): Observable<TimeZone[]> {
    return this.httpClient.post<TimeZone[]>(this.url, null);
  }

}
