import { Component, OnDestroy, OnInit } from '@angular/core';
import { Alert, AlertService, AuthService } from './app-core';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'Meet me ';

  destroyed$: Subject<boolean> = new Subject<boolean>();
  showAlert: boolean = false;
  alert:Alert|undefined;

  constructor(
    private authService: AuthService,
    private alertService: AlertService,
  ) {
    this.alertService.alert$
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: alert => {
          this.alert = alert;
          setTimeout(() => {
            this.alert=undefined;
          }, 2000);
        }
      });
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
