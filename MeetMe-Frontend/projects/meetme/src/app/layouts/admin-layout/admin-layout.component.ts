import { Component, OnInit } from '@angular/core';
import { EventTypeService, AuthService } from '../../app-core';
import { ModalService } from '../../app-core/controls/modal/modalService';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent implements OnInit {

  constructor(
    private eventTypeService: EventTypeService,
    private authService: AuthService,
    public modalService: ModalService) { }

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
}
