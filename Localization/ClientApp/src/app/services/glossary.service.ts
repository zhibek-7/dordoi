import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { GlossariesForEditing, GlossariesTableViewDTO } from 'src/app/models/DTO/glossariesDTO.type';

import { Locale } from 'src/app/models/database-entities/locale.type';
import { localizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';

@Injectable()
export class GlossaryService {
  static connectionUrl: string = 'api/glossaries/';

  constructor(private httpClient: HttpClient) { }
  
  getGlossariesDTO(): Observable<GlossariesTableViewDTO[]> {
    return this.httpClient.get<GlossariesTableViewDTO[]>(GlossaryService.connectionUrl);
  }

  getLocales(): Observable<Locale[]> {
    return this.httpClient.get<Locale[]>(GlossaryService.connectionUrl + "locales/list");
  }

  getlocalizationProjectForSelectDTO(): Observable<localizationProjectForSelectDTO[]> {
    return this.httpClient.get<localizationProjectForSelectDTO[]>(GlossaryService.connectionUrl + "localizationProjects/list");
  }

  addNewGlossary(glossary: GlossariesForEditing): Observable<Object> {
    return this.httpClient.post(GlossaryService.connectionUrl + "newGlossary", glossary);
  }

  editSaveGlossary(glossary: GlossariesForEditing): Observable<Object> {
    return this.httpClient.post(GlossaryService.connectionUrl + "editSaveGlossary", glossary);
  }

  deleteGlossary(glossaryId: number): Observable<Object> {
    return this.httpClient.delete(GlossaryService.connectionUrl + "deleteGlossary/" + glossaryId);
  }

  clearGlossary(glossaryId: number): Observable<Object> {
    return this.httpClient.delete(GlossaryService.connectionUrl + "clearGlossary/" + glossaryId);
  }

  async getGlossaryForEditing(glossaryId: number): Promise<GlossariesForEditing> {
    let result = await this.httpClient.post<GlossariesForEditing>(GlossaryService.connectionUrl + "edit", glossaryId).toPromise();
    return result;
  }
}
