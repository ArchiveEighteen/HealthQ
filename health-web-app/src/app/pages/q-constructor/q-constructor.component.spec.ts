import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QConstructorComponent } from './q-constructor.component';

describe('QConstructorComponent', () => {
  let component: QConstructorComponent;
  let fixture: ComponentFixture<QConstructorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QConstructorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QConstructorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
