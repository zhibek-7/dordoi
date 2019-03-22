import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface DialogData {
  animal: string;
  name: string;
}

@Component({
  selector: 'app-dialog-invite-translator',
  templateUrl: './dialog-invite-translator.component.html',
  styleUrls: ['./dialog-invite-translator.component.css']
})
export class DialogInviteTranslatorComponent {

  constructor(
    public dialogRef: MatDialogRef<DialogInviteTranslatorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
