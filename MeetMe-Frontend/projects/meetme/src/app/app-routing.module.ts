import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './features/users/login/login.component';
import { WorkinghoursComponent } from './features/workinghours/workinghours.component';
import { AuthGuard } from './gurads/auth-gurad';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  // {
  //   path: "", component: HomeComponent,
  //   canActivate: [AuthGuard]
  // },
  {
    path: "", redirectTo: "event-types", pathMatch:"full"
  },
  { path: "login", component: LoginComponent },
  { path: "event-types", loadChildren: () => import("./features/event-types/event-type.module").then(m => m.EventTypeModule), canActivate: [AuthGuard] },
  { path: "working-hours", component: WorkinghoursComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
