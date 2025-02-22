import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AMainPageComponent } from './a-main-page.component';

describe('AMainPageComponent', () => {
  let component: AMainPageComponent;
  let fixture: ComponentFixture<AMainPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AMainPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AMainPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
