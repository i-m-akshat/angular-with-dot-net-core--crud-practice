import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../../shared/services/auth.service';
import { FirstKeyPipe } from '../../../shared/pipes/first-key.pipe';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [ReactiveFormsModule, FirstKeyPipe, RouterLink],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css',
})
export class RegistrationComponent implements OnInit {
  form;
  isSubmitted: boolean = false;

  //constructor
  constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private toaster: ToastrService,
    private authService: AuthService,
    private router: Router
  ) {
    this.form = this.formBuilder.group(
      {
        fullName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(6),
            Validators.pattern(
              '^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$'
            ),
          ],
        ],
        confirmPassword: [
          '',
          [
            Validators.required,
            Validators.minLength(6),
            Validators.pattern(
              '^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$'
            ),
          ],
        ],
      },
      { validators: this.passwordMatchValidator }
    );
  }
  ngOnInit(): void {
    this.authService.getLoginStatus().subscribe((x) => {
      if (x) {
        this.router.navigate(['/dashboard']);
      }
    });
  }
  //has displayable error
  hasDisplayableError = (controlName: string): Boolean => {
    const control = this.form.get(controlName);
    return (
      Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched || Boolean(control?.dirty)))
    );
  };

  //password mismatch validator=>nothing but a validator function accepting abstract control and then returning null inside we are setting the passwordMisMatch property to true or false
  passwordMatchValidator: ValidatorFn = (control: AbstractControl): null => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');
    if (
      password &&
      confirmPassword &&
      password.value != confirmPassword.value
    ) {
      confirmPassword?.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword?.setErrors(null);
    }
    return null;
  };

  //function to handle the submission of form

  handleSubmit() {
    this.isSubmitted = true;
    if (this.form.valid) {
      this.service.createUser(this.form.value).subscribe({
        next: (res: any) => {
          if (res.succeeded) {
            this.form.reset();
            this.isSubmitted = false;
            this.toaster.success(
              'New User Created !',
              'Registration Successfull'
            );
          } else {
          }
          console.log(res);
        },
        error: (error) => {
          if (error.error) {
            error.error.errors.forEach((error: any) => {
              switch (error.code) {
                case 'DuplicateUserName':
                  break;
                case 'DuplicateEmail':
                  this.toaster.error(
                    'Email is already taken',
                    'Registration failed !'
                  );
                  break;

                default:
                  this.toaster.error(
                    'Contact the developer',
                    'Registration failed !'
                  );
                  break;
              }
            });
          }
        },
      });
    }
    console.log(this.form.value);
  }
}
