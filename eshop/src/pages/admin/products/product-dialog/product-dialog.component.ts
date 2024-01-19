import { FormControl, FormGroup, RequiredValidator, Validators } from '@angular/forms';
import { Component, Inject } from '@angular/core';
import { Product } from '../../../../domain/product';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from '../../../../app/material/material.module';

@Component({
  selector: 'app-product-dialog',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './product-dialog.component.html',
  styleUrl: './product-dialog.component.scss'
})
export class ProductDialogComponent {
  error: string = "";
  formGroup: FormGroup
  constructor(@Inject(MAT_DIALOG_DATA) public product: Product, private ref: MatDialogRef<ProductDialogComponent>) {
    this.formGroup = new FormGroup({
      name: new FormControl(product.name, [Validators.required]),
      description: new FormControl(product.description, [Validators.required]),
      price: new FormControl(product.price, [Validators.required, Validators.min(0)]),
      stock: new FormControl(product.stock, [Validators.required, Validators.min(0)]),

    });

  }

  save() {

    if (this.isValid()) {
      this.product.name = this.formGroup.controls["name"].value;
      this.product.description = this.formGroup.controls["description"].value;
      this.product.price = this.formGroup.controls["price"].value;
      this.product.stock = this.formGroup.controls["stock"].value;
      this.ref.close(this.product);
    }
  }
  isValid(): boolean {

    Object.keys(this.formGroup.controls).forEach(field => { // {1}
      const control = this.formGroup.get(field)!;            // {2}
      control.markAsTouched({ onlySelf: true });       // {3}
    });
    return this.formGroup.valid;
  }
  close() {
    this.ref.close();
  }
  getErrorMessage(control: string) {
    if (this.formGroup.controls[control].hasError('required')) {
      return 'You must enter a value';
    }
    if (this.formGroup.controls[control].hasError('min')) {
      return 'Value should be >= 0';
    }
    return "";
  }
  private files = []
  setImage(file: File) {

    if (!file) return;

    const FR = new FileReader();

    FR.addEventListener("load", (evt) => {
      this.product.image = evt.target?.result + "";
    });

    FR.readAsDataURL(file);

  }


}
