import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpResponse } from "@angular/common/http";
import { Observable } from "rxjs";
import { Guid } from "guid-typescript";
import { Image } from "src/app/models/database-entities/image.type";

@Injectable()
export class ImagesService {
  private _url = "api/images";

  constructor(private http: HttpClient) {}

  public getByProjectId(
    projectId: Guid,
    imageNameFilter: string,
    offset: number,
    limit: number
  ): Observable<HttpResponse<Image[]>> {
    let body: any = {};
    let b: GetImagesByProjectIdAsyncParam = new GetImagesByProjectIdAsyncParam();
    body.projectId = projectId;

    if (limit) {
      body.limit = limit;
      b.limit = limit;
    }
    if (imageNameFilter && imageNameFilter != "") {
      body.imageNameFilter = imageNameFilter;
      b.imageNameFilter = imageNameFilter;
    }
    if (offset) {
      body.offset = offset;
      b.offset = offset;
    }
    console.log("getByProjectId--" + projectId);
    //b.projectId = Guid.create();
    return this.http.post<Image[]>(
      this._url + "/getByProjectId/" + projectId,
      b,
      {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        ),
        observe: "response"
      }
    );
  }

  updateImage(updatedImage: Image): Observable<Object> {
    return this.http.post(this._url + "/update", updatedImage, {
      headers: new HttpHeaders().set(
        "Authorization",
        "Bearer " + sessionStorage.getItem("userToken")
      )
    });
  }
}

export class GetImagesByProjectIdAsyncParam {
  projectId: Guid;
  imageNameFilter: string;
  offset?: number;
  limit?: number;
}
