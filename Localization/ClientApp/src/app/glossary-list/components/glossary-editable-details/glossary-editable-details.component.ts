import { Component, OnInit, NgModule, Input, Output, EventEmitter } from '@angular/core';

//import { SetLanguagesComponent } from 'src/app/shared/components/set-languages/set-languages.component';

import { GlossaryService } from 'src/app/services/glossary.service';

import { Glossaries } from 'src/app/models/DTO/glossaries.type';


//import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
//import { TranslationSubstringService } from 'src/app/services/translationSubstring.service';
//import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { LanguageService } from 'src/app/services/languages.service';
import { ProjectsService } from 'src/app/services/projects.service';

import { Selectable } from 'src/app/shared/models/selectable.model';

import { Locale } from 'src/app/models/database-entities/locale.type';
import { LocalizationProject } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';

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
  

  //localizationProjects: LocalizationProject[] = [];


  constructor(
    private glossariesService: GlossaryService,
    private projectsService: ProjectsService,
    //private translationSubstringService: TranslationSubstringService,
    private languageService: LanguageService,
  ) { }

  ngOnInit() {
  }

  loadAvailableLanguages() {
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
    this.selectedLocales = newSelection;
  }

}
