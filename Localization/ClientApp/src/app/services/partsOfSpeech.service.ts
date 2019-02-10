import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
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
        params: new HttpParams({
          fromObject: {
            glossaryId: glossaryId.toString()
          }
        })
      }
    );
  }
}
