import {Component} from '@angular/core';
import {UntypedFormBuilder, UntypedFormGroup, Validators} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';
import {AuthService} from 'src/app/services/auth.service';
import {BlogRouterService} from 'src/app/services/blog-router.service';

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

  loginForm: UntypedFormGroup;

  constructor(
    private blogRouter: BlogRouterService,
    private authService: AuthService,
    private toastrService: ToastrService,
    private formBuilder: UntypedFormBuilder) {
    this.loginForm = formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  cancel(): void {
    this.blogRouter.goToBlog();
  }

  login(): void {
    const formValues = this.loginForm.value;
    if (!formValues?.password || !formValues?.username) {
      return;
    }

    this.authService
      .login(formValues.username, formValues.password)
      .subscribe(response => {
        if (!response) {
          this.loginInvalid = true;
        } else {
          this.toastrService.success('Logged successfully!');
          this.blogRouter.goToBlog();
        }
      });
  }

  isValidUserName(mouseoverLogin: boolean): boolean {
    return this.loginForm.controls.username?.invalid && (this.loginForm.controls.username?.touched || mouseoverLogin);
  }

  isValidPassword(mouseoverLogin): boolean {
    return this.loginForm.controls.password?.invalid && (this.loginForm.controls.password?.touched || mouseoverLogin);
  }
}
