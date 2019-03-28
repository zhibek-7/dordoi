import { Component, OnInit, EventEmitter } from "@angular/core";
import { forkJoin } from "rxjs";
import { ActivatedRoute } from "@angular/router";
import { MatDialog } from "@angular/material";

import { SetProjectsModalComponent } from "src/app/glossaries/components/set-projects-modal/set-projects-modal.component";

import { GlossariesService } from "src/app/services/glossaries.service";
import { GlossaryService } from "src/app/services/glossary.service";

import { RequestDataReloadService } from "src/app/glossaries/services/requestDataReload.service";

import { Glossary } from "src/app/models/database-entities/glossary.type";
import { TermViewModel } from "src/app/glossaries/models/term.viewmodel";
import { SortingArgs } from "src/app/shared/models/sorting.args";
import { Term } from "src/app/models/Glossaries/term.type";
import { GlossariesForEditing } from "src/app/models/DTO/glossariesDTO.type";
import { Guid } from 'guid-typescript';

@Component({
  selector: "app-glossary-details",
  templateUrl: "./glossary-details.component.html",
  styleUrls: ["./glossary-details.component.css"]
})
export class GlossaryDetailsComponent implements OnInit {

  glossaries: Glossary[];

  private _glossary: Glossary = null;

  pageSize: number = 10;

  currentOffset: number = 0;

  totalCount: number;

  sortByColumnName: string;

  ascending: boolean = true;

  set glossary(value) {
    this._glossary = value;
    this.loadTerms();
  }
  get glossary() {
    return this._glossary;
  }

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
    private glossaryService: GlossaryService,
    private requestDataReloadService: RequestDataReloadService,
    public dialog: MatDialog
  ) {
    this.requestDataReloadService.updateRequested.subscribe(() =>
      this.loadTerms(this.currentOffset)
    );
  }

  ngOnInit() {
    this.loadGlossaries();
    this.loadTerms();
  }

  loadGlossaries() {
    this.glossariesService.getGlossaries()
      .subscribe(
        glossaries => {
          this.glossaries = glossaries;
          if (this.glossaries != null && this.glossaries.length > 0)
            this.glossary = this.glossaries[0];
        });
  }

  loadTerms(offset = 0) {
    if (!this.glossary) return;

    let sortByColumns = [];
    if (this.sortByColumnName) {
      sortByColumns.push(this.sortByColumnName);
    }

    this.glossariesService
      .getAssotiatedTerms(
        this.glossary.id,
        this.termSearchString,
        this.pageSize,
        offset,
        sortByColumns,
        this.ascending
      )
      .subscribe(response => {
        let terms = response.body;
        this.termViewModels = terms.map(term => new TermViewModel(term, false));
        let totalCount = +response.headers.get("totalCount");
        this.totalCount = totalCount;
        this.currentOffset = offset;
      });
  }

  addNewTerm(newTerm: Term) {
    if (!this.glossary) return;

    this.glossariesService
      .addNewTerm(this.glossary.id, newTerm, newTerm.part_of_speech_id)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
  }

  deleteSelectedTerms() {
    forkJoin(
      this.selectedTerms.map(term =>
        this.glossariesService.deleteTerm(this.glossary.id, term.id)
      )
    ).subscribe(() => this.requestDataReloadService.requestUpdate());
  }

  onPageChanged(newOffset: number) {
    this.loadTerms(newOffset);
  }

  sortByRequested(sortingArgs: SortingArgs) {
    this.sortByColumnName = sortingArgs.columnName;
    this.ascending = sortingArgs.isAscending;
    this.loadTerms();
  }

  openSetProjectsModal() {
    const dialogRef = this.dialog.open(SetProjectsModalComponent, {
      data: { glossary: this._glossary }
    });
    dialogRef.afterClosed().subscribe((result: GlossariesForEditing) => {
      if (result) {
        this.glossaryService
          .editSaveGlossary(result)
          .subscribe();
      }
    });
  }
}
