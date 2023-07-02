import { Component, OnInit } from '@angular/core';
import { EventTypeService } from '../../services/eventtype.service';
import { ActivatedRoute } from '@angular/router';
import { IUserProfileDetailResponse } from '../../interfaces/event-type-interfaces';

@Component({
  selector: 'app-user-event-types',
  templateUrl: './user-event-types.component.html',
  styleUrls: ['./user-event-types.component.scss']
})
export class UserEventTypesComponent implements OnInit {

  userProfileDetails: IUserProfileDetailResponse | undefined;
  baseUri: string = "";

  constructor(
    private eventTypeService:EventTypeService,
    private route: ActivatedRoute,
    ) { }

  ngOnInit(): void {

    this.baseUri = this.route.snapshot.paramMap.get("user") ?? "";
    this.loadData();
  }
  
  loadData(){
    this.eventTypeService.getListByBaseURI(this.baseUri).subscribe({
      next: response => {
        this.userProfileDetails = response;
      },
      error: (error) => { console.log(error) },
      complete: () => { }
    })
  }
}
