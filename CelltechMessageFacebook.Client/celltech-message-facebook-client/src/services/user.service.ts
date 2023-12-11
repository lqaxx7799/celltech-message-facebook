import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {BehaviorSubject, Observable} from "rxjs";

@Injectable()
export class UserService{
  public currentUser$ = new BehaviorSubject<any>(null);

  constructor(private httpClient: HttpClient) {}

  public signUp(payload: any) : Observable<any> {
    return this.httpClient.post<any>('https://localhost:7297/user/signUp', payload);
  }
}
