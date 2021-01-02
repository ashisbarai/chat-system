import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Constants } from 'src/app/constants';


@Injectable()
export class HomeService {
    constructor(private _httpClient: HttpClient) { }
    
    //getNetworkInfos(): Observable<INetworkInfo[]> {
       // return this._httpClient.get<INetworkInfo[]>(Constants.apiRoot + 'getnetworkinfos');
    //}
}