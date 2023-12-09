import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable()
export class UserService{
  constructor(private httpClient: HttpClient) {}

  public signUp(payload: any) : Observable<any> {
    return this.httpClient.post<any>('https://localhost:7297/user/signUp', payload);
  }
}
