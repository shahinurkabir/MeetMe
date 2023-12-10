// clipboard.service.ts

import { Injectable, NgZone } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ClipboardService {
  constructor(private ngZone: NgZone) {}

  copyToClipboard(text: string): void {
    this.ngZone.runOutsideAngular(() => {
      const tempTextArea = document.createElement('textarea');
      tempTextArea.value = text;
      document.body.appendChild(tempTextArea);
      tempTextArea.select();
      document.execCommand('copy');
      document.body.removeChild(tempTextArea);
    });
  }
}
