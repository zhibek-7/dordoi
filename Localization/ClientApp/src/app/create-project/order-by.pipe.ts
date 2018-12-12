import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'orderBy'
})
export class OrderByPipe implements PipeTransform {

  transform(value: any[], args?: any): any {
  if(args==='descending'){

  args='ascending';
}else
if(args==='ascending'){

  args='descending';
} 

    if(args==='ascending'){
      return value.sort();
    }else if(args==='descending'){
      return value.sort().reverse();
    }
   
  }

}
