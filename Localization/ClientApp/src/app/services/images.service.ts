import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Image } from 'src/app/models/database-entities/image.type';

@Injectable()
export class ImagesService {

  private _url = "api/images";

  constructor(private http: HttpClient) { }

  public getByProjectId(projectId: number): Observable<Image[]> {
    return this.http
      .post<Image[]>(this._url + "/getByProjectId", projectId, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      });
  }

  updateImage(updatedImage: Image): Observable<Object> {
    return this.http
      .post(this._url + "/update", updatedImage, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      });
  }

}
