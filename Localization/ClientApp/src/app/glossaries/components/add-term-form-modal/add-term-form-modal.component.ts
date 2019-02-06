import { Component, OnInit, Output, EventEmitter, Input } from "@angular/core";

import { TranslationSubstring } from "src/app/models/database-entities/translationSubstring.type";
import { ModalComponent } from "src/app/shared/components/modal/modal.component";
import { PartOfSpeech } from "src/app/models/database-entities/partOfSpeech.type";
import { Term } from "src/app/models/Glossaries/term.type";
import { GlossariesService } from "src/app/services/glossaries.service";
import { Locale } from "src/app/models/database-entities/locale.type";
import { Glossary } from "src/app/models/database-entities/glossary.type";

@Component({
  selector: "app-add-term-form-modal",
  templateUrl: "./add-term-form-modal.component.html",
  styleUrls: ["./add-term-form-modal.component.css"]
})
export class AddTermFormComponent extends ModalComponent implements OnInit {
  newTerm: Term;

  @Input() glossary: Glossary;

  @Output() newTermSubmitted = new EventEmitter<Term>();

  constructor() {
    super();
    this.resetNewTermModel();
  }

  ngOnInit() {}

  show() {
    this.resetNewTermModel();
    super.show();
  }

  submitNewTerm() {
    this.newTerm.substring_To_Translate = this.newTerm.substring_To_Translate;
    this.newTerm.context = this.newTerm.substring_To_Translate;
    this.newTerm.position_In_Text = 0;
    this.hide();
    this.newTermSubmitted.emit(this.newTerm);
  }

  resetNewTermModel() {
    this.newTerm = new Term();
    this.newTerm.part_Of_Speech_Id = null;
    this.newTerm.is_Editable = true;
  }
}
