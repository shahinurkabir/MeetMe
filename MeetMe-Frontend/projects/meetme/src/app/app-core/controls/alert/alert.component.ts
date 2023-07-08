import { Component, Input, OnInit } from '@angular/core';
import { Alert } from './alert';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss']
})
export class AlertComponent implements OnInit {
  @Input() alert: Alert|undefined;
  constructor() { }

  ngOnInit(): void {
  }

}
