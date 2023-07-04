import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService, ModalService } from './app-core';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit ,OnDestroy {
  title = 'Meet me ';

  destroyed$:Subject<boolean> = new Subject<boolean>();

  constructor(
    private authService: AuthService,
    public modalService: ModalService
  ) {

  }
  
  ngOnInit(): void {
  }
  
  isLogin(): boolean {
    return this.authService.isLogin()
  }
  
  toggleMenu(elementClassName: string) {
    let element = document.querySelector(`.${elementClassName}`);
    element?.classList.toggle('is-open');
  }
  
  onLogout() {
    this.authService.logout();
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
