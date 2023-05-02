import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'Stores'
})
export class StoresPipe implements PipeTransform {
  stores: string[] = ['Amazon', 'eBay', 'TheStore'];

  transform(value: number, ...args: unknown[]): string {
    const index = value - 1;
    return this.stores[index];
  }
}
