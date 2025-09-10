import { Routes } from '@angular/router';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { UserComponent } from './components/user/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { authUserGuard } from './auth-user.guard';
import { AdminOnlyComponent } from './authorizeDemo/admin-only/admin-only.component';
import { AdminOrTeacherComponent } from './authorizeDemo/admin-or-teacher/admin-or-teacher.component';
import { ForbiddenComponent } from './errorPages/forbidden/forbidden.component';
import { claimReq } from './shared/utils/claimReq';

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
  //if the elements are in child components then  u need to pass the auth guard to canActivateChild like canActivateChild:authGuard or something like that

  {
    path: 'adminOnly',
    component: AdminOnlyComponent,
    canActivate: [authUserGuard],
    data: { claimReq: claimReq.adminOnly },
  },
  {
    path: 'adminOrTeacher',
    component: AdminOrTeacherComponent,
    canActivate: [authUserGuard],
    data: { claimReq: claimReq.adminOrTeacherOnly },
  },
  {
    path: 'forbidden',
    component: ForbiddenComponent,
  },
];
