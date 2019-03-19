import { Component, OnInit } from "@angular/core";
import { MatDialog, PageEvent } from "@angular/material";

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

  pageSize: number = 10;
  totalCount: number = 0;
  imageNameFilter: string = "";

  constructor(
    private imagesService: ImagesService,
    private projectsService: ProjectsService,
    private dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.loadImages();
  }

  loadImages(offset: number = 0) {
    this.imagesService.getByProjectId(this.projectsService.currentProjectId, this.imageNameFilter, offset, this.pageSize)
      .subscribe(imagesResponse => {
        this.totalCount = +imagesResponse.headers.get('totalCount');
        this.imgs = imagesResponse.body;
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

  onPageChanged(args: PageEvent) {
    this.pageSize = args.pageSize;
    const currentOffset = args.pageSize * args.pageIndex;
    this.loadImages(currentOffset);
  }

}

