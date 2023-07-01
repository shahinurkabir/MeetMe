import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth-service';
import { NgForm } from '@angular/forms';
import { AccountService } from '../../../services/account.service';
import { IUpdateUserLinkCommand } from '../../../interfaces/account-commands';
import { IUpdateAccountSettingsResponse } from '../../../interfaces/account-interfaces';

@Component({
  selector: 'app-link',
  templateUrl: './link.component.html',
  styleUrls: ['./link.component.scss']
})
export class LinkComponent implements OnInit {

  baseURI: string = "";
  submitted: boolean = false;
  timerTyping: any;
  typingInterval: number = 500;
  availabilityStatus: string = "";
  host: string = window.location.host;

  constructor(
    private authService: AuthService,
    private accountService: AccountService
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
    this.accountService.isLinkAvailable(link).subscribe({
      next: response => {
        console.log(response);
        this.availabilityStatus = 'Link is available';
      },
      error: error => {
        this.handleErrorResponse(error);
      },
      complete: () => { }

    });
  }

  onSubmit(form: NgForm) {

    this.submitted = true;

    if (form.invalid) return;

    let command: IUpdateUserLinkCommand = { baseURI: this.baseURI }

    this.accountService.updateLink(command).subscribe({
      next: response => {
        this.updateComplete(response)

      },
      error: error => { console.log(error) },
      complete: () => { }
    });

  }

  handleErrorResponse(error: any) {
    console.log(error); // TODO
    this.availabilityStatus = "";

    if (error.status != 400) {
      alert(error); // TODO
    }
    else {
      let validationErrors = error.error.errors;
      if (validationErrors && validationErrors.length > 0) {
        this.availabilityStatus = validationErrors[0].errorMessage;
      }
    }
  }
  onLinkChanged(e: any) {
    let value = e.target.value;
    e.target.value = value.replace(/[-\s]+/g, "-").replace(/^-/, '').replace(/[^a-zA-Z0-9àç_èéù-]+/g, "").toLowerCase();
  }
  private updateComplete(response: IUpdateAccountSettingsResponse) {
    this.authService.resetToken(response.newToken);
    this.updateFormFields();
    alert("Link updated successfylly") // TODO
  }

  private updateFormFields() {
    this.baseURI = this.authService.baseUri;
  }
}
