import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Project } from '../models/Project';

@Injectable()
export class ProjectsService  implements Project{
    name: string;
    id: string;
    url: string;
    owner: string;
    created: Date;
    changed: Date;
    hasErr: boolean;
    type: string;
    sourceLanguage: string;

    constructor(private httpClient: HttpClient) {}

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
            sourceLanguage: 'English'
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
            sourceLanguage: 'Spanish'
        }
    ];

    getProjects(userName: string) {
        const getUrl = ''; // тут формируем строку подключения к БД для получения проектов
        return this.httpClient.get(getUrl);
    }

    addProject(project: Project) {
        this.projects.push(project);
    }

    updateProject(project: Project) {
        this.projects[project.id] = project;
    }
} 