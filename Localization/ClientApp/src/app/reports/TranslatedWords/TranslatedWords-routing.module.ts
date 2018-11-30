import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TranslatedWordsComponent } from './TranslatedWords.component';

const routes: Routes = [
  {
    path: '',
    component: TranslatedWordsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TranslatedWordsRoutingModule { }


