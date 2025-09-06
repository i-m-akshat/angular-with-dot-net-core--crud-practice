import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
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
          console.log(res);
          localStorage.setItem('jwt', res.token);
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
