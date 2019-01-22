import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { LanguageService } from 'src/app/services/languages.service';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { File } from 'src/app/models/database-entities/file.type';
import { FileService } from 'src/app/services/file.service';
import { Selectable } from 'src/app/shared/models/selectable.model';

@Component({
  selector: 'app-set-languages-modal',
  templateUrl: './set-languages-modal.component.html',
  styleUrls: ['./set-languages-modal.component.css']
})
export class SetLanguagesModalComponent extends ModalComponent implements OnInit {

  @Input() file: File;

  availableLocales: Selectable<Locale>[] = [];

  selectedLocales: Locale[] = [];

  constructor(
    private languageService: LanguageService,
    private fileService: FileService,
  ) { super(); }

  loadAvailableLanguages() {
    this.fileService.getTranslationLocalesForFileAsync(this.file)
      .subscribe(currentLocales =>
        this.languageService.getLanguageList()
          .subscribe(allLocales => {
              this.selectedLocales = currentLocales;
              this.availableLocales = allLocales
                .map(locale =>
                  new Selectable<Locale>(
                    locale,
                    this.selectedLocales.some(currentTermLocale => currentTermLocale.id == locale.id)));
            },
            error => console.log(error)));
  }

  setSelectedLocales(newSelection: Locale[]) {
    this.selectedLocales = newSelection;
  }

  show() {
    this.loadAvailableLanguages();
    super.show();
  }

  applyChanges() {
    this.fileService
      .updateTranslationLocalesForFileAsync(this.file, this.selectedLocales.map(locale => locale.id))
      .subscribe();
    this.hide();
  }

}
