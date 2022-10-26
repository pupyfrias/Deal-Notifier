import { Injectable } from '@angular/core';
import { AES, enc } from 'crypto-js';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CryptService {

  constructor() { }

  Encrypt(data: string): string {
    try {
      return AES.encrypt(data, environment.encryptKey).toString();
    } catch {
      return '';
    }
  }

  Decrypt(data: string): string {
    try {
      return AES.decrypt(data, environment.encryptKey).toString(enc.Utf8);
    } catch {
      return '';
    }
  }
}
