import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
model: any = {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
     // we use subscribe because the login method in the auth service return an observable
    this.authService.login(this.model).subscribe(
      next => { console.log('logged in successfully'); }
      , error => {
        console.log(error);
            });
  }


  loggedIn() {

    // if token loaded return true else false
    const token = localStorage.getItem('token');
    return !!token;
  }

  logout() {
    localStorage.removeItem('token');
    console.log('logged out');

  }
}
