import { TestBed } from '@angular/core/testing';

import { DataStartFieldService } from './data-start-field.service';

describe('DataStartFieldService', () => {
  let service: DataStartFieldService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataStartFieldService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
