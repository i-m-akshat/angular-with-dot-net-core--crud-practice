import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../shared/services/auth.service';
import { UserService } from '../../shared/services/user.service';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  fullName!: string;
  Email!: string;
  constructor(
    private router: Router,
    private toastr: ToastrService,
    private authService: AuthService,
    private userService: UserService
  ) {}
  ngOnInit(): void {
    this.userService.getUserProfile().subscribe({
      next: (res: any) => {
        this.fullName = res.fullName;
        this.Email = res.email;
      },
      error: (res) => console.log('error while retrieving user profile', res),
    });
  }
  logout = () => {
    this.authService.deleteToken();
    this.authService.setLoginStatus();
    this.router.navigate(['user/login']);
    this.toastr.success('Logout Successfully', 'Logout !');
  };
}
