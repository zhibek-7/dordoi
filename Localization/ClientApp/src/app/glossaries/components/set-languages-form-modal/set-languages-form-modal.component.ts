import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';
import { SetLanguagesComponent } from 'src/app/glossaries/components/set-languages/set-languages.component';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';
import { Term } from 'src/app/models/Glossaries/term.type';
import { LanguageService } from 'src/app/services/languages.service';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { GlossariesService } from 'src/app/services/glossaries.service';
import { Selectable } from 'src/app/glossaries/models/selectable.model';

@Component({
  selector: 'app-set-languages-form-modal',
  templateUrl: './set-languages-form-modal.component.html',
  styleUrls: ['./set-languages-form-modal.component.css']
})
export class SetLanguagesFormModalComponent extends ModalComponent implements OnInit {

  @Input() glossary: Glossary;

  @Input() term: Term;

  availableLocales: Selectable<Locale>[] = [];

  selectedLocales: Locale[] = [];

  constructor(
    private languageService: LanguageService,
    private glossariesService: GlossariesService
  ) { super(); }

  loadAvailableLanguages() {
    this.glossariesService.getTranslationLocalesForTerm(this.glossary.id, this.term.id)
      .subscribe(localesForCurrentTerm =>
        this.glossariesService.getGlossaryLocale(this.glossary.id)
          .subscribe(currentGlossaryLocale =>
            this.languageService.getLanguageList()
              .subscribe(allLocales => {
                  this.selectedLocales = localesForCurrentTerm;
                  this.availableLocales = allLocales
                    .filter(locale => locale.id != currentGlossaryLocale.id)
                    .map(locale =>
                      new Selectable<Locale>(
                        locale,
                        this.selectedLocales.some(currentTermLocale => currentTermLocale.id == locale.id)));
                },
                error => console.log(error)),
            error => console.log(error)),
        error => console.log(error));
  }

  setSelectedLocales(newSelection: Locale[]) {
    this.selectedLocales = newSelection;
  }

  show() {
    this.loadAvailableLanguages();
    super.show();
  }

  applyChanges() {
    this.glossariesService.setTranslationLocalesForTerm(this.glossary.id, this.term.id, this.selectedLocales.map(locale => locale.id))
      .subscribe(null,
        error => console.log(error));
    this.hide();
  }

}
