import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ADoctorProfilePageComponent } from './a-doctor-profile-page.component';

describe('ADoctorProfilePageComponent', () => {
  let component: ADoctorProfilePageComponent;
  let fixture: ComponentFixture<ADoctorProfilePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ADoctorProfilePageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ADoctorProfilePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
