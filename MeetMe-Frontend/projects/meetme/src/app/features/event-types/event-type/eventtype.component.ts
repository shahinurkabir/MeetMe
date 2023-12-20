import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EventTypeService } from '../../../app-core';
import { DataExchangeService } from '../services/data-exchange-services';

@Component({
  selector: 'app-eventtype',
  templateUrl: './eventtype.component.html',
  styleUrls: ['./eventtype.component.css']
})
export class EventTypeComponent implements OnInit {
 @Input()eventTypeName: string= '';
  constructor(
    private eventTypeService: EventTypeService,
    private route: ActivatedRoute,
    private dataExchangeService: DataExchangeService
  ) {

    // this.route.params.subscribe((params) => {
    //   console.log(params);
    //   console.log(this.route.snapshot.data);
    // });

  }

  ngOnInit(): void {
    this.dataExchangeService.getEventTypeTitle().subscribe(title => {
      this.eventTypeName = title;
    });
  }

  // loadEventTypeDetail(id: string) {
  //   this.eventTypeService.getById(id).subscribe(response => {
  //     console.log(response)
  //   })
  // }
}
