import { AfterViewInit, Directive, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[loading-indicator]'
})
export class LoadingIndicatorDirective implements AfterViewInit {
  @Input('loading') set appLoadingIndicator(value: boolean) {
    this.showLoadingIndicator(value);
  };

  constructor(private elementRef: ElementRef) { }

  ngAfterViewInit(): void {

  }

  private showLoadingIndicator(isLoading: boolean) {
    if (isLoading) {
      this.elementRef.nativeElement.appendChild(this.createLoadingIndicator());
    } else {
      const elToRemove = this.elementRef.nativeElement.querySelector('.dot-pulse')
      this.elementRef.nativeElement.removeChild(elToRemove);
    }
  }
  createLoadingIndicator(): HTMLElement {
    let loadingIndicator = document.createElement('div');
    loadingIndicator.classList.add('dot-pulse');
    return loadingIndicator;
  }
}
