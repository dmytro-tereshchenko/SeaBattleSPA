import { TestBed, inject } from '@angular/core/testing';

import { DataApiService } from './data-api.service';

describe('TestApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DataApiService]
    });
  });

  it('should be created', inject([DataApiService], (service: DataApiService) => {
    expect(service).toBeTruthy();
  }));
});
