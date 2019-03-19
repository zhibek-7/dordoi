import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Image } from 'src/app/models/database-entities/image.type';

@Injectable()
export class ImagesService {

  private _url = "api/images";

  constructor(private http: HttpClient) { }

  public getByProjectId(projectId: number, imageNameFilter: string, offset: number, limit: number): Observable<HttpResponse<Image[]>> {
    let body: any = {};
    body.projectId = projectId;
    if (limit) {
      body.limit = limit;
    }
    if (imageNameFilter && imageNameFilter != "") {
      body.imageNameFilter = imageNameFilter;
    }
    if (offset) {
      body.offset = offset;
    }
    return this.http
      .post<Image[]>(this._url + "/getByProjectId", body, {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        ),
        observe: "response"
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
