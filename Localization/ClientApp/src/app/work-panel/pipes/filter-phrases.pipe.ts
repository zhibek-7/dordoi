import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'filterPhrases'})
export class FilterPhrasesPipe implements PipeTransform {

    transform(items: any[], searchText: string = ""): any[]{
        if(!items) return[];

        searchText = searchText.toLowerCase();

        return items.filter(it => {
            return it.substringToTranslate.toLowerCase().includes(searchText);
        });
    }
}