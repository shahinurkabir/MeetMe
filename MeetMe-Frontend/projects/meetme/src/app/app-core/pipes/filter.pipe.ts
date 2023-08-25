import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {
  transform(items: any[], property: string, filterValue: any): any[] {
    if (!items || !property || filterValue === undefined) {
      return items;
    }

    return items.filter(item => item[property] === filterValue);
  }
}
