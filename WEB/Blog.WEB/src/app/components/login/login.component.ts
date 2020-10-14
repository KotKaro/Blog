import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent {
  userName: object;
  password: object;
  mouseoverLogin: boolean;
  loginInvalid = false;

  constructor(private router: Router, private authService: AuthService, private toastrService: ToastrService) {

  }

  cancel(): void {
    this.router.navigate(['/blog']);
  }

  login(formValues: any): void {
    if (!formValues?.password || !formValues?.userName) {
      return;
    }

    this.authService
      .login(formValues.userName, formValues.password)
      .subscribe(response => {
        if (!response){
            this.loginInvalid = true;
        } else {
            this.toastrService.success('Logged successfully!');
            this.router.navigate(['/blog']);
        }
    });
  }

  isValidUserName(loginForm: NgForm, mouseoverLogin: boolean): boolean {
    return loginForm.controls.userName?.invalid && (loginForm.controls.userName?.touched || mouseoverLogin);
  }

  isValidPassword(loginForm: NgForm, mouseoverLogin): boolean {
    return loginForm.controls.password?.invalid && (loginForm.controls.password?.touched || mouseoverLogin);
  }
}
