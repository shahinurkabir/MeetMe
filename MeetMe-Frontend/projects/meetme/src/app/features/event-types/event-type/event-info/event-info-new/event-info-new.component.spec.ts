import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventInfoNewComponent } from './event-info-new.component';

describe('EventInfoNewComponent', () => {
  let component: EventInfoNewComponent;
  let fixture: ComponentFixture<EventInfoNewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventInfoNewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventInfoNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
