import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DPatientsPageComponent } from './d-patients-page.component';

describe('DPatientsPageComponent', () => {
  let component: DPatientsPageComponent;
  let fixture: ComponentFixture<DPatientsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DPatientsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DPatientsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
