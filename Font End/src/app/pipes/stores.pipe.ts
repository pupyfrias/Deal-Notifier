import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'Stores'
})
export class StoresPipe implements PipeTransform {

  private stores: string[] = ['Amazon', 'eBay', 'TheStore'];

  transform(value: string, ...args: unknown[]): string {
    const index = parseInt(value)-1;
    return this.stores[index];
  }

}
