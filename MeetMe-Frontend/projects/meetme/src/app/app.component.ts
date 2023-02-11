import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth-service';
import { EventTypeService } from './services/eventtype.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Meet me ';
  /**
   *
   */
  constructor(private eventTypeService: EventTypeService, private authService: AuthService) {

  }

  ngOnInit(): void {
  }
  isLogin(): boolean {
    return this.authService.isLogin()
  }
  toggleMenu(elementClassName:string) {
    let element=document.querySelector(`.${elementClassName}`);
    element?.classList.toggle('is-open');
  }
  onLogout() {
    this.authService.logout();
  }
}
