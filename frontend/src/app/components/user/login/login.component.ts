import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { TOKEN_KEY } from '../../../shared/constants';
@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  form;
  isSubmitted: boolean = false;
  constructor(
    public formBuilder: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }
  ngOnInit(): void {
    this.authService.getLoginStatus().subscribe((x) => {
      if (x) {
        console.log(x);
        this.router.navigate(['/dashboard']);
      }
    });
  }
  hasDisplayableError = (control: FormControl): Boolean => {
    return (
      Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched || Boolean(control?.dirty)))
    );
  };
  handleLogin() {
    this.isSubmitted = true;
    if (this.form.valid) {
      this.authService.signInUser(this.form.value).subscribe({
        next: (res: any) => {
          this.authService.saveToken(res.token);
          this.router.navigate(['dashboard']);
          this.toastr.success('Logged in successfully', 'Login Successfull!', {
            closeButton: true,
            newestOnTop: true,
          });
        },
        error: (error: any) => {
          if (error.status === 400) {
            console.log(error);
            this.toastr.error(error.error.message, 'Error !');
          }
        },
      });
    }
  }
}
