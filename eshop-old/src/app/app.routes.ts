import { CanActivateFn, Routes } from '@angular/router';
import { LoginComponent } from '../pages/login/login.component';
import { ShopComponent } from '../pages/shop/shop.component';
import { AdminComponent } from '../pages/admin/admin.component';

export const adminGuard: CanActivateFn = (route, state): boolean => {
  console.log("checking route guard")
  return (JSON.parse(sessionStorage.getItem("admin") ?? "false")) == true;
}
export const userGuard: CanActivateFn = (route, state): boolean => {
  console.log("checking route guard")
  return (JSON.parse(sessionStorage.getItem("shopper") ?? "false")) == true;
}

export const routes: Routes = [
  { path: "admin", component: AdminComponent, canActivate: [adminGuard] },
  { path: "shop", component: ShopComponent, canActivate: [userGuard] },
  { path: "**", component: LoginComponent }
];
