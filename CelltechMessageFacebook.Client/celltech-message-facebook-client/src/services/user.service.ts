import { Injectable } from '@angular/core';

@Injectable()
export class UserService{
  constructor(private httpClient: HttpClient) {
  }
}
