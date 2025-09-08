import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(
    private httpClient: HttpClient,
    private authService: AuthService
  ) {}

  getUserProfile = () => {
    // const token = this.authService.getToken();
    // const reqHeader = new HttpHeaders({ Authorization: 'Bearer ' + token });
    // return this.httpClient.get(`${environment.apiBaseUrl}/UserProfile`, {
    //   headers: reqHeader,
    // });

    return this.httpClient.get(`${environment.apiBaseUrl}/UserProfile`);
  };
}
