import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../shared/services/auth.service';
import { UserService } from '../../shared/services/user.service';
import { RouterLink } from '@angular/router';
import { HideIfClaimsNotMetDirective } from '../../directives/hide-if-claims-not-met.directive';
import { claimReq } from '../../shared/utils/claimReq';
@Component({
  selector: 'app-dashboard',
  imports: [RouterLink, HideIfClaimsNotMetDirective],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  claimReq = claimReq;
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
