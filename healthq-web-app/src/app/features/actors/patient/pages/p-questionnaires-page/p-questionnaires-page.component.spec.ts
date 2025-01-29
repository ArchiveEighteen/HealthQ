import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PQuestionnairesPageComponent } from './p-questionnaires-page.component';

describe('PQuestionnairesPageComponent', () => {
  let component: PQuestionnairesPageComponent;
  let fixture: ComponentFixture<PQuestionnairesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PQuestionnairesPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PQuestionnairesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
