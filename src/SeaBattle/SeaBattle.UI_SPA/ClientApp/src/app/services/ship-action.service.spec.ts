import { TestBed } from '@angular/core/testing';

import { ShipActionService } from './ship-action.service';

describe('ShipActionService', () => {
  let service: ShipActionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShipActionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
