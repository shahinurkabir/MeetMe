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

  static convertToDays(minutes: number): number {
    if (minutes <= 0) return 0;
    return (minutes / 60) / 24;
  }

  static toggleModalDialog(el: Element | null) {
    document.querySelector('#modal-backdrop')?.classList.toggle('is-open')
    el?.classList.toggle('is-open')
    document.body.classList.toggle('is-modal-open')
  }

  static parseJwt(token: string) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
  }

  static cloneObject<T>(a: T): T {
    return JSON.parse(JSON.stringify(a));
  }
}