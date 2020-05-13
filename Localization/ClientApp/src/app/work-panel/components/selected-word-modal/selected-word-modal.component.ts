import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { ShareWordFromModalService } from '../../localServices/share-word-from-modal.service';

@Component({
  selector: 'selected-word-modal-component',
  templateUrl: './selected-word-modal.component.html',
  styleUrls: ['./selected-word-modal.component.css']
})
export class SelectedWordModalComponent implements OnInit {

  selectedWord: string;

  constructor(private dialogRef: MatDialogRef<SelectedWordModalComponent>,
              private shareWordFromModalService: ShareWordFromModalService,
              @Inject(MAT_DIALOG_DATA) public data: any ) {

      this.selectedWord = this.data.selectedWord;      
  }

  ngOnInit() {
  }

  searchInMemory(){
    this.dialogRef.close();    

    this.shareWordFromModalService.clickFindInMemory(this.selectedWord);
  }

  searchInGlossary(){
    this.dialogRef.close();

    this.shareWordFromModalService.clickFindInGlossary(this.selectedWord);
  }

  addInGlossary(){
    this.dialogRef.close();
  }

}
