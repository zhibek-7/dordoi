import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { CreateProjectComponent } from '../../create-project.component';
import { ProjectsService } from '../../../services/projects.service';

@Component({
  selector: 'app-menu-project',
  templateUrl: './menu-project.component.html',
  styleUrls: ['./menu-project.component.css']
})
export class MenuProjectComponent extends CreateProjectComponent implements OnInit {



 @Output() newProjectSubmitted = new EventEmitter<any>();



  ngOnInit() {
  }


  show() {
    this.visible = true;
    this.resetNewProjectModel();
  }

  submitCreateProject() {
    //this.selectedItems = this.selectedItems.values;
    //this.newTerm.context = 'newContext';
    //this.newTerm.positionInText = 0;
    //this.hide();
    //this.newTermSubmitted.emit(this.newTerm);
  }

  resetNewProjectModel() {
    this.selectedItems = [];
    this.options.values = null;
    this.forms.values = null;
  }

 


}
