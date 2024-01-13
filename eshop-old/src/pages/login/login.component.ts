import { Component, ViewEncapsulation } from '@angular/core';
import { MaterialModule as MaterialModule } from '../../app/material/material.module';
import { Router } from '@angular/router';
import { FormControl, FormGroup, RequiredValidator } from '@angular/forms';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    MaterialModule
  ],
  encapsulation: ViewEncapsulation.ShadowDom,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  formGroup = new FormGroup({
    username: new FormControl(''),
    password: new FormControl('')
  })
  constructor(private router: Router) {

  }
  login() {
    console.log(this.formGroup.get("username")?.value, this.formGroup.get("password")?.value)
    if (this.formGroup.get("username")?.value == "admin" && this.formGroup.get("password")?.value == "admin") {
      sessionStorage.setItem("admin", JSON.stringify(true))
      this.router.navigateByUrl("/admin")
    }
    else if (this.formGroup.get("username")?.value == "shopper" && this.formGroup.get("password")?.value == "shopper") {
      sessionStorage.setItem("shopper", JSON.stringify(true))
      this.router.navigateByUrl("/shop")
    }
  }
}
