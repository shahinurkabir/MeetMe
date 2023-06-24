import { Component, OnInit } from '@angular/core';
import { ModalService } from '../../controls/modal/modalService';
import { AuthService } from '../../services/auth-service';
import { EventTypeService } from '../../services/eventtype.service';

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
