import { Component } from "@angular/core";
import { DataService } from '../shared/dataService';
import { Router } from '@angular/router';


@Component({
  selector: "login",
  templateUrl: "login.component.html"
})
export class Login {
  constructor(private data: DataService, private router: Router) {

  }

  private errorMessage: string = "";
  public creds = {
    username: "",
    password: ""
  };

  onLogin() {
    //call the login service
    //let message = `${this.creds.username} - ${this.creds.password}`;
    //console.log(message);
    this.data.login(this.creds)
      .subscribe(success => {
          if (success ) {
            if (this.data.order.items.length === 0) {
              this.router.navigate(["/"]);
            } else {
              this.router.navigate(["checkout"]);
            }
          }
        },
        error => this.errorMessage = "Failed to login");
  }
}