import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TranslationMemoriesComponent } from '../components/translation-memories/translation-memories.component';

const routes: Routes = [
  {
    path: '',
    component: TranslationMemoriesComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TranslationMemoriesRoutingModule { }
