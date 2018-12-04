import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { Language } from '../models/Language';
import { Project } from '../models/Project';
import { LanguageService } from '../services/languages.service';
import { ProjectsService } from '../services/projects.service';


@Component({
  selector: 'app-new-project',
  templateUrl: './new-project.component.html',
  styleUrls: ['./new-project.component.css']
})
export class NewProjectComponent implements OnInit {

  value = '';

  newProject = new ProjectsService(this.httpClient);

  checkedLanguages = [];

  languages: Language[];

  myForm = new FormGroup({
    name: new FormControl(null),
    url: new FormControl(null),
    language: new FormControl(null)
  });

  constructor(private languageService: LanguageService, private projectsService: ProjectsService, private httpClient: HttpClient) { }

  ngOnInit() {
    console.log('new-project.component ngOnInit ---!!!!!!!!!');
    this.languages = this.languageService.languages;
    //console.log(this.newProject);
  }

  checkLanguage(event) {

    if (event.target.checked) {
      this.checkedLanguages.push({
        name: event.target.value
      });
    } else {

      const indexToRemove = this.checkedLanguages.indexOf(event.target.value);
      this.checkedLanguages.splice(indexToRemove);
    }
    console.log('checkLanguage ---!!!!!!!!!');
    // console.log(this.checkedLanguages)
  }

  addProject() {
    console.log('addProject ---!!!!!!!!!');

    this.newProject.name = this.myForm.get('name').value;
   this.newProject.id = (Math.floor(Math.random() * 10000) + 1).toString();
    this.newProject.url = this.myForm.get('url').value;
    this.newProject.owner = 'Я';
    this.newProject.created = new Date;
    this.newProject.changed = new Date;
    this.newProject.hasErr = false;
    this.newProject.type = 'public';
    this.newProject.sourceLanguage = this.myForm.get('language').value;

    this.projectsService.addProject(this.newProject);
  }

}
