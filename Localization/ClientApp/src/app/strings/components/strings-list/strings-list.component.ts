import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { TranslationSubstringService } from 'src/app/services/translationSubstring.service';
import { LanguageService } from 'src/app/services/languages.service';
import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
import { Selectable } from 'src/app/shared/models/selectable.model';
import { SortingArgs } from 'src/app/shared/models/sorting.args';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-strings-list',
  templateUrl: './strings-list.component.html',
  styleUrls: ['./strings-list.component.css']
})
export class StringsListComponent implements OnInit {

  @Input()
  selectableStrings: Selectable<TranslationSubstring>[];

  lastSortColumnName: string = '';

  isSortingAscending: boolean = true;

  @Output()
  sortingParamsChanged = new EventEmitter<SortingArgs>();

  @Output()
  dataReloadRequested = new EventEmitter();

  allSelectedCheckboxChecked: boolean = false;

  get isAllStringsOutdated(): boolean {
    return this.selectableStrings.every(selectableString => selectableString.model.outdated);
  }

  constructor(
    private translationSubstringService: TranslationSubstringService,
    private languageService: LanguageService,
  ) { }

  ngOnInit() {
  }

  requestSortBy(columnName: string) {
    if (columnName != this.lastSortColumnName) {
      this.isSortingAscending = true;
    }

    this.sortingParamsChanged.emit(new SortingArgs(columnName, this.isSortingAscending));
    this.dataReloadRequested.emit();

    this.lastSortColumnName = columnName;
    this.isSortingAscending = !this.isSortingAscending;
  }

  toggleSelectionForAllDisplayed() {
    let allSelected = this.selectableStrings.every(selectableString => selectableString.isSelected);

    if (this.allSelectedCheckboxChecked && allSelected)
      return;

    if (allSelected)
      for (let selectableString of this.selectableStrings) { selectableString.isSelected = false; }
    else
      for (let selectableString of this.selectableStrings) { selectableString.isSelected = true; }
  }

  toggleOutdatedForAllDisplayedStrings() {
    if (this.isAllStringsOutdated) {
      for (let selectableString of this.selectableStrings) { selectableString.model.outdated = false; }
    }
    else {
      for (let selectableString of this.selectableStrings) { selectableString.model.outdated = true; }
    }
    forkJoin(this.selectableStrings.map(selectableString =>
      this.translationSubstringService.updateTranslationSubstring(selectableString.model)))
      .subscribe(
        () => this.dataReloadRequested.emit(),
        error => console.log(error));
  }

}
