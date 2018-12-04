import { Component, OnInit, EventEmitter } from '@angular/core';

import { GlossariesService } from 'src/app/services/glossaries.service';
import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { String } from 'src/app/models/database-entities/string.type';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';
import { TermViewModel } from 'src/app/glossaries/models/term.viewmodel';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-glossaries',
  templateUrl: './glossaries.component.html',
  styleUrls: ['./glossaries.component.css']
})
export class GlossariesComponent implements OnInit {

  glossaries: Glossary[];

  selectedGlossary: Glossary;

  termViewModels = new Array<TermViewModel>();

  get selectedTerms(): String[] {
    return this.termViewModels
      .filter(termViewModel => termViewModel.isSelected)
      .map(termViewModel => termViewModel.term);
  }

  termSearchString: string;

  constructor(
    private glossariesService: GlossariesService,
    private requestDataReloadService: RequestDataReloadService)
  {
    this.requestDataReloadService.updateRequested.subscribe(() => this.loadTerms());
  }

  ngOnInit() {
    this.glossariesService.getGlossaries()
      .subscribe(
        glossaries => this.glossaries = glossaries,
        error => console.error(error));
  }

  public selectGlossary(glossary: Glossary): void {
    this.selectedGlossary = glossary;
    this.loadTerms();
  }

  loadTerms() {
    if (!this.selectedGlossary)
      return;

    this.glossariesService.getAssotiatedTerms(this.selectedGlossary.id, this.termSearchString)
      .subscribe(
        terms => this.termViewModels = terms.map(term => new TermViewModel(term, false)),
        error => console.log(error));
  }

  addNewTerm(newTerm: String) {
    if (!this.selectedGlossary)
      return;

    this.glossariesService.addNewTerm(this.selectedGlossary.id, newTerm)
      .subscribe(
        () => this.requestDataReloadService.requestUpdate(),
        error => console.log(error));
  }

  deleteSelectedTerms() {
    forkJoin(
      this.selectedTerms.map(term => this.glossariesService.deleteTerm(this.selectedGlossary.id, term.id)))
      .subscribe(
        () => this.requestDataReloadService.requestUpdate(),
        error => console.log(error));
  }

}
