import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ResponseDTO } from '../models/ResponseDTO';
import { RequestOptions, Headers } from '@angular/http';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  constructor(private httpClient: HttpClient) { }

  public async tryRefreshingTokens(token: string | null): Promise<boolean> {
    if (!token) {
      return false;
    }

    let isRefreshSuccess: boolean = true;
    let options = new RequestOptions({ withCredentials: true });

    const api = environment.baseApi + 'account/refresh-token';
    await this.httpClient.post<ResponseDTO>(api, { accessToken: token })
      .toPromise()
      .then((response) => {
        sessionStorage.setItem('accesstoken', response?.data.accessToken);
      })
      .catch(() => {
        isRefreshSuccess = false;
      });

    return isRefreshSuccess;
  }
}
