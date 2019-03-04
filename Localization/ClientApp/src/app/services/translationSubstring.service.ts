import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
import { TranslationSubstringForEditingDTO, TranslationSubstringTableViewDTO } from 'src/app/models/DTO/TranslationSubstringDTO.type';

import { Locale } from 'src/app/models/database-entities/locale.type';
import { Image } from '../models/database-entities/image.type';


@Injectable()
export class TranslationSubstringService {

    private url: string = "api/string/";

  constructor(private http: HttpClient) {

    }

    async getStringById(){        
      let asyncResult = await this.http.post<TranslationSubstring>(this.url, null).toPromise();
        return asyncResult;    
    }

    async getStrings(){
      let strings: TranslationSubstring[] = await this.http.post<TranslationSubstring[]>(this.url, null).toPromise();
        return strings;
    }

    getImagesByTranslationSubstringId(translationSubstringId: number): Observable<Image[]>{
      return this.http.post<Image[]>(this.url + "GetImagesByStringId/" + translationSubstringId, null);
    }

    uploadImageToTranslationSubstring(fileToUpload: File[], translationSubstringId: number) {
        const formData: FormData = new FormData();

        fileToUpload.forEach(element => {
            formData.set('Image', element); 
            formData.append('TranslationSubstringId', translationSubstringId.toString());
            return this.http.post(this.url + "UploadImageToTranslationSubstring", formData).toPromise();
        });        
    }

    getStringsInFile(idFile: number): Observable<TranslationSubstring[]>{
      return this.http.post<TranslationSubstring[]>(this.url + "InFile/" + idFile, idFile);
    }

  getTranslationSubstringStatus(translationSubstringId: number): Observable<string>{
    return this.http.post<string>(this.url + "Status/" + translationSubstringId, null);
  }

  getStringsByProjectId(
    projectId: number,
    fileId?: number,
    searchString?: string,
    limit?: number,
    offset?: number,
    sortBy?: string[],
    sortAscending?: boolean): Observable<HttpResponse<TranslationSubstring[]>>
  {
    let params = new HttpParams();
    if (searchString && searchString != '') {
      params = params.set('searchString', searchString);
    }
    if (fileId) {
      params = params.set('fileId', fileId.toString());
    }
    if (limit) {
      params = params.set('limit', limit.toString());
    }
    if (offset) {
      params = params.set('offset', offset.toString());
    }
    if (sortBy && sortBy.length > 0) {
      sortBy.forEach(sortByItem => params = params.append('sortBy', sortByItem));
      if (sortAscending !== undefined) {
        params = params.set('sortAscending', sortAscending.toString());
      }
    }
    return this.http.post<TranslationSubstring[]>(this.url + 'ByProjectId/' + projectId, projectId,
       {
        params: params,
        observe: 'response',
        headers: new HttpHeaders().set('Authorization',"Bearer " + sessionStorage.getItem("userToken"))
      });
  }

  deleteTranslationSubstring(id: number): Observable<Object> {
    return this.http.delete(this.url + id.toString());
  }

  updateTranslationSubstring(translationSubstring: TranslationSubstring): Observable<Object> {
    return this.http.put(this.url + translationSubstring.id.toString(), translationSubstring);
  }

  getTranslationLocalesForString(id: number): Observable<Locale[]> {
    return this.http.post<Locale[]>(this.url + id + '/locales', id);
  }

  setTranslationLocalesForString(id: number, localesIds: number[]): Observable<Object> {
    return this.http.put(this.url + id + '/locales', localesIds);
  }


  
  /**
   * Возвращает список строк текущего проекта, со строками перечислений имен связанных объектов.
   */
  getAllWithTranslationMemoryByProject(projectId: number): Observable<TranslationSubstringTableViewDTO[]> {
    //let projectId = this.projectsService.currentProjectId;
    return this.http.post<TranslationSubstringTableViewDTO[]>(this.url + "getAllWithTranslationMemoryByProject", projectId, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  /**
   * Обновление поля substring_to_translate
   * @param translationSubstring
   */
  saveSubstringToTranslate(translationSubstring: TranslationSubstringTableViewDTO): Observable<boolean> {
    return this.http.post<boolean>(this.url + "saveSubstringToTranslate", translationSubstring);
  }

  /**
   * Удаление строк.
   * @param ids Идентификаторы строк.
   */
  deleteRange(ids: number[]): Observable<boolean> {
    let params = new HttpParams();
    ids.forEach(id => params = params.append('ids', id.toString()));
    return this.http.post<boolean>(this.url + "deleteRange", null,
      {
        params: params
      });
  }

}
