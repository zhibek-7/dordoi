import { Component } from '@angular/core';

@Component({
  selector: 'app-reports',
  templateUrl: './Reports.component.html',
  styleUrls: ['./Reports.component.css'],
})

export class ReportsComponent {

  showProjectStatus: boolean = false;
  showCostSumm: boolean = false;
  showTranslationCost: boolean = false;
  showTranslatedWords: boolean = false;
  showFoulsTranslation: boolean = false;

  constructor() { }

  public showReport(type: Number) {
    this.showProjectStatus    = (type === 1);
    this.showCostSumm         = (type === 2);
    this.showTranslationCost  = (type === 3);
    this.showTranslatedWords  = (type === 4);
    this.showFoulsTranslation = (type === 5);
  }

}

