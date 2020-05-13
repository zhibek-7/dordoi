import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";

import { ModalComponent } from "src/app/shared/components/modal/modal.component";

import { TranslationSubstringService } from "src/app/services/translationSubstring.service";
import { TranslationService } from "src/app/services/translationService.service";
import { ProjectsService } from "src/app/services/projects.service";
import { LanguageService } from "src/app/services/languages.service";

import {
  TranslationSubstringForEditingDTO,
  TranslationSubstringTableViewDTO
} from "src/app/models/DTO/TranslationSubstringDTO.type";
//import { Translation } from 'src/app/models/database-entities/translation.type';
//import { TranslationDTO } from 'src/app/models/DTO/translationDTO';

@Component({
  selector: "app-edit-string-project-form-modal",
  templateUrl: "./edit-string-project-form-modal.component.html",
  styleUrls: ["./edit-string-project-form-modal.component.css"]
})
export class EditStringProjectFormModalComponent extends ModalComponent
  implements OnInit {
  @Input()
  translationSubstring: TranslationSubstringForEditingDTO;

  @Output()
  editedSubmitted = new EventEmitter();

  constructor(
    private translationSubstringService: TranslationSubstringService,
    private translationService: TranslationService,
    private projectsService: ProjectsService,
    private languageService: LanguageService
  ) {
    super();
  }

  ngOnInit() {}

  show() {
    this.getSourceLocale();
    this.getTranslation();

    super.show();
  }

  getSourceLocale() {
    this.languageService
      .getSourceLocaleLocalizationProject(this.projectsService.currentProjectId)
      .subscribe(
        locale => {
          this.translationSubstring.localization_project_source_locale_name =
            locale.name_text;
        },
        error => {
          console.error(error);
        }
      );
  }

  getTranslation() {
    this.translationService
      .getAllTranslationsInStringWithLocaleById(this.translationSubstring.id)
      .subscribe(
        translations => {
          this.translationSubstring.translations = translations;
        },
        error => {
          console.error(error);
        }
      );
  }

  submit() {
    this.hide();

    this.saveTranslationSubstring();
    this.saveTranslation();

    this.editedSubmitted.emit();
  }

  saveTranslationSubstring() {
    let editedTranslationSubstring = new TranslationSubstringTableViewDTO();
    editedTranslationSubstring.id = this.translationSubstring.id;
    editedTranslationSubstring.substring_to_translate = this.translationSubstring.substring_to_translate;

    this.translationSubstringService
      .saveSubstringToTranslate(editedTranslationSubstring)
      .subscribe(
        () => {},
        error => {
          console.error(error);
        }
      );
  }

  saveTranslation() {
    this.translationService
      .saveTranslated(this.translationSubstring.translations)
      .subscribe(
        () => {},
        error => {
          console.error(error);
        }
      );
  }
}
