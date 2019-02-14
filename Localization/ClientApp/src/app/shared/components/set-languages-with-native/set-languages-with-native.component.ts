import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { SetLanguagesComponent } from '../set-languages/set-languages.component';

import { Selectable } from 'src/app/shared/models/selectable.model';

import { Locale } from 'src/app/models/database-entities/locale.type';

@Component({
  selector: 'app-set-languages-with-native',
  templateUrl: './set-languages-with-native.component.html',
  styleUrls: ['./set-languages-with-native.component.css'], 
})
export class SetLanguagesWithNativeComponent extends SetLanguagesComponent implements OnInit {

  @Input() availableLocales: Selectable<Locale>[] = [];

  @Output() selectedLocalesWithNativeChanged = new EventEmitter<Locale[]>();

  availableLocalesNative: Selectable<Locale>[] = [];

  constructor() {
      super();
  }

  ngOnInit() {
    this.availableLocalesNative = this.availableLocales.filter(locale => locale.isSelected).map(local =>
      new Selectable<Locale>(
        local.model,
        local.model.isNative
      ));
  }

  setSelectedLocales(newSelection: Locale[]) {

    this.availableLocalesNative = newSelection.map(local =>
      new Selectable<Locale>(
        local,
        this.availableLocalesNative.some(localesNative => localesNative.model.id == local.id && localesNative.model.isNative)
      )).reverse();

    this.raiseSelectionChanged();
  }

  get selectedLocalesNative(): Locale[] {
    return this.availableLocalesNative.map(selectable => selectable.model);
  }

  toggleSelectionNative(locale: Selectable<Locale>) {
    locale.isSelected = !locale.isSelected;
    locale.model.isNative = !locale.model.isNative;
    this.raiseSelectionChanged();
  }

  raiseSelectionChanged() {
    this.selectedLocalesWithNativeChanged.emit(this.selectedLocalesNative);
  }

}
