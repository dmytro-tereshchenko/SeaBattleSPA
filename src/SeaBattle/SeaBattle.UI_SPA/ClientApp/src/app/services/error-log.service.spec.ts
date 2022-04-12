import { TestBed } from '@angular/core/testing';

import { ErrorLogService } from './error-log.service';

describe('ErrorLogService', () => {
  let service: ErrorLogService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ErrorLogService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
