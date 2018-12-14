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
  selector: 'app-set-languages',
  templateUrl: './set-languages.component.html',
  styleUrls: ['./set-languages.component.css']
})
export class SetLanguagesComponent extends ModalComponent implements OnInit {
  args = 'ascending';
  searchText: string = '';
  reverse=false;

  @Input() availableLocales: Selectable<Locale>[] = [];

  @Output() selectedLocalesChanged = new EventEmitter<Locale[]>();

  get isNoneSelected() {
    return this.availableLocales.every(locale => !locale.isSelected);
  }

  get selectedLocales(): Locale[] {
    return this.availableLocales.filter(locale => locale.isSelected).map(selectable => selectable.model);
  }

  ngOnInit() {
  }

  selectAll() {
    this.availableLocales.forEach(locale => locale.isSelected = true);
    this.raiseSelectionChanged();
  }

  toggleSelection(locale: Selectable<Locale>) {
    locale.isSelected = !locale.isSelected;
    this.raiseSelectionChanged();
  }

  raiseSelectionChanged() {
    this.selectedLocalesChanged.emit(this.selectedLocales);
  }

}
