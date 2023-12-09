import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable()
export class FacebookService{
  constructor(private httpClient: HttpClient) {}

  public getPages(accessToken: string) : Observable<any> {
    const params = new HttpParams({
      fromObject: {
        accessToken
      },
    });
    return this.httpClient.get<any>('https://localhost:7297/facebook/pages', { params });
  }
}
