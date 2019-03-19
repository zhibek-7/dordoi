import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";

import { Translation } from "../models/database-entities/translation.type";
import { TranslationWithFile } from "../work-panel/localEntites/translations/translationWithFile.type";
import { SimilarTranslation } from "../work-panel/localEntites/translations/similarTranslation.type";
import { TranslationSubstring } from "../models/database-entities/translationSubstring.type";

import { Observable } from "rxjs";
import { TranslationDTO } from "../models/DTO/translationDTO";
import { TranslationWithLocaleText } from "../work-panel/localEntites/translations/translationWithLocaleText.type";

@Injectable()
export class TranslationService {
  private url: string =
    document.getElementsByTagName("base")[0].href + "api/translation";

  constructor(private http: HttpClient) {}

  async createTranslation(translation: Translation) {
    return await this.http
      .post<number>(this.url + "/Create", translation)
      .toPromise();
  }

  async getAllTranslationsInStringById(idString: number) {
    let translations: Translation[] = await this.http
      .post<Translation[]>(this.url + "/InString/" + idString, idString)
      .toPromise();
    return translations;
  }

  async getAllTranslationsInStringByIdAndLocale(idString: number, localeId: number) {
    const params = new HttpParams().set("stringId", idString.toString()).set("localeId", localeId.toString());

    let translations: Translation[] = await this.http
      .post<Translation[]>(this.url + "/InStringWithLocale/", params)
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
    return this.http.put(
      this.url + "/" + updatedTranslation.id,
      updatedTranslation
    );
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
      null
    );
  }

  findTranslationsInOtherLanguages(
    currentProjectId: number,
    translationSubsting: TranslationSubstring,
    localeId: number
  ): Observable<TranslationWithLocaleText[]>{

    const params = new HttpParams().set("currentProjectId", currentProjectId.toString())
                                   .set("translationSubstingId", translationSubsting.id.toString())
                                   .set("localeId", localeId.toString());

    return this.http.post<TranslationWithLocaleText[]>(this.url + "/FindTranslationsInOtherLanguages/", params);
  }

  findSimilarTranslations(
    currentProjectId: number,
    translationSubsting: TranslationSubstring,
    localeId: number
  ): Observable<SimilarTranslation[]> {
    return this.http.post<SimilarTranslation[]>(
      this.url + "/FindSimilarTranslations/" + currentProjectId + "/" + localeId,
      translationSubsting
    );
  }

  /**
   * Возвращает все варианты перевода конкретной фразы с языком перевода.
   * @param idString Идентификатор фразы.
   */
  getAllTranslationsInStringWithLocaleById(idString: number): Observable<TranslationDTO[]> {
    return this.http.post<TranslationDTO[]>(this.url + "/InStringWithLocale/" + idString, idString);
  }

  /**
   * Обновление поля translated.
   * @param translations
   */
  saveTranslated(translations: TranslationDTO[]): Observable<boolean> {
    return this.http.post<boolean>(this.url + "/saveTranslated", translations);
  }

}
