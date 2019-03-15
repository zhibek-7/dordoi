import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {
  MatIconModule,
  MatInputModule,
  MatButtonModule,
} from '@angular/material';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import 'hammerjs';
import * as $ from 'jquery';

import { ImagesRoutingModule } from './images-routing.module';

import { ImagesTilesComponent } from './components/images-tiles/images-tiles.component';
import { ImageEditingModalComponent } from './components/image-editing-modal/image-editing-modal.component';

import { RequestInterceptorService } from 'src/app/services/requestInterceptor.service';
import { ImagesService } from 'src/app/services/images.service';

@NgModule({
  declarations: [
    ImagesTilesComponent,
    ImageEditingModalComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    HttpClientModule,
    MatIconModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule,
    MatTooltipModule,
    ImagesRoutingModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
    ImagesService,
  ],
  entryComponents: [
    ImageEditingModalComponent,
  ],
})
export class ImagesModule { }
