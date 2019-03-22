import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { Guid } from "guid-typescript";
import { PartOfSpeech } from "src/app/models/database-entities/partOfSpeech.type";

@Injectable()
export class PartsOfSpeechService {
  static connectionUrl: string = "api/partofspeech/";

  constructor(private httpClient: HttpClient) {}

  public getListByGlossaryId(glossaryId: Guid): Observable<PartOfSpeech[]> {
    return this.httpClient.post<PartOfSpeech[]>(
      PartsOfSpeechService.connectionUrl + "List/" + glossaryId,
      glossaryId,
      {
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      }
    );
  }
}
