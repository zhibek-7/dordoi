import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: "filterTerms",
  pure: false
})
export class FilterTermsPipe implements PipeTransform {
  transform(items: any[], searchText: string = ""): any[] {
    if (!items) return [];

    searchText = searchText.toLowerCase();

    return items.filter(it => {
      if (it.substringToTranslate == null) return [];
      return it.substringToTranslate.toLowerCase().includes(searchText);
    });
  }
}
