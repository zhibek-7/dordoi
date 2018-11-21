import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'filterPhrases'})
export class FilterPhrasesPipe implements PipeTransform {

    transform(items: any[], searchText: string = ""): any[]{
        if(!items) return[];
        //if(!searchText) return[];  если включить то в списка фраз ничего не будет(т.к. поле изначально пустое)

        searchText = searchText.toLowerCase();

        return items.filter(it => {
            return it.text.toLowerCase().includes(searchText);
        });
    }
}