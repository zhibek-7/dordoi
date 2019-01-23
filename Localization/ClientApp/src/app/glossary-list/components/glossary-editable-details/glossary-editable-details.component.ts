import { Component, OnInit, NgModule, Input, Output, EventEmitter } from '@angular/core';


import { GlossariesForEditing } from 'src/app/models/DTO/glossariesDTO.type';
import { GlossaryService } from 'src/app/services/glossary.service';

import { Locale } from 'src/app/models/database-entities/locale.type';
import { LanguageService } from 'src/app/services/languages.service';

import { localizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';
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

  availableLocalizationProjects: Selectable<localizationProjectForSelectDTO>[] = [];


  constructor(
    private glossariesService: GlossaryService,

    private projectsService: ProjectsService,
    private languageService: LanguageService,
  ) {
    super();
  }

  ngOnInit() {
  }

  load() {
    super.load();
    this.loadAvailableLanguages();
    this.loadAvailableLocalizationProjects();
  }

  //---------------- Locales
  loadAvailableLanguages() {
    this.glossariesService.getLocales()
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
  }

  //---------------- LocalizationProjects
  loadAvailableLocalizationProjects() {
    this.glossariesService.getlocalizationProjectForSelectDTO()
      .subscribe(localizationProject => {
        this.availableLocalizationProjects = localizationProject
          .map(localProject =>
            new Selectable<localizationProjectForSelectDTO>(
              localProject,
              this.glossary.localizationProjectsIds.some(selectedLocalizationProjectId => selectedLocalizationProjectId == localProject.id)
            ));
      },
        error => console.error(error));
  }

  toggleSelection(localizationProject: Selectable<localizationProjectForSelectDTO>) {
    localizationProject.isSelected = !localizationProject.isSelected;
    this.raiseSelectionChanged();
  }

  raiseSelectionChanged() {
    this.glossary.localizationProjectsIds = this.availableLocalizationProjects.filter(localizationProject => localizationProject.isSelected).map(selectable => selectable.model.id);
  }
}
