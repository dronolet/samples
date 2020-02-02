import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  LoginComponent,
  RememberComponent
} from './';
import { FlatModuleNgFactory } from './flat/flat.module.test'
//import { FlatModule } from './flat/flat.module';


export function loadMainModule() {
  return FlatModuleNgFactory;
}

const routes: Routes = [
  {
    path: '',
    loadChildren: loadMainModule// () => import('./flat/flat.module').then(mod => mod.FlatModule)
  },
  {
    path: 'login', component: LoginComponent  //login
  },
  {
    path: 'remember', component: RememberComponent 
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
];

@NgModule({
  declarations: [
    LoginComponent,
    RememberComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule],
  providers: []
})
export class AppRoutingModule { }
