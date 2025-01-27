import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PMainPageComponent } from './p-main-page.component';

describe('PMainPageComponent', () => {
  let component: PMainPageComponent;
  let fixture: ComponentFixture<PMainPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PMainPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PMainPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
