import { Component, OnInit, NgModule, Input, Output, EventEmitter } from '@angular/core';

//import { SetLanguagesComponent } from 'src/app/shared/components/set-languages/set-languages.component';

import { GlossaryService } from 'src/app/services/glossary.service';

import { Glossaries } from 'src/app/models/DTO/glossaries.type';

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
  newGlossary: Glossaries;

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
      .subscribe(locale => //this.availableLocalizationProjects = localizationProject,

      {
        this.selectedLocales = locale;
        this.availableLocales = locale
          .map(local =>
            new Selectable<Locale>(
              local,
              false//this.selectedLocalizationProjects.some(selectedLocalizationProjects => selectedLocalizationProjects.id == localProject.id)
            ));
      },

      error => console.error(error));


  //  this.translationSubstringService.getTranslationLocalesForString(this.translationSubstring.id)
  //    .subscribe(localesForTranslationSubstring =>
  //      this.projectsService.getProject(this.projectsService.currentProjectId)
  //        .subscribe(project =>
  //          this.languageService.getLanguageList()
  //            .subscribe(allLocales => {
  //              this.selectedLocales = localesForTranslationSubstring;
  //              this.availableLocales = allLocales
  //                .filter(locale => locale.id != project.ID_SourceLocale)
  //                .map(locale =>
  //                  new Selectable<Locale>(
  //                    locale,
  //                    this.selectedLocales.some(selectedLocale => selectedLocale.id == locale.id)));
  //            },
  //              error => console.log(error)),
  //          error => console.log(error)),
  //      error => console.log(error));
  //}

  //loadAvailableLanguages() {
  //  this.glossariesService.getTranslationLocalesForTerm(this.glossary.id, this.term.id)
  //    .subscribe(localesForCurrentTerm =>
  //      this.glossariesService.getGlossaryLocale(this.glossary.id)
  //        .subscribe(currentGlossaryLocale =>
  //          this.languageService.getLanguageList()
  //            .subscribe(allLocales => {
  //              this.selectedLocales = localesForCurrentTerm;
  //              this.availableLocales = allLocales
  //                .filter(locale => locale.id != currentGlossaryLocale.id)
  //                .map(locale =>
  //                  new Selectable<Locale>(
  //                    locale,
  //                    this.selectedLocales.some(currentTermLocale => currentTermLocale.id == locale.id)));
  //            },
  //              error => console.log(error))));
  }

  setSelectedLocales(newSelection: Locale[]) {
    this.newGlossary.locales = newSelection; //this.selectedLocales = newSelection;
  }

  //---------------- LocalizationProjects
  loadAvailableLocalizationProjects()
  {
    this.glossariesService.getlocalizationProjectForSelectDTO()
      .subscribe(localizationProject => //this.availableLocalizationProjects = localizationProject,

      {
        this.selectedLocalizationProjects = localizationProject;
        this.availableLocalizationProjects = localizationProject
          .map(localProject =>
            new Selectable<localizationProjectForSelectDTO>(
              localProject,
              false//this.selectedLocalizationProjects.some(selectedLocalizationProjects => selectedLocalizationProjects.id == localProject.id)
            ));
      },

    error => console.error(error));
  }

  //get selectedLocalizationProjects(): localizationProjectForSelectDTO[] {
  //  return this.availableLocalizationProjects.filter(localizationProject => localizationProject.isSelected).map(selectable => selectable.model);
  //}

  toggleSelection(localizationProject: Selectable<localizationProjectForSelectDTO>) {
    localizationProject.isSelected = !localizationProject.isSelected;
    this.raiseSelectionChanged();
  }

  raiseSelectionChanged() {
    //this.selectedLocalizationProjectsChanged.emit(this.selectedLocalizationProjects);
    this.newGlossary.localizationProjects = this.availableLocalizationProjects.filter(localizationProject => localizationProject.isSelected).map(selectable => selectable.model);
  }
}
