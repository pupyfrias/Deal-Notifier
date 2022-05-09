import { Router } from '@angular/router';
import { ItemService } from 'src/app/services/item.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
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
    private service: ItemService,
    private toastService: ToastrService,
    private router: Router
  ) {}

  get user_name() {
    return this.formGroup.get('user_name');
  }
  get password() {
    return this.formGroup.get('password');
  }

  ngOnInit(): void {

   const enc = this.service.Encrypt('pupy frias');
   const dec = this.service.Decrypt('U2FsdGVkX18INfZ7TRB+YcGTy+84mea6Emy48nEC4lr4=');
    console.log(enc)
    console.log(dec.length)

    this.formGroup = this.fb.group({
      user_name: ['', [Validators.required]],
      password: ['', [Validators.minLength(8), Validators.required]],
    });
  }

  Submit() {
    if (this.formGroup.valid) {
      const data = this.formGroup.value;
      this.service.Login(data).subscribe({
        next: (respon) => {
          const encryptToken = this.service.Encrypt(respon);
          sessionStorage.setItem('token', encryptToken);
          this.router.navigateByUrl('/');
        },
        error: (error) => {
          this.toastService.error(error?.error);
        },
      });
    }
  }
}
