import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ProjectsService } from './projects.service';

import { ProjectTranslationMemory } from '../models/database-entities/projectTranslationMemory.type';

@Injectable()
export class ProjectTranslationMemoryService {

  private url: string = "api/ProjectTranslationMemory/";

  constructor(private http: HttpClient, private projectsService: ProjectsService) { }

  /** Возвращает список связок проект - ((не)назначенных) памяти переводов. */
  getAllByProject(): Observable<ProjectTranslationMemory[]> {
    let projectId = this.projectsService.currentProjectId;
    return this.http.post<ProjectTranslationMemory[]>(this.url + "ByProject", projectId);
  }

  /**
   * Назначение на проект локализации памятей переводов.
   * Пересоздание в таблице "localization_projects_translation_memories" связей.
   * @param idTranslationMemories идентификаторы памятей переводов.
   */
  saveProjectTranslationMemories(idTranslationMemories: number[]): Observable<Object> {
    let projectId = this.projectsService.currentProjectId;
    let params = new HttpParams();
    idTranslationMemories.forEach(id => params = params.append('idTranslationMemories', id.toString()));
    params = params.set('idProject', projectId.toString());
    return this.http.post(this.url + "saveProjectTranslationMemories", null,
      {
        params: params
      });
  }
  
}
