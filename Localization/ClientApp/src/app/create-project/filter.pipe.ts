import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(dropdownList: any, searchText?: any): any {

    if(searchText===undefined) return dropdownList;

    return dropdownList.filter(
      function(lang){
        console.log(searchText);
	if(lang.itemName===undefined || lang.itemName == null) return dropdownList;
        
         return lang.itemName.toLowerCase().includes(searchText.toLowerCase());
      }
    );
  }

}
