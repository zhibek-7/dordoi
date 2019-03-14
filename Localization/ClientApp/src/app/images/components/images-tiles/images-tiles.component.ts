import { Component, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material";

import { ImageEditingModalComponent } from '../image-editing-modal/image-editing-modal.component';

import { ImagesService } from 'src/app/services/images.service';
import { Image } from "src/app/models/database-entities/image.type";
import { ProjectsService } from "src/app/services/projects.service";

declare var $: any;

@Component({
  selector: "app-images-tiles",
  templateUrl: "./images-tiles.component.html",
  styleUrls: ["./images-tiles.component.css"]
})
export class ImagesTilesComponent implements OnInit {

  imgs: Image[] = [];

  constructor(
    private imagesService: ImagesService,
    private projectsService: ProjectsService,
    private dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.imagesService.getByProjectId(this.projectsService.currentProjectId)
      .subscribe(images => {
        this.imgs = images;
      },
      error => {
        console.log(error);
        alert(error);
      });
  }

  showModal(clickEventArgs, image: Image) {
    let dialogRef = this.dialog.open(ImageEditingModalComponent, {
      data: { clickEventArgs: clickEventArgs, image: image }
    });
    dialogRef.componentInstance
  }

}

