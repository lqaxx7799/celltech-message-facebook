import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable()
export class FacebookService{
  constructor(private httpClient: HttpClient) {}

  public getPages(userId: string) : Observable<any> {
    const params = new HttpParams({
      fromObject: {
        userId
      },
    });
    return this.httpClient.get<any>('https://localhost:7279/facebook/pages', { params });
  }
}
