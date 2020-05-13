import { Component, OnInit, Inject } from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material";

import { Image } from "src/app/models/database-entities/image.type";

@Component({
  selector: "show-image-modal-component",
  templateUrl: "./show-image-modal.html",
  styleUrls: ["./show-image-modal.css"]
})
export class ShowImageModalComponent implements OnInit {
  selectedImage: Image;

  constructor(
    //TODO не используется private dialogRef: MatDialogRef<ShowImageModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.selectedImage = this.data.selectedImage;
  }

  ngOnInit(): void {}
}
