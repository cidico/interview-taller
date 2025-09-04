import { Component, ElementRef, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatInputModule, MatButtonModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  postKey = '';
  postValue = '';
  getKey = '';
  getResult: string | null = null;
  message: string | null = null;
  showSuccess = false;
  showError = false;

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  addItem() {
    if (this.postKey && this.postValue) {
      this.showSuccess = false;
      this.http.post(`${this.apiUrl}/api/sample`, { key: this.postKey, value: this.postValue }).subscribe({
        next: () => {
          this.postKey = '';
          this.postValue = '';
          this.showSuccess = true;

          setTimeout(() => {
            this.showSuccess = false;
          }, 3000);
        },
        error: (error) => console.error('Error adding item:', error),
      });
    }
  }

  getItem() {
    if (this.getKey) {
      this.http.get<string>(`${this.apiUrl}/api/sample/${this.getKey}`).subscribe({
        next: (value) => {
          this.getResult = value;
          this.message = ""
        },
        error: (error) => {
          this.getResult = null;
          this.showError = true;
          this.message = null;
        },
      });
    }
  }
}