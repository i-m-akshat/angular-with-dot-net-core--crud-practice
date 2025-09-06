import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent {
  constructor(private router: Router, private toastr: ToastrService) {}
  logout = () => {
    localStorage.removeItem('jwt');
    this.router.navigate(['user/login']);
    this.toastr.success('Logout Successfully', 'Logout !');
  };
}
