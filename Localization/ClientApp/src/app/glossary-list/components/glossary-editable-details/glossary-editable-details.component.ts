import { Component, OnInit, NgModule, Input, Output, EventEmitter } from '@angular/core';

import { GlossaryService } from 'src/app/services/glossary.service';

import { GlossariesForEditing } from 'src/app/models/DTO/glossariesDTO.type';

import { LanguageService } from 'src/app/services/languages.service';
import { ProjectsService } from 'src/app/services/projects.service';

import { Selectable } from 'src/app/shared/models/selectable.model';

import { Locale } from 'src/app/models/database-entities/locale.type';
import { localizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';

@Component({
  selector: 'app-glossary-editable-details',
  templateUrl: './glossary-editable-details.component.html',
  styleUrls: ['./glossary-editable-details.component.css']
})
export class GlossaryEditableDetailsComponent implements OnInit
{
  @Input()
  newGlossary: GlossariesForEditing; //переименовать в glossary

  availableLocales: Selectable<Locale>[] = [];
  selectedLocales: Locale[] = [];
  
  availableLocalizationProjects: Selectable<localizationProjectForSelectDTO>[] = [];
  selectedLocalizationProjects: localizationProjectForSelectDTO[] = [];


  constructor(
    private glossariesService: GlossaryService,

    private projectsService: ProjectsService,
    private languageService: LanguageService,
  ) { }

  ngOnInit() {
    this.loadAvailableLanguages();
    this.loadAvailableLocalizationProjects();
  }

  //---------------- Locales
  loadAvailableLanguages()
  {
    this.glossariesService.getLocales()
      .subscribe(locale =>
      {
        this.selectedLocales = locale;
        this.availableLocales = locale
          .map(local =>
            new Selectable<Locale>(
              local,
              false//this.selectedLocales.some(selectedLocale => selectedLocale.id == local.id)
            ));
      },
      error => console.error(error));
  }

  setSelectedLocales(newSelection: Locale[]) {
    this.newGlossary.locales = newSelection;
  }

  //---------------- LocalizationProjects
  loadAvailableLocalizationProjects()
  {
    this.glossariesService.getlocalizationProjectForSelectDTO()
      .subscribe(localizationProject =>
      {
        this.selectedLocalizationProjects = localizationProject;
        this.availableLocalizationProjects = localizationProject
          .map(localProject =>
            new Selectable<localizationProjectForSelectDTO>(
              localProject,
              false//this.selectedLocalizationProjects.some(selectedLocalizationProject => selectedLocalizationProject.id == localProject.id)
            ));
      },
    error => console.error(error));
  }
  
  toggleSelection(localizationProject: Selectable<localizationProjectForSelectDTO>) {
    localizationProject.isSelected = !localizationProject.isSelected;
    this.raiseSelectionChanged();
  }

  raiseSelectionChanged() {
    this.newGlossary.localizationProjects = this.availableLocalizationProjects.filter(localizationProject => localizationProject.isSelected).map(selectable => selectable.model);
  }
}
