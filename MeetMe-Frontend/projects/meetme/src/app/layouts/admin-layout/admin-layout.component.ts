import { Component, ElementRef, OnDestroy, OnInit, Renderer2, ViewChild } from '@angular/core';
import { EventTypeService, AuthService, ModalService } from '../../app-core';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent implements OnInit, OnDestroy {

  destroyed$:Subject<boolean> = new Subject<boolean>();
  
  @ViewChild('toggleButton') toggleButton: ElementRef | undefined;
  @ViewChild('accountSettingsMenu') menu: ElementRef | undefined
   userName: string = "";
  constructor(
    private eventTypeService: EventTypeService,
    private authService: AuthService,
    public modalService: ModalService,
    private renderer: Renderer2
  ) {
    this.renderer.listen('window', 'click', (e: Event) => {

      if (e.target != this.toggleButton?.nativeElement && e.target != this.menu?.nativeElement) {
        this.menu?.nativeElement.classList.remove('is-open');
      }

    });

    this.authService.loginChanged$.subscribe(res => {
      this.userName = this.authService.userName;
    } );
  }

  ngOnInit(): void {
   this.userName = this.authService.userName;
  }
  isLogin(): boolean {
    return this.authService.isLogin()
  }
  toggleMenu(e:any) {
    e.preventDefault();
    this.menu?.nativeElement.classList.toggle('is-open');
  }

  onLogout() {
    this.authService.logout();
  }
  ngOnDestroy() {
    this.destroyed$.next(true);
    this.destroyed$.complete();

  }
}
