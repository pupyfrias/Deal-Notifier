import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { ItemService } from 'src/app/services/item.service';

import { ResponseDTO } from '../../models/ResponseDTO';
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
    private itemService: ItemService,
    private authService: AuthService,
    private router: Router
  ) { }

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
      const userName = this.formGroup.get('username')?.value;
      const password = this.formGroup.get('password')?.value;
      this.authService.login(userName, password ).subscribe(response => {
        if(response){
          this.router.navigateByUrl('/');
        }
      });

    }
  }
}
