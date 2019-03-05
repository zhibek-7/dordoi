import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class GuidsService {

  private url: string = "api/guids/";

  constructor(private httpClient: HttpClient) { }

  getNew(): Observable<string> {
    return this.httpClient.post(this.url + '/getNew', null, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken")),
      responseType: "text"
    });
  }

}
