export function convertToDays(minutes: number): number {
  if (minutes <= 0) return 0;
  return (minutes / 60) / 24;
}

export function toggleModalDialog(el: Element | null) {
  document.querySelector('#modal-backdrop')?.classList.toggle('is-open')
  el?.classList.toggle('is-open')
  document.body.classList.toggle('is-modal-open')
}

export function parseJwt(token: string) {
  var base64Url = token.split('.')[1];
  var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
    return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
  }).join(''));

  return JSON.parse(jsonPayload);
}




export function cloneObject<T>(a: T): T {
  return JSON.parse(JSON.stringify(a));
}