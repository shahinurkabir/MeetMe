import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSettingsShellComponent } from './account-settings-shell.component';

describe('AccountSettingsShellComponent', () => {
  let component: AccountSettingsShellComponent;
  let fixture: ComponentFixture<AccountSettingsShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountSettingsShellComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AccountSettingsShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
