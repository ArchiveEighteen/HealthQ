import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DTemplateComponent } from './d-template.component';

describe('DTemplateComponent', () => {
  let component: DTemplateComponent;
  let fixture: ComponentFixture<DTemplateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DTemplateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
