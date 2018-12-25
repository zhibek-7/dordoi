import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
import { GlossariesService } from 'src/app/services/glossaries.service';
import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';
import { TermViewModel } from 'src/app/glossaries/models/term.viewmodel';
import { SortingArgs } from 'src/app/shared/models/sorting.args';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';
import { Term } from 'src/app/models/Glossaries/term.type';

@Component({
  selector: 'app-glossary-terms',
  templateUrl: './glossary-terms.component.html',
  styleUrls: ['./glossary-terms.component.css']
})
export class GlossaryTermsComponent implements OnInit {

  @Input() glossary: Glossary;

  _termViewModels = new Array<TermViewModel>();

  @Input()
  set termViewModels(value) {
    this._termViewModels = value;
    this.allDispayedTermsSelectedCheckboxChecked = this.termViewModels.every(termViewModel => termViewModel.isSelected);
  }

  get termViewModels() {
    return this._termViewModels;
  }

  allDispayedTermsSelectedCheckboxChecked = false;

  isSortingAscending = true;

  @Output() sortByRequested = new EventEmitter<SortingArgs>();

  lastSortColumnName = '';

  @Input() partsOfSpeech: PartOfSpeech[];

  constructor(
    private glossariesService: GlossariesService,
    private requestDataReloadService: RequestDataReloadService
  ) { }

  ngOnInit() { }

  deleteTerm(term: Term) {
    this.glossariesService.deleteTerm(this.glossary.id, term.id)
      .subscribe(
        () => this.requestDataReloadService.requestUpdate());
  }

  toggleSelectionForAllDisplayedTerms() {
    let allTermsSelected = this.termViewModels.every(termViewModel => termViewModel.isSelected);

    if (this.allDispayedTermsSelectedCheckboxChecked && allTermsSelected)
      return;

    if (allTermsSelected)
      for (let termViewModel of this.termViewModels) { termViewModel.isSelected = false; }
    else
      for (let termViewModel of this.termViewModels) { termViewModel.isSelected = true; }
  }

  requestSortBy(columnName: string) {
    if (columnName != this.lastSortColumnName) {
      this.isSortingAscending = true;
    }
    this.sortByRequested.emit(new SortingArgs(columnName, this.isSortingAscending));
    this.lastSortColumnName = columnName;
    this.isSortingAscending = !this.isSortingAscending;
  }

}
