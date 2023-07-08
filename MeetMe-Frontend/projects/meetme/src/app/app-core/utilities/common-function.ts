export class CommonFunction {
    static toCamelCase(str: string): string {
        return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function (word: string, index: number) {
          return index === 0 ? word.toLowerCase() : word;
        }).replace(/\s+/g, '');
      }
    
      static getErrorListAndShowIncorrectControls(form: any, errors: any) {
        let errorList = [];
        for (let key in errors) {
          let values = errors[key];
          try {
            form.controls[this.toCamelCase(key)]?.setErrors({ 'incorrect': values });
            for (let index in values) {
              errorList.push(values[index]);
            }
          } catch (e) {
            console.log(e);
          }
        }
        return errorList;
      }
}