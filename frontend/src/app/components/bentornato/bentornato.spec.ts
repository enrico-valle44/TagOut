import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Bentornato } from './bentornato';

describe('Bentornato', () => {
  let component: Bentornato;
  let fixture: ComponentFixture<Bentornato>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Bentornato]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Bentornato);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
