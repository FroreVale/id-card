import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'user-auth-app';
  

  showIdCardDetails: boolean = false;


  activeForm: 'login' | 'register' = 'login';


  loginData = {
    username: '',
    password: ''
  };

  registerData = {
    username: '',
    password: ''
  };


  idCardDetails = {
    admissionNo: '',
    parentName: '',
    phoneNo: ''
  };


  message: string = '';


  downloadUrl: string = '';

  // Change active form between login and register
  toggleForm(form: 'login' | 'register') {
    this.activeForm = form;
    this.message = ''; 
    
    this.downloadUrl = '';
  }

  // Handle Login form submit using fetch
  async onLogin() {
    try {
      const response = await fetch('https://localhost:7125/api/User/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          username: this.loginData.username,
          userPassword: this.loginData.password
        })
      });

      if (response.ok) {
        // On success, hide the auth forms and show the ID Card Details
        this.showIdCardDetails = true;
        this.message = '';
      } else {
        const errorText = await response.text();
        this.message = errorText || 'Invalid username or password';
      }
    } catch (error: any) {
      this.message = error.message;
    }
  }

  // Handle Register form submit using fetch
  async onRegister(registerForm: NgForm) {

    if (!registerForm.valid) {
      this.message = 'Please fill in both username and password.';
      return;
    }
    try {
      const response = await fetch('https://localhost:7125/api/User/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          username: this.registerData.username,
          userPassword: this.registerData.password
        })
      });

      if (response.ok) {
        // On success, hide the auth forms and show the ID Card Details
        this.showIdCardDetails = true;
        this.message = '';
      } else {
        const errorText = await response.text();
        this.message = errorText || 'Unable to register';
      }
    } catch (error: any) {
      this.message = error.message;
    }
  }

  // Handle ID Card Details form submit using fetch
  async onIDCardSubmit(idCardForm: NgForm) {
    // If not all required fields are filled, show an error message.
    if (!idCardForm.valid) {
      this.message = 'Please fill in all required fields for the ID Card Details.';
      return;
    }
    try {
      const response = await fetch('https://localhost:7125/api/User/idCardDetails', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          admissionNo: this.idCardDetails.admissionNo,
          parentName: this.idCardDetails.parentName,
          phoneNo: this.idCardDetails.phoneNo
        })
      });
      if (response.ok) {
        // Process the PDF file returned as blob and create an object URL.
        const pdfBlob = await response.blob();
        this.downloadUrl = URL.createObjectURL(pdfBlob);
        this.message = "PDF is ready for download.";
      } else {
        const errorText = await response.text();
        this.message = errorText || "Failed to submit ID Card details.";
        this.downloadUrl = "";  
      }
    } catch (error: any) {
      this.message = error.message;
      this.downloadUrl = ""; 
    }
  }
}
