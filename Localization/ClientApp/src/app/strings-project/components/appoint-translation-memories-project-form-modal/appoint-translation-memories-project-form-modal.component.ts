import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";

import { ModalComponent } from "src/app/shared/components/modal/modal.component";

import { Selectable } from "src/app/shared/models/selectable.model";

import { ProjectTranslationMemory } from '../../../models/database-entities/projectTranslationMemory.type';

import { ProjectTranslationMemoryService } from "src/app/services/projectTranslationMemory.service";

@Component({
  selector: 'app-appoint-translation-memories-project-form-modal',
  templateUrl: './appoint-translation-memories-project-form-modal.component.html',
  styleUrls: ['./appoint-translation-memories-project-form-modal.component.css']
})
export class AppointTranslationMemoriesProjectFormModalComponent extends ModalComponent implements OnInit {

  dataSource: Selectable<ProjectTranslationMemory>[];

  lastSortColumnName: string = '';
  isSortingAscending: boolean = true;

  constructor(private projectTranslationMemoryService: ProjectTranslationMemoryService) {
    super();
  }

  ngOnInit() {
  }
  
  show() {
    this.loadAvailableTranslationMemory();
    super.show();
  }

  loadAvailableTranslationMemory() {

    this.projectTranslationMemoryService.getAllByProject()
      .subscribe(projectTranslationMemories => {
          this.dataSource = projectTranslationMemories.map(
            t => new Selectable<ProjectTranslationMemory>(
              t,
              t.project_id != null
            )
          );
        console.log("loadAvailableTranslationMemory", projectTranslationMemories);
        },
      error => console.error(error)
    );
  }

  requestSortBy(columnName: string) {
    if (columnName != this.lastSortColumnName) {
      this.isSortingAscending = true;
    }
    
    if (columnName == "name_text")
      if (this.isSortingAscending)
        this.dataSource.sort((t1, t2) => t1.model.translationMemory_name_text > t2.model.translationMemory_name_text ? 1 : -1);
      else this.dataSource.sort((t1, t2) => t1.model.translationMemory_name_text > t2.model.translationMemory_name_text ? -1 : 1);
    if (columnName == "selected")
      if (this.isSortingAscending)
        this.dataSource.sort((t1, t2) => t1.isSelected > t2.isSelected ? 1 : -1).sort((t1, t2) => t1.model.translationMemory_name_text > t2.model.translationMemory_name_text ? 1 : -1);
      else this.dataSource.sort((t1, t2) => t1.isSelected > t2.isSelected ? -1 : 1).sort((t1, t2) => t1.model.translationMemory_name_text > t2.model.translationMemory_name_text ? 1 : -1);

    this.lastSortColumnName = columnName;
    this.isSortingAscending = !this.isSortingAscending;
  }

  get isAllSelected(): boolean {
    return this.dataSource ? this.dataSource.every(t => t.isSelected) : false;

    //if (this.dataSource) {
    //  if (this.dataSource.every(t => t.isSelected)) return true;
    //  else if (this.dataSource.every(t => !t.isSelected)) return false;
    //}
    //return null;
  }

  allSelected() {
    if (this.isAllSelected)
      this.dataSource.forEach(t => t.isSelected = false);
    else
      if (!this.isAllSelected)
      this.dataSource.forEach(t => t.isSelected = true);

  }

  submit() {
    this.hide();

    let selected = this.dataSource.filter(t => t.isSelected).map(t => t.model.translationMemory_id);

    this.projectTranslationMemoryService.saveProjectTranslationMemories(selected)
      .subscribe(
        () => {},
        error => console.error(error)
      );

  }
}
