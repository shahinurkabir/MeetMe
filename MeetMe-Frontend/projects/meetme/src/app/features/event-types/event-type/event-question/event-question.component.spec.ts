import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventQuestionComponent } from './event-question.component';

describe('EventQuestionComponent', () => {
  let component: EventQuestionComponent;
  let fixture: ComponentFixture<EventQuestionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventQuestionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EventQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
