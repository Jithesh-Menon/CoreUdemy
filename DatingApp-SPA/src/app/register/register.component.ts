import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/_services/authservice/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      console.log('registrartion successful');
    }, error => {
      console.log(error);
    })
  }

  cancel() {
    console.log('cancelled..!');
  }
}
