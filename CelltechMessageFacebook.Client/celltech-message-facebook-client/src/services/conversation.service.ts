import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable()
export class ConversationService{
  constructor(private httpClient: HttpClient) {}

  public list(params: any) : Observable<any[]> {
    return this.httpClient.get<any[]>('https://localhost:7297/conversation/list', { params });
  }

  public get(params: any) : Observable<any> {
    return this.httpClient.get<any>('https://localhost:7297/conversation/get', { params });
  }

}
