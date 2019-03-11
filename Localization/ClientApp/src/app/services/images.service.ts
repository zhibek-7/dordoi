import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Image } from 'src/app/models/database-entities/image.type';

@Injectable()
export class ImagesService {

  private _url = "api/images";

  constructor(private http: HttpClient) { }

  public getByProjectId(projectId: number): Observable<Image[]> {
    let body : any = {};
    body.projectId = projectId;
    return this.http
      .post<Image[]>(this._url + "/getByProjectId", body, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      });
  }

}
