export function convertToDays(minutes: number): number {
  if (minutes <= 0) return 0;
  return (minutes / 60) / 24;
}

export function toggleModalDialog(el: Element|null) {
  document.querySelector('#modal-backdrop')?.classList.toggle('is-open')
  el?.classList.toggle('is-open')
  document.body.classList.toggle('is-modal-open')
}

