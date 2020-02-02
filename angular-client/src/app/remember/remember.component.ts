import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { AuthService } from '../core';
import { NgForm } from '@angular/forms';


@Component({
  templateUrl: 'remember.component.html',
  styleUrls: ['remember.component.css']
})
export class RememberComponent implements OnInit {

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
    
    this.authservice.remember({ email: form.value.email}).subscribe(
      data => {
        this.router.navigate(['/']);
      },

      error => {
        alert('Ошибка отправки')
      }

    );
  }

}

