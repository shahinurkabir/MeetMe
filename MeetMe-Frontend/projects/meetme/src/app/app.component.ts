import { Component, OnInit } from '@angular/core';
import { ModalService } from './controls/modal/modalService';
import { AuthService } from './services/auth-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Meet me ';
  
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
}
