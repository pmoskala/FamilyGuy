import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {FgErrorStateMatcher} from '@app/_common/FgErrorStateMatcher';
import {UserService} from '@app/_services';
import {RegisterUser} from '@app/_models/register-user';
import * as uuid from 'uuid';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})

export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  private readonly passwordMinLength = 8;
  matcher = new FgErrorStateMatcher();

  constructor( private formBuilder: FormBuilder,
               private userService: UserService,
               private route: ActivatedRoute,
               private router: Router) { }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      telephone: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(this.passwordMinLength)]],
      confirmPassword: ['']
    }, {validator: this.checkPasswords});
  }

  checkPasswords(group: FormGroup) {
    const pass = group.get('password').value;
    const confirmPass = group.get('confirmPassword').value;

    return pass === confirmPass ? null : { notSame: true };
  }

  public hasError = (controlName: string, errorName: string) => {
    return this.registerForm.controls[controlName].hasError(errorName);
  }

  public formHasError = (errorName: string) => {
    if (this.registerForm.errors) {
      return this.registerForm.errors.hasOwnProperty(errorName);
    }
    return false;
  }

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }
    const {username, firstName, lastName, email, telephone, password} = this.registerForm.value;

    const registeredUser: RegisterUser = {
      id: uuid.v4(),
      userName: username,
      email,
      firstName,
      lastName,
      telephoneNumber: telephone,
      password
    };

    this.userService.register(registeredUser).subscribe(() => {

    }, error => {
      console.log('Registration error');
      }
    );
  }
}

