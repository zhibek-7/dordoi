import { Pipe, PipeTransform } from "@angular/core";
import { Selectable } from "src/app/shared/models/selectable.model";
import { Locale } from "src/app/models/database-entities/locale.type";

@Pipe({
  name: "filterLocales"
})
export class FilterSelectableLocalesPipe implements PipeTransform {
  transform(
    listToFilter: Selectable<Locale>[],
    searchText?: string
  ): Selectable<Locale>[] {
    if (!searchText) return listToFilter;
    return listToFilter.filter(selectableLocale =>
      selectableLocale.model.name_text
        .toLowerCase()
        .includes(searchText.toLowerCase())
    );
  }
}
