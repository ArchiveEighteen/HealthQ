import { TestBed } from '@angular/core/testing';

import { QConstructorService } from './q-constructor.service';

describe('QConstructorService', () => {
  let service: QConstructorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(QConstructorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
