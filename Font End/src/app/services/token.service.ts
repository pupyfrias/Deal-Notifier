import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ResponseDTO } from '../models/ResponseDTO';

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

    /*    const refreshRes = await new Promise<AuthenticatedResponse>((resolve, reject) => {
          this.httpClient.post<AuthenticatedResponse>("https://localhost:5001/api/token/refresh", credentials, {
            headers: new HttpHeaders({
              "Content-Type": "application/json"
            })
          }).subscribe({
            next: (res: AuthenticatedResponse) => resolve(res),
            error: (_) => { reject; isRefreshSuccess = false; }
          });
        });*/

    const api = environment.baseApi + 'account/refresh-token';
    await this.httpClient.post<ResponseDTO>(api, { accessToken: token }, { withCredentials: true })
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
