import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { String } from 'src/app/models/database-entities/string.type';
import { GlossariesService } from 'src/app/services/glossaries.service';
import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';
import { TermViewModel } from 'src/app/glossaries/models/term.viewmodel';

@Component({
  selector: 'app-glossary-terms',
  templateUrl: './glossary-terms.component.html',
  styleUrls: ['./glossary-terms.component.css']
})
export class GlossaryTermsComponent implements OnInit {

  @Input() glossary: Glossary;

  @Input() termViewModels = new Array<TermViewModel>();

  constructor(
    private glossariesService: GlossariesService,
    private requestDataReloadService: RequestDataReloadService
  ) { }

  ngOnInit() { }

  deleteTerm(term: String) {
    this.glossariesService.deleteTerm(this.glossary.id, term.id)
      .subscribe(
        () => this.requestDataReloadService.requestUpdate(),
        error => console.log(error));
  }

  updateTerm(updatedTerm: String) {
    if (!this.glossary)
      return;

    this.glossariesService.updateTerm(this.glossary.id, updatedTerm)
      .subscribe(
        () => this.requestDataReloadService.requestUpdate(),
        error => console.log(error));
  }

  toggleSelectionForAllDisplayedTerms(args) {
    let allTermsSelected = this.termViewModels.every(termViewModel => termViewModel.isSelected);

    if (args.target.checked && allTermsSelected)
      return;

    if (allTermsSelected)
      for (let termViewModel of this.termViewModels) { termViewModel.isSelected = false; }
    else
      for (let termViewModel of this.termViewModels) { termViewModel.isSelected = true; }
  }

}
