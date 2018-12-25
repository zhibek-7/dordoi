import { Component, OnInit} from '@angular/core';


import { ProjectsService } from '../../../services/projects.service';
import { Project } from '../../../models/Project';


import { FormControl} from '@angular/forms';



@Component({
  selector: 'app-all-settings',
  templateUrl: './all-settings.component.html',
  styleUrls: ['./all-settings.component.css']
})
export class AllSettingsComponent implements OnInit {
  args = 'ascending';
  reverse = false;
  constructor(private projectsService: ProjectsService) { }
  currentProjectName = '';

  pjName = new FormControl();



  ngOnInit() {
    this.currentProjectName = sessionStorage.getItem('ProjectName');
    console.log('ProjectName=' + sessionStorage.getItem('ProjectName'));
    console.log('ProjecID=' + sessionStorage.getItem('ProjecID'));

  }


  addProject() {

    console.log("!!!="+this.pjName.value);

    let newProject: Project = new Project(this.pjName.value, this.pjName.value, this.pjName.value);        // поменять на id реального пользователя, когда появится
    this.projectsService.addProject(newProject);

  }


}
