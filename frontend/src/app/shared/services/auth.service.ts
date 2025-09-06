import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  baseUrl: string = 'https://localhost:7024/Account';
  constructor(private httpClient: HttpClient) {}

  createUser = (formData: any) => {
    return this.httpClient.post(this.baseUrl + '/signup', formData);
  };

  signInUser = (formData: object) => {
    return this.httpClient.post(this.baseUrl + '/signin', formData);
  };

  setLoginStatus() {
    if (localStorage.getItem('jwt')) {
      this.isLoggedIn.next(true);
    }
  }
  getLoginStatus() {
    return this.isLoggedIn.asObservable();
  }
}
