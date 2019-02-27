import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { ProjectsService } from "src/app/services/projects.service";
import { GlossaryService } from 'src/app/services/glossary.service';

import { Selectable } from "src/app/shared/models/selectable.model";
import { LocalizationProjectForSelectDTO } from "src/app/models/DTO/localizationProjectForSelectDTO.type";
import { GlossariesForEditing } from 'src/app/models/DTO/glossariesDTO.type';
import { Glossary } from 'src/app/models/database-entities/glossary.type';

export interface SetProjectsModalComponentInputData {
  glossary: Glossary;
}

@Component({
  selector: 'app-set-projects-modal',
  templateUrl: './set-projects-modal.component.html',
  styleUrls: ['./set-projects-modal.component.css']
})
export class SetProjectsModalComponent implements OnInit {

  availableLocalizationProjects: Selectable<LocalizationProjectForSelectDTO>[] = [];

  glossary: Glossary;

  glossariesForEditing: GlossariesForEditing;

  errorsRequiredLocalizationProjects: boolean = false;

  constructor(
    public dialogRef: MatDialogRef<SetProjectsModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SetProjectsModalComponentInputData,
    private projectsService: ProjectsService,
    private glossaryService: GlossaryService,
  ) {
    this.glossary = data.glossary;
  }

  async ngOnInit() {
    await this.loadGlossaryForEditing();
    this.loadAvailableLocalizationProjects();
  }

  loadAvailableLocalizationProjects() {
    this.projectsService.getLocalizationProjectForSelectDTOByUser().subscribe(
      localizationProject => {
        this.availableLocalizationProjects = localizationProject.map(
          localProject =>
            new Selectable<LocalizationProjectForSelectDTO>(
              localProject,
              this.glossariesForEditing.localization_Projects_Ids.some(
                selectedLocalizationProjectId =>
                  selectedLocalizationProjectId == localProject.id
              )
            )
        );
      },
      error => console.error(error)
    );
  }

  async loadGlossaryForEditing() {
    this.glossariesForEditing = await this.glossaryService.getGlossaryForEditing(this.glossary.id);
  }

  toggleSelection(
    localizationProject: Selectable<LocalizationProjectForSelectDTO>
  ) {
    localizationProject.isSelected = !localizationProject.isSelected;
    this.raiseSelectionChanged();
  }

  raiseSelectionChanged() {
    this.glossariesForEditing.localization_Projects_Ids = this.availableLocalizationProjects
      .filter(localizationProject => localizationProject.isSelected)
      .map(selectable => selectable.model.id);
    //Валидация. Обязательно должно быть выбрано хотя бы одно значение.
    this.errorsRequiredLocalizationProjects =
      this.glossariesForEditing.localization_Projects_Ids.length == 0;
  }

  close() {
    this.dialogRef.close();
  }

  applyChanges() {
    this.dialogRef.close(this.glossariesForEditing);
  }

}
