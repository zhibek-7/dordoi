import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';
import { Term } from 'src/app/models/Glossaries/term.type';
import { GlossariesService } from 'src/app/services/glossaries.service';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { PartsOfSpeechService } from 'src/app/services/partsOfSpeech.service';
import { LoadOnRequestBase } from 'src/app/shared/models/load-on-request-base.model';

@Component({
  selector: 'app-term-details-editable',
  templateUrl: './term-details-editable.component.html',
  styleUrls: ['./term-details-editable.component.css']
})
export class TermDetailsEditableComponent extends LoadOnRequestBase implements OnInit {

  @Input() term: Term;

  @Input() glossary: Glossary;

  glossaryLocale: Locale;

  partsOfSpeech: PartOfSpeech[] = [];

  constructor(
    private glossariesService: GlossariesService,
    private partsOfSpeechService: PartsOfSpeechService,
  ) { super(); }

  ngOnInit() {
  }

  load() {
    super.load();
    this.loadGlossaryLocale();
    this.loadPartsOfSpeech();
  }

  loadGlossaryLocale() {
    this.glossariesService.getGlossaryLocale(this.glossary.id)
      .subscribe(
        glossaryLocale => this.glossaryLocale = glossaryLocale,
        error => console.log(error)
      );
  }

  loadPartsOfSpeech() {
    this.partsOfSpeechService.getListByGlossaryId(this.glossary.id)
      .subscribe(
        partsOfSpeech => {
          this.partsOfSpeech = partsOfSpeech;
        },
        error => console.log(error)
      );
  }

}
