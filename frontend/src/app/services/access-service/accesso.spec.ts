import { TestBed } from '@angular/core/testing';

import { Accesso } from './accesso-service';

describe('Accesso', () => {
  let service: Accesso;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Accesso);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
