import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";

import { TranslationSubstringService } from "src/app/services/translationSubstring.service";
//import { LanguageService } from 'src/app/services/languages.service';
import { TranslationSubstring } from "src/app/models/database-entities/translationSubstring.type";
import { SortingArgs } from "src/app/shared/models/sorting.args";
import { forkJoin } from "rxjs";

@Component({
  selector: "app-strings-list",
  templateUrl: "./strings-list.component.html",
  styleUrls: ["./strings-list.component.css"]
})
export class StringsListComponent implements OnInit {
  @Input()
  translationStrings: TranslationSubstring[];
  lastSortColumnName: string = "";
  isSortingAscending: boolean = true;

  @Output()
  sortingParamsChanged = new EventEmitter<SortingArgs>();

  @Output()
  dataReloadRequested = new EventEmitter();

  get isAllStringsOutdated(): boolean {
    return this.translationStrings.every(
      translationString => translationString.outdated
    );
  }

  constructor(
    private translationSubstringService: TranslationSubstringService
  ) {}

  ngOnInit() {}

  requestSortBy(columnName: string) {
    if (columnName != this.lastSortColumnName) {
      this.isSortingAscending = true;
    }

    this.sortingParamsChanged.emit(
      new SortingArgs(columnName, this.isSortingAscending)
    );
    this.dataReloadRequested.emit();

    this.lastSortColumnName = columnName;
    this.isSortingAscending = !this.isSortingAscending;
  }

  toggleOutdatedForAllDisplayedStrings() {
    if (this.isAllStringsOutdated) {
      for (let translationString of this.translationStrings) {
        translationString.outdated = false;
      }
    } else {
      for (let translationString of this.translationStrings) {
        translationString.outdated = true;
      }
    }
    forkJoin(
      this.translationStrings.map(translationString =>
        this.translationSubstringService.updateTranslationSubstring(
          translationString
        )
      )
    ).subscribe(
      () => this.dataReloadRequested.emit(),
      error => console.log(error)
    );
  }

  updateString(updatedTranslationSubsrtring: TranslationSubstring) {
    this.translationSubstringService
      .updateTranslationSubstring(updatedTranslationSubsrtring)
      .subscribe(
        () => this.dataReloadRequested.emit(),
        error => console.log(error)
      );
  }
}
