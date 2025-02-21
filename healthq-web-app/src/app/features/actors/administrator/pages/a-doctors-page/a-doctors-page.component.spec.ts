import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ADoctorsPageComponent } from './a-doctors-page.component';

describe('ADoctorsPageComponent', () => {
  let component: ADoctorsPageComponent;
  let fixture: ComponentFixture<ADoctorsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ADoctorsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ADoctorsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
