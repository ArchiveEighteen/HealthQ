import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PDoctorsPageComponent } from './p-doctors-page.component';

describe('PDoctorsPageComponent', () => {
  let component: PDoctorsPageComponent;
  let fixture: ComponentFixture<PDoctorsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PDoctorsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PDoctorsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
