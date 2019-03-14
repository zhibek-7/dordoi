import { Component, OnInit, Input } from '@angular/core';

import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { LanguageService } from 'src/app/services/languages.service';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { Selectable } from 'src/app/shared/models/selectable.model';
import { TranslationSubstringService } from 'src/app/services/translationSubstring.service';
import { ProjectsService } from 'src/app/services/projects.service';

@Component({
  selector: 'app-set-languages-form-modal',
  templateUrl: './set-languages-form-modal.component.html',
  styleUrls: ['./set-languages-form-modal.component.css']
})
export class SetLanguagesFormModalComponent extends ModalComponent implements OnInit {

  @Input() translationSubstring: TranslationSubstring;

  availableLocales: Selectable<Locale>[] = [];

  selectedLocales: Locale[] = [];

  constructor(
    private projectsService: ProjectsService,
    private translationSubstringService: TranslationSubstringService,
    private languageService: LanguageService,
  ) { super(); }

  loadAvailableLanguages() {
    this.translationSubstringService.getTranslationLocalesForString(this.translationSubstring.id)
      .subscribe(localesForTranslationSubstring =>
        this.projectsService.getProject(this.projectsService.currentProjectId)
          .subscribe(project =>
            this.languageService.getLanguageList()
              .subscribe(allLocales => {
                this.selectedLocales = localesForTranslationSubstring;
                this.availableLocales = allLocales
                  .filter(locale => locale.id != project.ID_Source_Locale)
                  .map(locale =>
                    new Selectable<Locale>(
                      locale,
                      this.selectedLocales.some(selectedLocale => selectedLocale.id == locale.id)));
              },
                error => console.log(error)),
            error => console.log(error)),
        error => console.log(error));
  }

  setSelectedLocales(newSelection: Locale[]) {
    this.selectedLocales = newSelection;
  }

  show() {
    this.loadAvailableLanguages();
    super.show();
  }

  applyChanges() {
    this.translationSubstringService
      .setTranslationLocalesForString(
        this.translationSubstring.id,
        this.selectedLocales.map(locale => locale.id))
      .subscribe();
    this.hide();
  }

}
