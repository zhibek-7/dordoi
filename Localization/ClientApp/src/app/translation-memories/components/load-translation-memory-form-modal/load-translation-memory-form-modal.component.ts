import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";

import { ModalComponent } from "src/app/shared/components/modal/modal.component";

import { settingFileLoad, fileType } from "../../models/settingFileLoad";

import { Locale } from "src/app/models/database-entities/locale.type";
import { LanguageService } from "src/app/services/languages.service";
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-load-translation-memory-form-modal',
  templateUrl: './load-translation-memory-form-modal.component.html',
  styleUrls: ['./load-translation-memory-form-modal.component.css']
})
export class LoadTranslationMemoryFormModalComponent extends ModalComponent implements OnInit {

  @Input()
  id: Guid;

  setting: settingFileLoad;
  types = [];
  availableLocales: Locale[] = [];

  @Output()
  loadSubmitted = new EventEmitter<settingFileLoad>();

  constructor(private languageService: LanguageService) {
    super();
    this.resetModel();
    this.getFileTypes();
  }

  ngOnInit() {
  }

  show() {
    super.show();
    this.loadAvailableLanguages();
    this.resetModel();
  }

  submit() {
    this.hide();

    if (this.setting.isAllLocale) {
      this.setting.localesIds = this.availableLocales.map(t => t.id);
      this.setting.baseLocaleId = null;
      this.setting.translationLocaleId = null;
    }

    this.loadSubmitted.emit(this.setting);
  }

  getFileTypes() {
    for (var enumMember in fileType) {
      if (!isNaN(parseInt(enumMember, 10))) {
        this.types.push({ key: enumMember, value: fileType[enumMember] });
      }
    }
  }

  loadAvailableLanguages() {
    this.languageService.getByTranslationMemory(this.id).subscribe(
      locale => this.availableLocales = locale,
      error => console.error(error)
    );
  }

  resetModel() {
    this.setting = new settingFileLoad();
    this.setting.typeFile = null;
    this.setting.isAllLocale = null;
    this.setting.localesIds = [];
    this.setting.baseLocaleId = null;
    this.setting.translationLocaleId = null;
  }

}
