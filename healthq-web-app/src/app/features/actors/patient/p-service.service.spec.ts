import { TestBed } from '@angular/core/testing';

import { PServiceService } from './p-service.service';

describe('PServiceService', () => {
  let service: PServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
