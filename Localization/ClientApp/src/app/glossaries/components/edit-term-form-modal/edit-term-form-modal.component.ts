import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

import { TranslationSubstring } from 'src/app/models/database-entities/translationSubstring.type';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { PartOfSpeech } from 'src/app/models/database-entities/partOfSpeech.type';
import { Term } from 'src/app/models/Glossaries/term.type';
import { GlossariesService } from 'src/app/services/glossaries.service';
import { Glossary } from 'src/app/models/database-entities/glossary.type';
import { Locale } from 'src/app/models/database-entities/locale.type';
import { PartsOfSpeechService } from 'src/app/services/partsOfSpeech.service';
import { Translation } from 'src/app/models/database-entities/translation.type';
import { TranslationService } from 'src/app/services/translationService.service';
import { TranslationWithLocale } from 'src/app/glossaries/models/translationWithLocale.model';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';
import { forkJoin, Observable } from 'rxjs';

@Component({
  selector: 'app-edit-term-form-modal',
  templateUrl: './edit-term-form-modal.component.html',
  styleUrls: ['./edit-term-form-modal.component.css']
})
export class EditTermFormComponent extends ModalComponent implements OnInit {

  @Input() glossary: Glossary;

  _originalTermReference: Term;

  _term: Term = new Term();

  translations: TranslationWithLocale[] = [];

  @Input()
  set term(value: Term) {
    this._originalTermReference = value;
  }

  constructor(
    private translationService: TranslationService,
    private glossariesService: GlossariesService,
    private requestDataReloadService: RequestDataReloadService,
  ) { super(); }

  ngOnInit() {
  }

  updateTerm() {
    this.hide();

    if (!this.glossary)
      return;

    let updateTermObservable = this.glossariesService.updateTerm(this.glossary.id, this._term, this._term.partOfSpeechId);
    updateTermObservable.subscribe();

    let updateRequests = [updateTermObservable];
    for (let translation of this.translations) {
      if (translation.translated) {
        if (translation.id) {
          let updateTermTranslationObservable = this.translationService.updateTranslation(translation);
          updateTermTranslationObservable.subscribe(null, error => console.log(error));

          updateRequests.push(updateTermTranslationObservable);
        }
        else {
          this.translationService.createTranslate(translation);
        }
      }
    }
    forkJoin(updateRequests)
      .subscribe(() => this.requestDataReloadService.requestUpdate());
  }

  show() {
    this.resetTermViewModel();
    this.loadTranslations();
    super.show();
  }

  resetTermViewModel() {
    this._term = (JSON.parse(JSON.stringify(this._originalTermReference)));
    if (!this._term.partOfSpeechId) {
      this._term.partOfSpeechId = null;
    }
  }

  tempUserId: number = 300;

  loadTranslations() {   
    this.translationService.getAllTranslationsInStringById(this._term.id)
      .then(translations =>
        this.glossariesService.getTranslationLocalesForTerm(this.glossary.id, this._term.id)
          .subscribe(translationLocales =>
            this.translations = translationLocales
              .map(locale => {
                let translation = translations.find(translation => locale.id == translation.iD_Locale);
                if (!translation) {
                  translation = new Translation('', this._term.id, this.tempUserId, locale.id)
                }
                return new TranslationWithLocale(translation, locale);
              })));
  }

}
