import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConstructorQuestionComponent } from './constructor-question.component';

describe('ConstructorQuestionComponent', () => {
  let component: ConstructorQuestionComponent;
  let fixture: ComponentFixture<ConstructorQuestionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConstructorQuestionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConstructorQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
