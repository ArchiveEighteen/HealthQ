import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PQuestionnaireComponent } from './p-questionnaire.component';

describe('PQuestionnaireComponent', () => {
  let component: PQuestionnaireComponent;
  let fixture: ComponentFixture<PQuestionnaireComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PQuestionnaireComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PQuestionnaireComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
