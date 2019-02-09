import { Pipe, PipeTransform } from "@angular/core";

@Pipe({ name: "filterPhrases" })
export class FilterPhrasesPipe implements PipeTransform {
  transform(items: any[], searchText: string = ""): any[] {
    if (!items) return [];

    searchText = searchText.toLowerCase();

    return items.filter(it => {
      if (it.substring_to_translate == null) return [];
      return it.substring_to_translate.toLowerCase().includes(searchText);
    });
  }
}
