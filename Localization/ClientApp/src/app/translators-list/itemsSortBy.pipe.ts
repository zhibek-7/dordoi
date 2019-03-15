import { Pipe, PipeTransform } from '@angular/core';
import { Translator } from 'src/app/models/Translators/translator.type';

@Pipe({
  name: 'sortBy'
})
export class ItemsSortBy implements PipeTransform {
  transform(translators: Translator[], sortingColumn: string, isDesc: boolean) {
    if (translators && translators.length>0) {
      return translators.sort(function (a, b) {
        if (isDesc) {
          if (a[sortingColumn] > b[sortingColumn]) {
            return 1;
          }
          if (a[sortingColumn] < b[sortingColumn]) {
            return -1;
          }
          return 0;
        } else {
          if (a[sortingColumn] < b[sortingColumn]) {
            return 1;
          }
          if (a[sortingColumn] > b[sortingColumn]) {
            return -1;
          }
          return 0;
        }
      });
    } else {
      return translators;
    }
  }
}

