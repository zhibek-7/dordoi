import { Component, OnInit} from '@angular/core';
import { TranslatedWordsReportRow } from "../../models/Reports/TranslatedWordsReportRow";
import { ReportService } from '../../services/reports.service';

@Component({
  selector: 'app-translated-words',
  templateUrl: './TranslatedWords.component.html',
  styleUrls: ['./TranslatedWords.component.css'],
  providers: [ReportService]
})

export class TranslatedWordsComponent implements OnInit {
  public reportrows: TranslatedWordsReportRow[];

  constructor(private reportService: ReportService) {
  }

  ngOnInit(): void {
    this.getRows();
  }

  async getRows() {
    this.reportrows = await this.reportService.getTranslatedWordsReport();
  }
}

