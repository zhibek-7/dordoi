import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders  } from "@angular/common/http";
import { Observable } from "rxjs";

import { PartOfSpeech } from "src/app/models/database-entities/partOfSpeech.type";

@Injectable()
export class PartsOfSpeechService {
  static connectionUrl: string = "api/partofspeech/";

  constructor(private httpClient: HttpClient) {}

  public getListByGlossaryId(glossaryId: number): Observable<PartOfSpeech[]> {
    return this.httpClient.get<PartOfSpeech[]>(
      PartsOfSpeechService.connectionUrl,
      {
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken")), params: new HttpParams({
          fromObject: {
            glossaryId: glossaryId.toString()
          }
        })
    }
    );
  }
}
