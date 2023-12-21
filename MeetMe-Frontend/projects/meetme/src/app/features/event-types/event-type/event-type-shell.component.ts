import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataExchangeService } from '../services/data-exchange-services';

@Component({
  selector: 'app-eventtype',
  templateUrl: './event-type-shell.component.html',
  styleUrls: ['./event-type-shell.component.css']
})
export class EventTypeShellComponent implements OnInit {
 @Input()eventTypeName: string= '';
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private dataExchangeService: DataExchangeService
  ) {


  }

  ngOnInit(): void {
    this.dataExchangeService.getEventTypeTitle().subscribe(title => {
      this.eventTypeName = title;
    });
  }
  onBackToEventTypeList() {
     this.router.navigate(['../'], { relativeTo: this.route });
  }
  
}
