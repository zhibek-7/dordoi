import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { LocalizationProject } from '../models/database-entities/localizationProject.type';
import { ProjectsService } from '../services/projects.service';
import { MatTableDataSource } from '@angular/material';

import { LanguageService } from '../services/languages.service';
import { UserService } from '../services/user.service';
import { Locale } from '../models/database-entities/locale.type';
import { User } from '../models/database-entities/user.type';

//export interface InfoElement {
//  file: string;
//  variant: string;
//  language: string;
//  time: string;
//  userId: string;
//}

//const allLanguages = [
//  'Русский',
//  'Французский',
//  'Китайский',
//  'Английский',
//];

//const ELEMENT_DATA: InfoElement[] = [
//  // tslint:disable-next-line:max-line-length
//  { variant: ' Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не т',
//    file: 'Hydrogen.txt',
//    language: 'Русский',
//    time: '6.25',
//    userId: '14585' },
//  // tslint:disable-next-line:max-line-length
//  { variant: ' Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не т',
//    file: 'Hydrogen.txt',
//    language: 'Русский',
//    time: '6.25',
//    userId: '14585' },
//  // tslint:disable-next-line:max-line-length
//  { variant: ' Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsu',
//    file: 'Hydrogen.txt',
//    language: 'Русский',
//    time: '6.25',
//    userId: '14585' },
//  // tslint:disable-next-line:max-line-length
//  { variant: ' Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum н',
//    file: 'Hydrogen.txt',
//    language: 'Русский',
//    time: '6.25',
//    userId: '14585' },
//  // tslint:disable-next-line:max-line-length
//  { variant: ' Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum н',
//    file: 'Hydrogen.txt',
//    language: 'Французский',
//    time: '6.25',
//    userId: '89899' },
//  // tslint:disable-next-line:max-line-length
//  { variant: ' Lorem Ipsum является стандартной "рыбой" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum н',
//    file: 'Hydrogen.txt',
//    language: 'Китайский',
//    time: '6.25',
//    userId: '89899' },
//];

@Component({
  selector: 'app-project-page',
  templateUrl: './project-page.component.html',
  styleUrls: ['./project-page.component.css'],
  providers: [LanguageService, UserService ]
})
export class ProjectPageComponent implements OnInit {
  currentProject: LocalizationProject;
  name: string;
  projectId: number;
  //langOptions = allLanguages;
  langList: Array<Locale>;
  panelOpenState = false;
  selected = 'none';
  selectedLang = 'none';
  users: Array<User>;
  filtredUsers = [];

  displayedColumns: string[] = ['variant', 'file', 'language', 'time'];
  //dataSource = new MatTableDataSource(ELEMENT_DATA);
  currentUserName = '';

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectsService,
    private languagesService: LanguageService,
    private userService: UserService
  ) { }

  ngOnInit() {
    this.currentUserName = sessionStorage.getItem('currentUserName');

    this.languagesService.getLanguageList()
      .subscribe(Languages => { this.langList = Languages; },
        error => console.error(error));

    this.userService.getUserList()
      .subscribe(Users => { this.users = Users; },
        error => console.error(error));

    this.projectId = Number(sessionStorage.getItem('ProjecID'));

    this.projectService.getProject(this.projectId)
      .subscribe(project => {
         this.currentProject = project;
        },
        error => console.error(error));

    this.filtredUsers = this.users;
    //this.langList = [
    //  { name: 'Французский', icon: '../../assets/images/11.png' },
    //  { name: 'Мандинго', icon: '../../assets/images/22.png' },
    //  { name: 'Испанский', icon: '../../assets/images/333.png' }
    //];
  }

  applyFilterUsers(filterValue: string) {
    if (filterValue === 'none') {
      this.filtredUsers = this.users;
    } else {
      this.filtredUsers = this.users.filter(function(i) {
        return filtredArr(i.id);
      });
    }

    function filtredArr(id) {
      return id === filterValue;
    }
  }

  applyFilterLang(filterValue) {
    let currentLang = filterValue.source.triggerValue;

    if (currentLang === 'Все языки') {
    //  this.dataSource = new MatTableDataSource(ELEMENT_DATA);
    //} else {
    //  const filtredLangArr = ELEMENT_DATA.filter(function(i) {
    //    return filtredArr(i.language);
    //  });
    //  this.dataSource = new MatTableDataSource(filtredLangArr);
    }

    function filtredArr(language) {
      return language === currentLang;
    }
  }

}
