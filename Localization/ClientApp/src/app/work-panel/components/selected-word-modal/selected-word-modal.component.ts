import { Component, OnInit, Input, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'selected-word-modal-component',
  templateUrl: './selected-word-modal.component.html',
  styleUrls: ['./selected-word-modal.component.css']
})
export class SelectedWordModalComponent implements OnInit {

  @Input() selectedWord: string;

  constructor(private dialogRef: MatDialogRef<SelectedWordModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any ) {

      this.dialogRef.updatePosition({ top: '50px', left: '50px' });
  }

  ngOnInit() {
  }

}
