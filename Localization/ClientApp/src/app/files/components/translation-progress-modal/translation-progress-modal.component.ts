import { Component, OnInit } from '@angular/core';

import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { LanguageService } from 'src/app/services/languages.service';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { File } from 'src/app/models/database-entities/file.type';
import { FileService } from 'src/app/services/file.service';
import { FileTranslationInfo } from 'src/app/models/database-entities/fileTranslationInfo.type';

@Component({
  selector: 'app-translation-progress-modal',
  templateUrl: './translation-progress-modal.component.html',
  styleUrls: ['./translation-progress-modal.component.css']
})
export class TranslationProgressModalComponent extends ModalComponent implements OnInit {

  file: File;

  fileTranslationInfos: FileTranslationInfo[] = [];

  locales: Locale[] = [];

  getLocale(id: number): Locale {
    return this.locales.filter(locale => locale.id == id)[0];
  }

  constructor(
    private languageService: LanguageService,
    private fileService: FileService,
  ) { super(); }

  loadLanguages() {
    this.languageService.getLanguageList()
      .subscribe(allLocales => {
        this.locales = allLocales;
      },
      error => {
        console.log(error);
        alert(error);
      });
  }

  loadFileTranslationInfo() {
    this.fileService.getFileTranslationInfos(this.file)
      .subscribe(translationInfos => this.fileTranslationInfos = translationInfos,
                 error => alert(error));
  }

  showForFile(file: File) {
    this.file = file;
    this.loadLanguages();
    this.loadFileTranslationInfo();
    this.show();
  }

}
