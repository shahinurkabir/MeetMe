import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService, AccountService, IUpdateUserLinkCommand, IUpdateAccountSettingsResponse, CommonFunction, AlertService } from '../../../app-core';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-link',
  templateUrl: './link.component.html',
  styleUrls: ['./link.component.scss']
})
export class LinkComponent implements OnInit, OnDestroy {
  @ViewChild('formBaseURI') formBaseURI:NgForm | undefined;
  destroyed$: Subject<boolean> = new Subject<boolean>();
  base_URI: string = "";
  submitted: boolean = false;
  timerTyping: any;
  typingInterval: number = 500;
  availabilityStatus: string = "";
  host: string = window.location.host;
  isLoading: boolean = false;
  constructor(
    private authService: AuthService,
    private accountService: AccountService,
    private alertService: AlertService
  ) {

    this.updateFormFields();

  }


  ngOnInit(): void {
  }

  onCancel(e: any) {
    e.preventDefault();
    this.submitted = false;
    this.updateFormFields();
  }
  onKeyUp(e: any) {
    clearTimeout(this.timerTyping);
    let link: string = e.target.value;
    if (link.trim() == "") {
      this.availabilityStatus = "";
      return;
    }

    this.timerTyping = setTimeout(() => this.checkLinkAvailability(link), this.typingInterval);
  }

  onKeyDown(e: any) {
    clearTimeout(this.timerTyping);
  }


  checkLinkAvailability(link: string) {
    if (link.trim() == "") return;
    this.availabilityStatus = 'checking ...';
    this.accountService.isLinkAvailable(link)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response:any) => {
          this.availabilityStatus =`${link} ${response.isAvailable ? "available" : "not available"}`;
        },
        error: error => {
          CommonFunction.getErrorListAndShowIncorrectControls(this.formBaseURI?.controls, error.error.errors);
        },
        complete: () => { }

      });
  }

  onSubmit(form: NgForm) {

    this.submitted = true;

    if (form.invalid) return;

    let command: IUpdateUserLinkCommand = { baseURI: this.base_URI }
    this.isLoading = true;
    this.accountService.updateLink(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertService.success("Update link success");
          this.updateComplete(response)
        },
        error: error => {
          CommonFunction.getErrorListAndShowIncorrectControls(this.formBaseURI?.controls, error.error.errors);
        },
        complete: () => {this.isLoading=false }
      });

  }

  onLinkChanged(e: any) {
    let value = e.target.value;
    e.target.value = value.replace(/[-\s]+/g, "-").replace(/^-/, '').replace(/[^a-zA-Z0-9àç_èéù-]+/g, "").toLowerCase();
  }
  private updateComplete(response: IUpdateAccountSettingsResponse) {
    this.authService.resetToken(response.newToken);
    this.updateFormFields();
  }

  private updateFormFields() {
    this.base_URI = this.authService.baseUri;
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
