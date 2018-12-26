import { Component, OnInit, EventEmitter } from '@angular/core';
import { forkJoin } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

import { GlossariesService } from 'src/app/services/glossaries.service';
import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';
import { TermViewModel } from 'src/app/glossaries/models/term.viewmodel';
import { SortingArgs } from 'src/app/shared/models/sorting.args';
import { PartsOfSpeechService } from 'src/app/services/partsOfSpeech.service';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';
import { Term } from 'src/app/models/Glossaries/term.type';

@Component({
  selector: 'app-glossary-details',
  templateUrl: './glossary-details.component.html',
  styleUrls: ['./glossary-details.component.css']
})
export class GlossaryDetailsComponent implements OnInit {

  private _glossary: Glossary;

  pageSize: number = 10;

  currentOffset: number = 0;

  totalCount: number;

  sortByColumnName: string;

  ascending: boolean = true;

  set glossary(value) {
    this._glossary = value;
    this.loadTerms();
  }
  get glossary() { return this._glossary; }

  termViewModels = new Array<TermViewModel>();

  get selectedTerms(): Term[] {
    return this.termViewModels
      .filter(termViewModel => termViewModel.isSelected)
      .map(termViewModel => termViewModel.term);
  }

  termSearchString: string;

  constructor(
    private route: ActivatedRoute,
    private glossariesService: GlossariesService,
    private requestDataReloadService: RequestDataReloadService
  ) {
    this.requestDataReloadService.updateRequested.subscribe(() => this.loadTerms(this.currentOffset));
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      let glossaryId = +params['id'];
      this.glossariesService.get(glossaryId)
        .subscribe(
          glossary => this.glossary = glossary);
    });
  }

  loadTerms(offset = 0) {
    if (!this.glossary)
      return;

    let sortByColumns = []
    if (this.sortByColumnName) {
      sortByColumns.push(this.sortByColumnName);
    }

    this.glossariesService.getAssotiatedTerms(this.glossary.id, this.termSearchString, this.pageSize, offset, sortByColumns, this.ascending)
      .subscribe(
        response => {
          let terms = response.body;
          this.termViewModels = terms.map(term => new TermViewModel(term, false));
          let totalCount = +response.headers.get('totalCount');
          this.totalCount = totalCount;
          this.currentOffset = offset;
        });
  }

  addNewTerm(newTerm: Term) {
    if (!this.glossary)
      return;

    this.glossariesService.addNewTerm(this.glossary.id, newTerm, newTerm.partOfSpeechId)
      .subscribe(
        () => this.requestDataReloadService.requestUpdate());
  }

  deleteSelectedTerms() {
    forkJoin(
      this.selectedTerms.map(term => this.glossariesService.deleteTerm(this.glossary.id, term.id)))
      .subscribe(
        () => this.requestDataReloadService.requestUpdate());
  }

  onPageChanged(newOffset: number) {
    this.loadTerms(newOffset);
  }

  sortByRequested(sortingArgs: SortingArgs) {
    this.sortByColumnName = sortingArgs.columnName;
    this.ascending = sortingArgs.isAscending;
    this.loadTerms();
  }

}
