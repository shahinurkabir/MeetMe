import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventInfoUpdateComponent } from './event-info-update.component';

describe('EventInfoUpdateComponent', () => {
  let component: EventInfoUpdateComponent;
  let fixture: ComponentFixture<EventInfoUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventInfoUpdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventInfoUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
