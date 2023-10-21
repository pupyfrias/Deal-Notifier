import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, mapTo, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ResponseDTO } from '../models/ResponseDTO';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenExpiration: number;
  private refreshingTokenSource = new BehaviorSubject<boolean>(false);
  public refreshingToken$ = this.refreshingTokenSource.asObservable();

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<boolean> {
    return this.http.post<ResponseDTO>(environment.baseApi + 'account/login', { username, password }).pipe(
      tap(response => {
        this.setAccessToken(response.data.accessToken);
        this.tokenExpiration = Date.now() + response.data.expiresIn * 60000;
      }),
      mapTo(true),
      catchError(error => {
        console.error('Error de autenticación:', error);
        return of(false);
      })
    );
  }

  refreshToken(): Observable<any> {
    this.refreshingTokenSource.next(true);
    return this.http.post<any>(environment.baseApi + 'account/refresh-token', { accessToken: this.getAccessToken() })
      .pipe(
        tap(response => {
          this.setAccessToken(response.accessToken);
          this.tokenExpiration = Date.now() + response.expiresIn * 60000;
          this.refreshingTokenSource.next(false);
        }),
        catchError(error => {
          console.error('Error al actualizar el token:', error);
          return of(null);
        })
      );
  }

  logout(): void {
    localStorage.removeItem('accessToken')
    this.tokenExpiration = 0;
  }

  isAuthenticated(): boolean {
    return !!this.getAccessToken();
  }

  getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  setAccessToken(token: string): void {
    localStorage.setItem('accessToken', token);
  }

  setTokenExpiration(expiration: number): void {
    this.tokenExpiration = Date.now() + expiration * 60000;
  }

  getTokenExpiration(): number {
    return this.tokenExpiration;
  }
}
