import { TestBed } from '@angular/core/testing';

import { DataGameService } from './data-game.service';

describe('DataGameService', () => {
  let service: DataGameService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataGameService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
