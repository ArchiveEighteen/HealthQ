import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QTemplateSelectorComponent } from './q-template-selector.component';

describe('QTemplateSelectorComponent', () => {
  let component: QTemplateSelectorComponent;
  let fixture: ComponentFixture<QTemplateSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QTemplateSelectorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QTemplateSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
