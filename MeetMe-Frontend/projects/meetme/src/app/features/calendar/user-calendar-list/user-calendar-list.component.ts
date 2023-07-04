import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IUserProfileDetailResponse, EventTypeService, AuthService } from '../../../app-core';

@Component({
  selector: 'app-user-calendar-list',
  templateUrl: './user-calendar-list.component.html',
  styleUrls: ['./user-calendar-list.component.scss']
})
export class UserCalendarListComponent implements OnInit {

  userProfileDetails: IUserProfileDetailResponse | undefined;
  baseUri: string = "";
  userName: string = "";
  welcomeText:string |undefined;
  constructor(
    private eventTypeService:EventTypeService,
    private route: ActivatedRoute,
    private authService: AuthService
    ) {
      this.authService.loginChanged$.subscribe(res => {
        this.baseUri = this.authService.baseUri;
        this.userName = this.authService.userName;
      } );
     }

  ngOnInit(): void {

    this.baseUri = this.route.snapshot.paramMap.get("user") ?? "";
    this.loadData();
  }
  
  loadData(){
    this.eventTypeService.getListByBaseURI(this.baseUri).subscribe({
      next: response => {
        this.userProfileDetails = response;
        this.userName=this.userProfileDetails.profile.userName;
        this.baseUri=this.userProfileDetails.profile.baseURI;
        this.welcomeText =this.userProfileDetails.profile.welcomeText;
      },
      error: (error) => { console.log(error) },
      complete: () => { }
    })
  }
}
