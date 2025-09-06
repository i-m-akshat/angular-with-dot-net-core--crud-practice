import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl: string = 'https://localhost:7024/Account';
  constructor(private httpClient: HttpClient) {}

  createUser = (formData: any) => {
    return this.httpClient.post(this.baseUrl + '/signup', formData);
  };
}
