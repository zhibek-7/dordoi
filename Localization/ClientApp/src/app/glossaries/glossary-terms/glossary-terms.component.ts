import { Component, OnInit, Input } from '@angular/core';
import { String } from 'src/app/models/database-entities/string.type';
import { GlossariesService } from 'src/app/services/glossaries.service';
import { Glossary } from 'src/app/models/database-entities/glossary.type';

@Component({
  selector: 'app-glossary-terms',
  templateUrl: './glossary-terms.component.html',
  styleUrls: ['./glossary-terms.component.css']
})
export class GlossaryTermsComponent implements OnInit {

  private _glossary: Glossary;

  @Input()
  set glossary(glossary: Glossary) {
    this._glossary = glossary;
    this.loadTerms();
  }

  terms: String[];

  constructor(private glossariesService: GlossariesService) { }

  ngOnInit() {
  }

  loadTerms() {
    this.glossariesService.getAssotiatedTerms(this._glossary.id)
      .subscribe(
        terms => this.terms = terms,
        error => console.log(error));
  }

}
