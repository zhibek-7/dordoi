import { Component, OnInit,  Output, EventEmitter } from "@angular/core";

import { ModalComponent } from "src/app/shared/components/modal/modal.component";

import { ProjectsService } from "src/app/services/projects.service";

import { TranslationMemoryForEditingDTO } from "src/app/models/DTO/translationMemoryDTO.type";

@Component({
  selector: 'app-add-translation-memory-form-modal',
  templateUrl: './add-translation-memory-form-modal.component.html',
  styleUrls: ['./add-translation-memory-form-modal.component.css']
})
export class AddTranslationMemoryFormModalComponent extends ModalComponent implements OnInit {

  newTranslationMemory: TranslationMemoryForEditingDTO;

  @Output()
  newSubmitted = new EventEmitter<TranslationMemoryForEditingDTO>();

  constructor(private projectsService: ProjectsService) {
    super();
    this.resetNewTranslationMemoryModel();
  }

  ngOnInit() { }

  show() {
    super.show();
    this.resetNewTranslationMemoryModel();
  }

  submit() {
    this.hide();
    this.newSubmitted.emit(this.newTranslationMemory);
  }

  resetNewTranslationMemoryModel() {
    this.newTranslationMemory = new TranslationMemoryForEditingDTO();
    this.newTranslationMemory.locales_ids = [];
    this.newTranslationMemory.localization_projects_ids = [];
    let currentProjectId = this.projectsService.currentProjectId;
    if (!currentProjectId.isEmpty())
      this.newTranslationMemory.localization_projects_ids.push(currentProjectId);
  }

}
