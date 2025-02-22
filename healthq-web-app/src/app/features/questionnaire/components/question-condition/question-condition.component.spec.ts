import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionConditionComponent } from './question-condition.component';

describe('QuestionConditionComponent', () => {
  let component: QuestionConditionComponent;
  let fixture: ComponentFixture<QuestionConditionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuestionConditionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuestionConditionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
