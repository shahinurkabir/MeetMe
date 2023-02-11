import { TestBed } from '@angular/core/testing';

import { WorkinghourService } from './workinghour.service';

describe('WorkinghourService', () => {
  let service: WorkinghourService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WorkinghourService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
