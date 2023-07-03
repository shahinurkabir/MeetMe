import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IUserProfileDetailResponse, EventTypeService } from '../../../app-core';

@Component({
  selector: 'app-user-calendar',
  templateUrl: './user-calendar.component.html',
  styleUrls: ['./user-calendar.component.scss']
})
export class UserCalendarComponent implements OnInit {

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
