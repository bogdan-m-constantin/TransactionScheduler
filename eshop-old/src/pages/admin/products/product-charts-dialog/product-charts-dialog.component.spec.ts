import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductChartsDialogComponent } from './product-charts-dialog.component';

describe('ProductChartsDialogComponent', () => {
  let component: ProductChartsDialogComponent;
  let fixture: ComponentFixture<ProductChartsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductChartsDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProductChartsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
