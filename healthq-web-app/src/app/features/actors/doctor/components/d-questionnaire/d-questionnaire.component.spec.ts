import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DQuestionnaireComponent } from './d-questionnaire.component';

describe('DQuestionnaireComponent', () => {
  let component: DQuestionnaireComponent;
  let fixture: ComponentFixture<DQuestionnaireComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DQuestionnaireComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DQuestionnaireComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
