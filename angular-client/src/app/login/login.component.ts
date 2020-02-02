import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { AuthService } from '../core';
import { NgForm } from '@angular/forms';


@Component({
  templateUrl: 'login.component.html',
  styleUrls: ['login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private location: Location,
    private authservice: AuthService
  ) {}

  ngOnInit() {

    this.authservice.signOut().subscribe(
       data => {},
       error => {        
        console.log('logout error');
      }
    );

  }

  onSubmit(form: NgForm) {
    
    this.authservice.sign({ login: form.value.login, password: form.value.password  }).subscribe(
      data => {
        this.router.navigate(['/']);
      },

      error => {
        alert('Ошибка входа в систему')
      }

    );
  }

}

