import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AlertService, AuthService } from '../../../app-core';
import { Subject } from 'rxjs';

@Component({ templateUrl: './login.component.html' 
,styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  destroyed$: Subject<boolean> = new Subject<boolean>();
  form!: FormGroup;
  loading = false;
  submitted = false;
  loginError: string = "";
  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AuthService,
    private alertService: AlertService
  ) { }


  ngOnInit() {
    this.form = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  get f() { return this.form.controls; }

  onLogin(e: any) {
    e.preventDefault();
    this.submitted = true;
    
    if (this.form.invalid) {
      return;
    }
    this.loading = true;

    this.accountService.onLogin(
      {
        userId: this.f['username'].value,
        password: this.f['password'].value
      })
      .pipe(first())
      .subscribe({
        next: response => {
          if (response == undefined) {
            this.loginError = "Invalid username or password";
            return;
          }
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
          this.router.navigateByUrl(returnUrl);
        },
        error: error => {
          this.alertService.error(error);
        },
        complete: () => {
          this.loading = false;
        }
      });
  }

  onCancelLogin(e: any) {
    e.preventDefault();
    this.loading = false;
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
