import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DMainPageComponent } from './d-main-page.component';

describe('DMainPageComponent', () => {
  let component: DMainPageComponent;
  let fixture: ComponentFixture<DMainPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DMainPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DMainPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
