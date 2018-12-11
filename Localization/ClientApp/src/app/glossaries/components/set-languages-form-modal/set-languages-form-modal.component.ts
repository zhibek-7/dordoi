import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { String } from 'src/app/models/database-entities/string.type';
import { ModalComponent } from 'src/app/glossaries/components/modal/modal.component';
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
  args = 'ascending';
  searchText: string = '';

  @Input() glossary: Glossary;

  @Input() term: String;

  availableLocales: Selectable<Locale>[] = [];

  get isNoneSelected() {
    return this.availableLocales.every(locale => !locale.isSelected);
  }

  get selectedLocales(): Array<Locale> {
    return this.availableLocales.filter(locale => locale.isSelected).map(selectable => selectable.model);
  }

  constructor(
    private languageService: LanguageService,
    private glossariesService: GlossariesService
  ) { super(); }

  ngOnInit() {
  }

  selectAll() {
    this.availableLocales.forEach(locale => locale.isSelected = true);
  }

  show() {
    this.loadAvailableLanguages();
    super.show();
  }

  loadAvailableLanguages() {
    this.glossariesService.getGlossaryLocale(this.glossary.id)
      .subscribe(currentGlossaryLocale =>
        this.languageService.getLanguageList()
          .subscribe(locales => {
            this.availableLocales = locales
              .filter(locale => locale.id != currentGlossaryLocale.id)
              .map(locale => new Selectable<Locale>(locale, false));
          }));
  }

  applyChanges() {
    this.hide();
  }

}
