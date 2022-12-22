import { Router } from '@angular/router';
import { ItemService } from 'src/app/services/item.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { CryptService } from 'src/app/services/crypt.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  public formGroup: FormGroup;
  public hide = true;
  constructor(
    private fb: FormBuilder,
    private cryptService: CryptService,
    private itemService: ItemService,
    private router: Router
  ) {}

  get user_name() {
    return this.formGroup.get('username');
  }
  get password() {
    return this.formGroup.get('password');
  }

  ngOnInit(): void {

    this.formGroup = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.minLength(8), Validators.required]],
    });
  }

  Submit() {
    if (this.formGroup.valid) {
      const data = this.formGroup.value;
      this.itemService.Login(data).subscribe({
        next: (response) => {

          var parsedResponse = JSON.parse(response);
          var accessToken = parsedResponse.data.accessToken;

          const encryptToken = this.cryptService.Encrypt("mkjkjj");
          sessionStorage.setItem('token', encryptToken);
          this.router.navigateByUrl('/');
        }
      });
    }
  }
}
