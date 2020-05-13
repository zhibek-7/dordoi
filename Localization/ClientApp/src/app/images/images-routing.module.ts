import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ImagesTilesComponent } from './components/images-tiles/images-tiles.component';

const routes: Routes = [
  {
    path: '',
    component: ImagesTilesComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ImagesRoutingModule { }
