import { TestBed } from '@angular/core/testing';

import { DataShipService } from './data-ship.service';

describe('DataShipService', () => {
  let service: DataShipService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataShipService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
