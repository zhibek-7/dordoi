import { Injectable } from "@angular/core";
import {
  HttpClient,
  HttpParams,
  HttpResponse,
  HttpHeaders
} from "@angular/common/http";
import { Observable } from "rxjs";

import { TranslationSubstring } from "src/app/models/database-entities/translationSubstring.type";
import { TranslationSubstringTableViewDTO } from "src/app/models/DTO/TranslationSubstringDTO.type";

import { Locale } from "src/app/models/database-entities/locale.type";
import { Image } from "../models/database-entities/image.type";
import { Guid } from "guid-typescript";

@Injectable()
export class TranslationSubstringService {
  private url: string = "api/string/";

  constructor(private http: HttpClient) {}

  async getStringById() {
    let asyncResult = await this.http
      .post<TranslationSubstring>(this.url, null)
      .toPromise();
    return asyncResult;
  }

  async getStrings() {
    let strings: TranslationSubstring[] = await this.http
      .post<TranslationSubstring[]>(this.url, null)
      .toPromise();
    return strings;
  }

  getImagesByTranslationSubstringId(
    translationSubstringId: Guid
  ): Observable<Image[]> {
    return this.http.post<Image[]>(
      this.url + "GetImagesByStringId/" + translationSubstringId,
      null
    );
  }

  uploadImageToTranslationSubstring(
    fileToUpload: File[],
    translationSubstringId: Guid
  ) {
    const formData: FormData = new FormData();

    // fileToUpload.forEach(element => {
    formData.set("Image", fileToUpload[0]);
    formData.append(
      "TranslationSubstringId",
      translationSubstringId.toString()
    );
    return this.http.post(
      this.url + "UploadImageToTranslationSubstring",
      formData
    );
    // });
  }

  getStringsInFile(
    fileId: Guid,
    localeId: Guid
  ): Observable<TranslationSubstring[]> {
    let params = new HttpParams()
      .set("fileId", fileId.toString())
      .set("localeId", localeId.toString());

    return this.http.post<TranslationSubstring[]>(
      this.url + "getStringsInFileWithByLocale/",
      params
    );
  }

  getTranslationSubstringStatus(
    translationSubstringId: Guid
  ): Observable<string> {
    return this.http.post<string>(
      this.url + "Status/" + translationSubstringId,
      null
    );
  }

  setTranslationSubstringStatus(
    translationSubstringId: Guid,
    status: string
  ): Observable<string> {
    let params = new HttpParams()
      .set("translationSubstringId", translationSubstringId.toString())
      .set("status", status);

    return this.http.post<string>(this.url + "SetStatus/", params);
  }

  getStringsByProjectId(
    projectId: Guid,
    fileId?: Guid,
    searchString?: string,
    limit?: number,
    offset?: number,
    sortBy?: string[],
    sortAscending?: boolean
  ): Observable<HttpResponse<TranslationSubstring[]>> {
    let params = new HttpParams();
    if (searchString && searchString != "") {
      params = params.set("searchString", searchString);
    }
    if (fileId) {
      params = params.set("fileId", fileId.toString());
    }
    if (limit) {
      params = params.set("limit", limit.toString());
    }
    if (offset) {
      params = params.set("offset", offset.toString());
    }
    if (sortBy && sortBy.length > 0) {
      sortBy.forEach(
        sortByItem => (params = params.append("sortBy", sortByItem))
      );
      if (sortAscending !== undefined) {
        params = params.set("sortAscending", sortAscending.toString());
      }
    }
    return this.http.post<TranslationSubstring[]>(
      this.url + "ByProjectId/" + projectId,
      projectId,
      {
        params: params,
        observe: "response",
        headers: new HttpHeaders().set(
          "Authorization",
          "Bearer " + sessionStorage.getItem("userToken")
        )
      }
    );
  }

  deleteTranslationSubstring(id: Guid): Observable<Object> {
    return this.http.delete(this.url + id.toString());
  }

  updateTranslationSubstring(
    translationSubstring: TranslationSubstring
  ): Observable<Object> {
    return this.http.put(
      this.url + translationSubstring.id.toString(),
      translationSubstring
    );
  }

  getTranslationLocalesForString(id: Guid): Observable<Locale[]> {
    return this.http.post<Locale[]>(this.url + id + "/locales", id);
  }

  setTranslationLocalesForString(
    id: Guid,
    localesIds: Guid[]
  ): Observable<Object> {
    return this.http.put(this.url + id + "/locales", localesIds);
  }

  /**
   * Возвращает список строк текущего проекта, со строками перечислений имен связанных объектов.
   * @param projectId Идентификатор проекта.
   * @param translationMemoryId Идентификатор памяти переводов.
   * @param searchString Шаблон строки (поиск по substring_to_translate).
   * @param limit Количество возвращаемых строк.
   * @param offset Количество пропущенных строк.
   * @param sortBy Имя сортируемого столбца.
   * @param sortAscending Порядок сортировки.
   */
  getAllWithTranslationMemoryByProject(
    projectId: Guid,
    translationMemoryId?: Guid,
    searchString?: string,
    limit?: number,
    offset?: number,
    sortBy?: string[],
    sortAscending?: boolean
  ): Observable<HttpResponse<TranslationSubstringTableViewDTO[]>> {
    let params = new HttpParams();
    if (projectId) {
      params = params.set("projectId", projectId.toString());
    }
    if (searchString && searchString != "") {
      params = params.set("searchString", searchString);
    }
    if (translationMemoryId) {
      params = params.set(
        "translationMemoryId",
        translationMemoryId.toString()
      );
    }
    if (limit) {
      params = params.set("limit", limit.toString());
    }
    if (offset) {
      params = params.set("offset", offset.toString());
    }
    if (sortBy && sortBy.length > 0) {
      sortBy.forEach(
        sortByItem => (params = params.append("sortBy", sortByItem))
      );
      if (sortAscending !== undefined) {
        params = params.set("sortAscending", sortAscending.toString());
      }
    }
    return this.http.post<TranslationSubstringTableViewDTO[]>(
      this.url + "getAllWithTranslationMemoryByProject",
      null,
      {
        params: params,
        observe: "response"
      }
    );
  }

  /**
   * Обновление поля substring_to_translate
   * @param translationSubstring
   */
  saveSubstringToTranslate(
    translationSubstring: TranslationSubstringTableViewDTO
  ): Observable<boolean> {
    return this.http.post<boolean>(
      this.url + "saveSubstringToTranslate",
      translationSubstring
    );
  }

  /**
   * Удаление строк.
   * @param ids Идентификаторы строк.
   */
  deleteRange(ids: Guid[]): Observable<boolean> {
    let params = new HttpParams();
    ids.forEach(id => (params = params.append("ids", id.toString())));
    return this.http.post<boolean>(this.url + "deleteRange", null, {
      params: params
    });
  }
}
