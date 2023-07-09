import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { TimeZoneData, ListOfTimeZone, IAccountProfileInfo, AccountService, AuthService, IUpdateProfileCommand, IUpdateAccountSettingsResponse, AlertService, CommonFunction } from '../../../app-core';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit, OnDestroy {
  destroyed$: Subject<boolean> = new Subject<boolean>();
  submitted: boolean = false;
  timeZoneList: TimeZoneData[] = ListOfTimeZone;
  model: IAccountProfileInfo = {
    id: "",
    userName: "",
    timeZone: "",
    baseURI: "",
    welcomeText: ""
  };
  selectedTimeZone: TimeZoneData |undefined;
  isLoading: boolean = false;

  constructor(
    private accountService: AccountService,
    private authService: AuthService,
    private alertService:AlertService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.accountService.getProfile()
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.loadProfileDataComplete(response);
        },
        error: error => { console.log(error) },
        complete: () => { }
      });
  }

  private loadProfileDataComplete(response: IAccountProfileInfo) {
    this.model = response;
    this.selectedTimeZone = this.timeZoneList.find(x => x.name == response.timeZone);
  }

  onSubmit(form: NgForm) {

    this.submitted = true;

    if (form.invalid) return;
    if (this.model.timeZone == undefined) return;
    let command: IUpdateProfileCommand = {
      userName: this.model.userName, timeZone: this.selectedTimeZone?.name!, welcomeText: this.model.welcomeText
    }
    this.isLoading = true;

    this.accountService.updateProfile(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertService.success("Save changes successfully!");
          this.updateComplete(response)
        },
        error: error => { CommonFunction.getErrorListAndShowIncorrectControls(form,error.error.errors)},
        complete: () => { 
          this.isLoading = false;
        }
      });
  }


  onTimeZoneChanged(e: TimeZoneData) {
    this.selectedTimeZone = e;
  }

  private updateComplete(response: IUpdateAccountSettingsResponse) {
    this.authService.resetToken(response.newToken);
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
