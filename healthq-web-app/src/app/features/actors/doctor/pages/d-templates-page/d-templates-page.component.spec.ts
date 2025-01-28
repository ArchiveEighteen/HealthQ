import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DTemplatesPageComponent } from './d-templates-page.component';

describe('DTemplatesPageComponent', () => {
  let component: DTemplatesPageComponent;
  let fixture: ComponentFixture<DTemplatesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DTemplatesPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DTemplatesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
