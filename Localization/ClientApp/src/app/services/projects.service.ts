import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Project } from '../models/Project';
import { Observable, of } from 'rxjs';

@Injectable()
export class ProjectsService {

  private controllerUrl: string = "api/Project/";

    //name: string;
    //id: string;
    //url: string;
    //owner: string;
    //created: Date;
    //changed: Date;
    //hasErr: boolean;
    //type: string;
    //sourceLanguage: string;

    constructor(private httpClient: HttpClient) {}

    //Тестовые данные потом не забыть удалить
    projects: Project[] = [
        {
            name: 'Тестовый проект',
            id: '1111',
            url: '1111',
            owner: 'я',
            created: new Date(2018, 8, 28),
            changed: new Date(2018, 9, 10),
            hasErr: true,
            type: 'privat',
            sourceLanguage: 'English',

            description: 'Test project not from DB',
            visibility: true,
            dateOfCreation: new Date(2018, 8, 28),
            lastActivity: new Date(2018, 9, 10),
            ID_SourceLocale: 300,
            ableToDownload: true,
            ableToLeftErrors: true,
            defaultString: "",
            notifyNew: true,
            notifyFinish: true,
            notifyConfirm: true,
            logo: undefined
        },
        {
            name: 'Тестовый проект 2',
            id: '2222',
            url: '2222',
            owner: 'я',
            created: new Date(2018, 9, 5),
            changed: new  Date(2018, 9, 10),
            hasErr: false,
            type: 'public',
            sourceLanguage: 'Spanish',
            description: 'Test project not from DB',
            visibility: true,
            dateOfCreation: new Date(2018, 9, 5),
            lastActivity: new Date(2018, 9, 10),
            ID_SourceLocale: 300,
            ableToDownload: true,
            ableToLeftErrors: true,
            defaultString: "",
            notifyNew: true,
            notifyFinish: true,
            notifyConfirm: true,
            logo: undefined
        }
    ];

    getProjects(): Observable<Project[]> {
        return this.httpClient.get<Project[]>(this.controllerUrl + "List");
    }

    getProject(id: number): Observable<Project> {
        return this.httpClient.get<Project>(this.controllerUrl + id);
    }

    addProject(project: Project) {
        this.projects.push(project);
    }

    updateProject(project: Project) {
        this.projects[project.id] = project;
    }
}
