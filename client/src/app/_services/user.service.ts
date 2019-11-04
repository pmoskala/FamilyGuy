import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '@environments/environment';
import { User } from '@app/_models';
import {RegisterUser} from "@app/_models/register-user";

@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<User[]>(`${environment.versionedApiUrl}/accounts`);
    }

    register(registerUser: RegisterUser) {
      return this.http.post(`${environment.versionedApiUrl}/accounts`, registerUser);
    }
}
