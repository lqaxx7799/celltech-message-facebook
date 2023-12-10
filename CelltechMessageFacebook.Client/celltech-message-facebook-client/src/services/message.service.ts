import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable()
export class MessageService{
  constructor(private httpClient: HttpClient) {}

  public list(params: any) : Observable<any> {
    return this.httpClient.get<any>('https://localhost:7297/message/list', { params });
  }

  public reply(payload: any) : Observable<any> {
    return this.httpClient.post<any>('https://localhost:7297/message/reply', payload);
  }
}
