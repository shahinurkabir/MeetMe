import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { TimeZoneData, ListOfTimeZone, IAccountProfileInfo, AccountService, AuthService, IUpdateProfileCommand, IUpdateAccountSettingsResponse } from '../../../app-core';
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
  constructor(
    private accountService: AccountService,
    private authService: AuthService
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
        error: error => { console.log(error) },// TODO: Display error message
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

    this.accountService.updateProfile(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.updateComplete(response)
        },
        error: error => { console.log(error) },// TODO: Display error message
        complete: () => { }
      });
  }

  onCancel(e: any) {
    e.preventDefault();
    this.submitted = false;
    this.loadData();
  }

  onChangedTimezone(e: TimeZoneData) {
    console.log(e);
  }

  private updateComplete(response: IUpdateAccountSettingsResponse) {
    this.authService.resetToken(response.newToken);
    alert("Link updated successfylly") // TODO
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
