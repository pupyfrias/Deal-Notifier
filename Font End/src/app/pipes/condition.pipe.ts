import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'Condition'
})
export class ConditionPipe implements PipeTransform {

  condition:string[] = ['New','Used']
  transform(value: number, ...args: unknown[]): string {
    const index = value-1;
    return this.condition[index];
  }

}
