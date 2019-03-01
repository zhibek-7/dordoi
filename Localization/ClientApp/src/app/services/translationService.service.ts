import { Injectable } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

import { Translation } from "../models/database-entities/translation.type";
import { TranslationWithFile } from "../work-panel/localEntites/translations/translationWithFile.type";
import { SimilarTranslation } from "../work-panel/localEntites/translations/similarTranslation.type";
import { TranslationSubstring } from "../models/database-entities/translationSubstring.type";

import { Observable } from "rxjs";

@Injectable()
export class TranslationService {
  private url: string =
    document.getElementsByTagName("base")[0].href + "api/translation";

  constructor(private http: HttpClient) {}

  async createTranslation(translate: Translation) {
    return await this.http.post<number>(this.url, translate).toPromise();
  }

  async getAllTranslationsInStringById(idString: number) {
    let translations: Translation[] = await this.http
      .get<Translation[]>(this.url + "/InString/" + idString)
      .toPromise();
    return translations;
  }

  async deleteTranslation(idTranslation: number) {
    await this.http
      .delete(this.url + "/DeleteTranslation/" + idTranslation)
      .toPromise();
  }

  async acceptTranslate(translationId: number) {
    await this.http
      .put(this.url + "/AcceptTranslation/" + translationId, true)
      .toPromise();
  }

  async acceptFinalTranslation(translationId: number) {
    await this.http
      .put(this.url + "/AcceptFinalTranslation/" + translationId, true)
      .toPromise();
  }

  async rejectFinalTranslattion(translationId: number) {
    await this.http
      .put(this.url + "/RejectFinalTranslation/" + translationId, false)
      .toPromise();
  }

  async rejectTranslate(translationId: number) {
    await this.http
      .put(this.url + "/RejectTranslation/" + translationId, false)
      .toPromise();
  }

  updateTranslation(updatedTranslation: Translation): Observable<Object> {
    return this.http.put(this.url + "/" + updatedTranslation.id, updatedTranslation);
  }

  findTranslationByMemory(
    currentProjectId: number,
    translationText: string
  ): Observable<TranslationWithFile[]> {
    return this.http.post<TranslationWithFile[]>(
      this.url +
        "/FindTranslationByMemory/" +
        currentProjectId +
        "/" +
        translationText,
        null);
  }

  findSimilarTranslations(
    currentProjectId: number,
    translationSubsting: TranslationSubstring
  ): Observable<SimilarTranslation[]> {
    return this.http.post<SimilarTranslation[]>(this.url + "/FindSimilarTranslations/" + currentProjectId, translationSubsting);
  }
}
