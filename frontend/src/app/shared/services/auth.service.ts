// Corrected AuthService
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'; // Remove OnInit
import { BehaviorSubject } from 'rxjs';
import { TOKEN_KEY } from '../constants';
import { Token } from '@angular/compiler';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // Remove implements OnInit
  isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  baseUrl: string = 'https://localhost:7024/Account';

  constructor(private httpClient: HttpClient) {
    // Call the method from the constructor instead
    this.setLoginStatus();
  }

  // ... rest of the code is the same

  createUser = (formData: any) => {
    formData.Gender = 'female';
    formData.role = 'Teacher';

    return this.httpClient.post(this.baseUrl + '/signup', formData);
  };

  signInUser = (formData: object) => {
    return this.httpClient.post(this.baseUrl + '/signin', formData);
  };

  saveToken(token: string) {
    localStorage.setItem(TOKEN_KEY, token);
    this.setLoginStatus();
  }

  deleteToken() {
    localStorage.removeItem(TOKEN_KEY);
    this.setLoginStatus();
  }
  getToken() {
    return localStorage.getItem(TOKEN_KEY);
  }
  setLoginStatus() {
    if (localStorage.getItem(TOKEN_KEY)) {
      this.isLoggedIn.next(true);
    } else {
      this.isLoggedIn.next(false);
    }
  }

  getLoginStatus() {
    return this.isLoggedIn.asObservable();
  }
  getLoginStatus_synchronous() {
    return this.isLoggedIn;
  }

  getClaims() {
    const payload = JSON.parse(window.atob(this.getToken()?.split('.')[1]!));
    return payload;
  }
}
