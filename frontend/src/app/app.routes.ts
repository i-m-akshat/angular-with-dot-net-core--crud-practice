import { Routes } from '@angular/router';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { UserComponent } from './components/user/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { authUserGuard } from './auth-user.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/user/login', pathMatch: 'full' },
  {
    path: 'user',
    component: UserComponent,
    children: [
      { path: 'sign-up', component: RegistrationComponent },
      { path: 'login', component: LoginComponent },
    ],
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authUserGuard],
  },
];
