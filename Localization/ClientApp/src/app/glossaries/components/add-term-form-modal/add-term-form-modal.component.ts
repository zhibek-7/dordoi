import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';
import { Term } from 'src/app/models/Glossaries/term.type';
import { GlossariesService } from 'src/app/services/glossaries.service';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { Glossary } from 'src/app/models/database-entities/glossary.type';

@Component({
  selector: 'app-add-term-form-modal',
  templateUrl: './add-term-form-modal.component.html',
  styleUrls: ['./add-term-form-modal.component.css']
})
export class AddTermFormComponent extends ModalComponent implements OnInit {

  newTerm: Term;

  @Input() glossary: Glossary;

  @Output() newTermSubmitted = new EventEmitter<Term>();

  constructor() {
    super();
    this.resetNewTermModel();
  }

  ngOnInit() {
  }

  show() {
    this.resetNewTermModel();
    super.show();
  }

  submitNewTerm() {
    this.newTerm.substringToTranslate = this.newTerm.value;
    this.newTerm.context = 'newContext';
    this.newTerm.positionInText = 0;
    this.hide();
    this.newTermSubmitted.emit(this.newTerm);
  }

  resetNewTermModel() {
    this.newTerm = new Term();
    this.newTerm.partOfSpeechId = null;
  }

}
