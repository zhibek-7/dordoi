import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Translator } from 'src/app/models/Translators/translator.type';
import { ProjectsService } from 'src/app/services/projects.service';
import { LocalizationProjectForSelectDTO } from 'src/app/models/DTO/localizationProjectForSelectDTO.type';

export interface DialogData {
  user: Translator;
  //name: string;
}

@Component({
  selector: 'app-dialog-invite-translator',
  templateUrl: './dialog-invite-translator.component.html',
  styleUrls: ['./dialog-invite-translator.component.css']
})
export class DialogInviteTranslatorComponent {

  projects: LocalizationProjectForSelectDTO[];
  selectedProjects: LocalizationProjectForSelectDTO[] = [];
  

  constructor(private projectsService: ProjectsService,
    public dialogRef: MatDialogRef<DialogInviteTranslatorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Translator) {
    console.log("dialog: ", data);
  }

  ngOnInit() {
    this.projectsService.getLocalizationProjectForSelectDTOByUser().subscribe(
      localizationProject => this.projects = localizationProject,
      error => console.error(error));
  }

  valueChange(selected: LocalizationProjectForSelectDTO, event: any) {
    if (event.checked)
      this.selectedProjects.push(selected);
    else
      this.selectedProjects = this.selectedProjects.filter(t => t != selected);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
