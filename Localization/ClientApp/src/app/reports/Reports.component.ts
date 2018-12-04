import { Component } from '@angular/core';

@Component({
  selector: 'app-reports',
  templateUrl: './Reports.component.html',
  styleUrls: ['./Reports.component.css'],
})

export class ReportsComponent {

  showProjectState: boolean = false;
  showCostSumm: boolean = false;
  showTranslationCost: boolean = false;
  showTranslatedWords: boolean = false;
  showViolations: boolean = false;

  constructor() { }

  public showReport(type: Number) {
    this.showTranslatedWords = (type == 4);
  }

}

