import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserEventTypesComponent } from './user-event-types.component';

describe('UserEventTypesComponent', () => {
  let component: UserEventTypesComponent;
  let fixture: ComponentFixture<UserEventTypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserEventTypesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserEventTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
