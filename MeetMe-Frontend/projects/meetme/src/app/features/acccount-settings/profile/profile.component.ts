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

  isloading: boolean = false;

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
  }

  onSubmit(form: NgForm) {

    this.submitted = true;

    if (form.invalid) return;

    let command: IUpdateProfileCommand = {
      userName: this.model.userName, timeZone: this.model.timeZone, welcomeText: this.model.welcomeText
    }
    this.isloading = true;

    this.accountService.updateProfile(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertService.success("Save changes successfully!");
          this.updateComplete(response)
        },
        error: error => { CommonFunction.getErrorListAndShowIncorrectControls(form,error.error.errors)},
        complete: () => { 
          this.isloading = false;
        }
      });
  }


  onChangedTimezone(e: TimeZoneData) {
    console.log(e);
  }

  private updateComplete(response: IUpdateAccountSettingsResponse) {
    this.authService.resetToken(response.newToken);
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
