import { Component, Inject } from '@angular/core';
import { MaterialModule } from '../../../../app/material/material.module';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Client } from '../../../../domain/product';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-clients-dialog',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './clients-dialog.component.html',
  styleUrl: './clients-dialog.component.scss'
})
export class ClientsDialogComponent {
  formGroup: FormGroup
  constructor(@Inject(MAT_DIALOG_DATA) public client: Client, private ref: MatDialogRef<ClientsDialogComponent>) {
    this.formGroup = new FormGroup({
      firstName: new FormControl(client.firstName, [Validators.required]),
      lastName: new FormControl(client.lastName, [Validators.required]),
      dateOfBirth: new FormControl(client.dateOfBirth, [Validators.required]),
      idNumber: new FormControl(client.idNumber, [Validators.required]),
      personalCode: new FormControl(client.personalCode, [Validators.required]),
      ammountOfPoints: new FormControl(client.ammountOfPoints, [Validators.required, Validators.min(0)]),
    });

  }

  save() {

    if (this.isValid()) {

      this.client.firstName = this.formGroup.controls["firstName"].value
      this.client.lastName = this.formGroup.controls["lastName"].value
      this.client.dateOfBirth = this.formGroup.controls["dateOfBirth"].value
      this.client.idNumber = this.formGroup.controls["idNumber"].value
      this.client.personalCode = this.formGroup.controls["personalCode"].value
      this.client.ammountOfPoints = this.formGroup.controls["ammountOfPoints"].value
      this.ref.close(this.client);
    }
  }
  isValid(): boolean {

    Object.keys(this.formGroup.controls).forEach(field => {
      const control = this.formGroup.get(field)!;
      control.markAsTouched({ onlySelf: true });
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

}
