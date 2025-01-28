import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DPatientComponent } from './d-patient.component';

describe('DPatientComponent', () => {
  let component: DPatientComponent;
  let fixture: ComponentFixture<DPatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DPatientComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DPatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
