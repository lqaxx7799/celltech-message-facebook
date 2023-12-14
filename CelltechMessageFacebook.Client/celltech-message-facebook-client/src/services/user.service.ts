import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {BehaviorSubject, Observable} from "rxjs";

@Injectable()
export class UserService{
  public currentUser$ = new BehaviorSubject<any>(null);

  constructor(private httpClient: HttpClient) {}

  public get(id: string) : Observable<any> {
    const params = new HttpParams({
      fromObject: {
        id
      },
    });
    return this.httpClient.get<any>('https://localhost:7297/user/get', { params });
  }

  public signUp(payload: any) : Observable<any> {
    return this.httpClient.post<any>('https://localhost:7297/user/signUp', payload);
  }
}
