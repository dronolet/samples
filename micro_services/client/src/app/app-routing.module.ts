import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppCompanyComponent } from './company/company';
import { AppEmploeeComponent } from './emploee/emploee';

const routes: Routes = [
  {
    path: 'company',
    component: AppCompanyComponent
  },
  {
    path: 'emploee',
    component: AppEmploeeComponent
  },
  {
    path: '',
    component: AppEmploeeComponent
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
