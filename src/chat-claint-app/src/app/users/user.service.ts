import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Constants } from '../constants';
import { IUserInfo, IUserInfoDto } from './user-info';


@Injectable()
export class UserService {
    constructor(private _httpClient: HttpClient) { }
    
    createUser(user: IUserInfo): Observable<IUserInfoDto> {
        return this._httpClient.post<IUserInfoDto>(Constants.apiRoot + 'createuser', user);
    }
    getALlUsers(): Observable<IUserInfoDto[]> {
        return this._httpClient.get<IUserInfoDto[]>(Constants.apiRoot + 'GetAllUsers');
    }
    getAllFriendsByUserId(userId: number): Observable<IUserInfoDto[]> {
        return this._httpClient.get<IUserInfoDto[]>(Constants.apiRoot + 'GetAllFriendsByUserId/' + userId);
    }
    getUserById(userId: number): Observable<IUserInfoDto> {
        return this._httpClient.get<IUserInfoDto>(Constants.apiRoot + 'GetUserById/' + userId);
    }
}