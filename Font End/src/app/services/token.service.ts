import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { AuthenticatedResponse } from '../models/AuthenticatedResponse';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private httpClient: HttpClient) { }

  private async tryRefreshingTokens(token: string): Promise<boolean> {
    // Try refreshing tokens using refresh token
    const refreshToken: string | null = localStorage.getItem("refreshToken");
    if (!token || !refreshToken) {
      return false;
    }

    const credentials = JSON.stringify({ accessToken: token, refreshToken: refreshToken });
    let isRefreshSuccess: boolean;

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

    const api = environment.baseApi + 'account/authenticate';
    this.httpClient.post(api, data, { responseType: 'text' });

    localStorage.setItem("jwt", refreshRes.token);
    localStorage.setItem("refreshToken", refreshRes.refreshToken);
    isRefreshSuccess = true;

    return isRefreshSuccess;
  }
}
