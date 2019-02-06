import { Component, OnInit, NgModule, Input, Output, EventEmitter } from '@angular/core';


import { GlossariesForEditing } from 'src/app/models/DTO/glossariesDTO.type';
import { GlossaryService } from 'src/app/services/glossary.service';

import { Locale } from 'src/app/models/database-entities/locale.type';
import { LanguageService } from 'src/app/services/languages.service';

import { LocalizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';
import { ProjectsService } from 'src/app/services/projects.service';

import { Selectable } from 'src/app/shared/models/selectable.model';

import { LoadOnRequestBase } from 'src/app/shared/models/load-on-request-base.model';

@Component({
  selector: 'app-glossary-editable-details',
  templateUrl: './glossary-editable-details.component.html',
  styleUrls: ['./glossary-editable-details.component.css']
})
export class GlossaryEditableDetailsComponent extends LoadOnRequestBase implements OnInit {
  @Input()
  glossary: GlossariesForEditing;

  availableLocales: Selectable<Locale>[] = [];

  availableLocalizationProjects: Selectable<LocalizationProjectForSelectDTO>[] = [];

  errorsRequiredLocales: boolean = false;
  errorsRequiredLocalizationProjects: boolean = false;

  constructor(
    private projectsService: ProjectsService,
    private languageService: LanguageService,
  )
  {
    super();
  }

  ngOnInit()
  {
  }

  load() {
    super.load();
    this.loadAvailableLanguages();
    this.loadAvailableLocalizationProjects();
  }

  //#region Работа с Locales
  
  loadAvailableLanguages() {
    this.languageService.getLanguageList()
      .subscribe(locale => {
        this.availableLocales = locale
          .map(local =>
            new Selectable<Locale>(
              local,
              this.glossary.localesIds.some(selectedLocaleId => selectedLocaleId == local.id)
            ));
      },
        error => console.error(error));
  }
  
  setSelectedLocales(newSelection: Locale[]) {
    this.glossary.localesIds = newSelection.map(t => t.id);
    //Валидация. Обязательно должно быть выбрано хотя бы одно значение.
    this.errorsRequiredLocales = this.glossary.localesIds.length == 0;
  }

  //#endregion

  //#region Работа с LocalizationProjects

  loadAvailableLocalizationProjects() {
    this.projectsService.getLocalizationProjectForSelectDTO()
      .subscribe(localizationProject => {
        this.availableLocalizationProjects = localizationProject
          .map(localProject =>
            new Selectable<LocalizationProjectForSelectDTO>(
              localProject,
              this.glossary.localizationProjectsIds.some(selectedLocalizationProjectId => selectedLocalizationProjectId == localProject.id)
            ));
      },
        error => console.error(error));
  }

  toggleSelection(localizationProject: Selectable<LocalizationProjectForSelectDTO>) {
    localizationProject.isSelected = !localizationProject.isSelected;
    this.raiseSelectionChanged();
  }

  raiseSelectionChanged() {
    this.glossary.localizationProjectsIds = this.availableLocalizationProjects.filter(localizationProject => localizationProject.isSelected).map(selectable => selectable.model.id);
    //Валидация. Обязательно должно быть выбрано хотя бы одно значение.
    this.errorsRequiredLocalizationProjects = this.glossary.localizationProjectsIds.length == 0;
  }

  //#endregion
}
