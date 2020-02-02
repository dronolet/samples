import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from './main.component';
import { MainResolve } from './main.resolver.component';


const routes: Routes = [
  {
    path: '', component: MainComponent,
    resolve: {
      userinfo: MainResolve
    }
  }
  //{
  //  path: 'flat/:id', component: EditRecordComponent, resolve: {
  //    currentFlat: FlatObjectResolve
  //  }
  //}
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class FlatRoutingModule { }
