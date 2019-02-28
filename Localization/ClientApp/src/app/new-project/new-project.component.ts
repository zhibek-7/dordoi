import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { HttpClient, HttpHeaders  } from '@angular/common/http';

import { Locale } from '../models/database-entities/locale.type';
import { LocalizationProject } from '../models/database-entities/localizationProject.type';
import { LanguageService } from '../services/languages.service';
import { ProjectsService } from '../services/projects.service';


@Component({
  selector: 'app-new-project',
  templateUrl: './new-project.component.html',
  styleUrls: ['./new-project.component.css']
})
export class NewProjectComponent implements OnInit {

  value = '';

  newProject: LocalizationProject;// = new ProjectsService(this.httpClient);

  checkedLanguages = [];

  languages: Locale[];

  myForm = new FormGroup({
    name: new FormControl(null),
    url: new FormControl(null),
    language: new FormControl(null)
  });

  constructor(private languageService: LanguageService, private projectsService: ProjectsService, private httpClient: HttpClient) { }

  ngOnInit() {
    this.languageService.getLanguageList()
      .subscribe(Languages => { this.languages = Languages; },
        error => console.error(error));
    //console.log(this.newProject);
  }

  checkLanguage(event) {

    if (event.target.checked) {
      this.checkedLanguages.push({
        name_text: event.target.value
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

    this.newProject.name_text = this.myForm.get('name_text').value;
    this.newProject.id = Math.floor(Math.random() * 10000) + 1;
    this.newProject.url = this.myForm.get('url').value;

    this.projectsService.addProject(this.newProject);
  }

}
