import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { TranslationSubstring } from '../models/database-entities/translationSubstring.type';
import { Observable } from 'rxjs';

@Injectable()
export class TranslationSubstringService {

    private url: string = "api/string/";

    constructor(private http: HttpClient) {

    }

    async getStringById(){        
      let asyncResult = await this.http.get<TranslationSubstring>(this.url).toPromise();
        return asyncResult;    
    }

    async getStrings(){
      let strings: TranslationSubstring[] = await this.http.get<TranslationSubstring[]>(this.url).toPromise();
        return strings;
    }

  getStringsInFile(idFile: number): Observable<TranslationSubstring[]>{
    return this.http.get<TranslationSubstring[]>(this.url + "InFile/" + idFile);
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
        params: params,
        observe: 'response'
      });
  }

  deleteTranslationSubstring(id: number): Observable<Object> {
    return this.http.delete(this.url + id.toString());
  }

  updateTranslationSubstring(translationSubstring: TranslationSubstring): Observable<Object> {
    return this.http.put(this.url + translationSubstring.id.toString(), translationSubstring);
  }

}
