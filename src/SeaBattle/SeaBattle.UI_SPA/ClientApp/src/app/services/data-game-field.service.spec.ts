import { TestBed } from '@angular/core/testing';

import { DataGameFieldService } from './data-game-field.service';

describe('DataGameFieldService', () => {
  let service: DataGameFieldService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataGameFieldService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
