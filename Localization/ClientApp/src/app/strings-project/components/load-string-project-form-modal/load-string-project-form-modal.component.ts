import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";

import { ModalComponent } from "src/app/shared/components/modal/modal.component";

import { settingFileLoad, fileType } from "../../models/settingFileLoad";

import { Locale } from "src/app/models/database-entities/locale.type";
import { LanguageService } from "src/app/services/languages.service";

import { TranslationSubstringForEditingDTO } from "src/app/models/DTO/TranslationSubstringDTO.type";
import { ProjectsService } from "src/app/services/projects.service";

@Component({
  selector: 'app-load-string-project-form-modal',
  templateUrl: './load-string-project-form-modal.component.html',
  styleUrls: ['./load-string-project-form-modal.component.css']
})
export class LoadStringProjectFormModalComponent extends ModalComponent implements OnInit {

  @Input()
  translationSubstrings: TranslationSubstringForEditingDTO[];

  setting: settingFileLoad;
  types = [];
  availableLocales: Locale[] = [];

  @Output()
  loadSubmitted = new EventEmitter<settingFileLoad>();

  constructor(private languageService: LanguageService,
    private projectsService: ProjectsService) {
    super();
    this.resetModel();
    this.getFileTypes();
  }

  ngOnInit() {
  }

  show() {
    console.log("show load", this.translationSubstrings);

    this.loadAvailableLanguages();
    this.resetModel();

    super.show();
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
    let ids = this.translationSubstrings.map(t => t.id);
    this.languageService.getByIdsTranslationSubstring(this.projectsService.currentProjectId, ids).subscribe(
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

