import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EventTypeService } from '../../../services/eventtype.service';

@Component({
  selector: 'app-eventtype',
  templateUrl: './eventtype.component.html',
  styleUrls: ['./eventtype.component.css']
})
export class EventTypeComponent implements OnInit {

  constructor(
    private eventTypeService: EventTypeService,
    private route: ActivatedRoute
  ) {

    this.route.params.subscribe((params) => {
      console.log(params);
      console.log(this.route.snapshot.data);
    });

  }

  ngOnInit(): void {
  }

  loadEventTypeDetail(id: string) {
    this.eventTypeService.getById(id).subscribe(response => {
      console.log(response)
    })
  }
}
