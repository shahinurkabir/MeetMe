import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventTypeShellComponent } from './event-type-shell.component';

describe('EventtypeComponent', () => {
  let component: EventTypeShellComponent;
  let fixture: ComponentFixture<EventTypeShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventTypeShellComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventTypeShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
