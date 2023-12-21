import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventInfoModalComponent } from './event-info-modal.component';

describe('EventInfoComponent', () => {
  let component: EventInfoModalComponent;
  let fixture: ComponentFixture<EventInfoModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventInfoModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventInfoModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
