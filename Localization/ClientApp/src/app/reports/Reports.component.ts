import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Project } from '../models/Project';

import { ProjectsService } from '../services/projects.service';

@Component({
  selector: 'app-reports',
  templateUrl: './Reports.component.html',
  styleUrls: ['./Reports.component.css'],
})

export class ReportsComponent implements OnInit {

  public projectId: number;
  public project: Project;

  showProjectStatus: boolean = false;
  showCostSumm: boolean = false;
  showTranslationCost: boolean = false;
  showTranslatedWords: boolean = false;
  showFoulsTranslation: boolean = false;

  constructor(private route: ActivatedRoute, private location: Location, private projectsService: ProjectsService) { }

  ngOnInit() {
    
      this.projectsService.getProject(this.route.snapshot.params['id'])
      .subscribe(project => {
          this.project = project;
        this.projectId = project.id;
        console.log(this.projectId);
          },
      error => console.error(error));

  }

  public showReport(type: Number) {
    this.showProjectStatus    = (type === 1);
    this.showCostSumm         = (type === 2);
    this.showTranslationCost  = (type === 3);
    this.showTranslatedWords  = (type === 4);
    this.showFoulsTranslation = (type === 5);
  }

  goBack(): void {
    this.location.back();
  }
}

