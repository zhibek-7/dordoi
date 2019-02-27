import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { Image } from '../models/database-entities/image.type';

@Injectable()
export class TranslationSubstringService {

    private url: string = "api/string/";

    constructor(private http: HttpClient) {

    }

    async getStringById(){        
      let asyncResult = await this.http.get<TranslationSubstring>(this.url, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    }).toPromise();
        return asyncResult;    
    }

    async getStrings(){
      let strings: TranslationSubstring[] = await this.http.get<TranslationSubstring[]>(this.url, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    }).toPromise();
        return strings;
    }

    getImagesByTranslationSubstringId(translationSubstringId: number): Observable<Image[]>{
      return this.http.post<Image[]>(this.url + "GetImagesByStringId/" + translationSubstringId, null, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
    }

    uploadImageToTranslationSubstring(fileToUpload: File[], translationSubstringId: number) {
        const formData: FormData = new FormData();

        fileToUpload.forEach(element => {
            formData.set('Image', element); 
            formData.append('TranslationSubstringId', translationSubstringId.toString());
            return this.http.post(this.url + "UploadImageToTranslationSubstring", formData, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    }).toPromise();
        });        
    }

    getStringsInFile(idFile: number): Observable<TranslationSubstring[]>{
      return this.http.get<TranslationSubstring[]>(this.url + "InFile/" + idFile, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
    }

  getTranslationSubstringStatus(translationSubstringId: number): Observable<string>{
    return this.http.post<string>(this.url + "Status/" + translationSubstringId, null, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
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
    return this.http.get<TranslationSubstring[]>(this.url + 'ByProjectId/' + projectId,
       {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
,
        params: params,
        observe: 'response'
    }
);
  }

  deleteTranslationSubstring(id: number): Observable<Object> {
    return this.http.delete(this.url + id.toString(), {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  updateTranslationSubstring(translationSubstring: TranslationSubstring): Observable<Object> {
    return this.http.put(this.url + translationSubstring.id.toString(), translationSubstring, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  getTranslationLocalesForString(id: number): Observable<Locale[]> {
    return this.http.get<Locale[]>(this.url + id + '/locales', {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

  setTranslationLocalesForString(id: number, localesIds: number[]): Observable<Object> {
    return this.http.put(this.url + id + '/locales', localesIds, {
      headers: new HttpHeaders().set('Authorization', "Bearer " + sessionStorage.getItem("userToken"))
    });
  }

}
