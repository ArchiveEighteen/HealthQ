import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-file-upload',
  imports: [MatIconModule, MatButtonModule],
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.scss',
})
export class FileUploadComponent implements OnInit {
  fileName: string;

  constructor() {}

  ngOnInit(): void {}

  onFileSelected(event) {
    const file: File = event.target.files[0];

    if (file) {
      this.fileName = file.name;

      const formData = new FormData();

      formData.append('thumbnail', file);

      console.log(formData);

      // const upload$ = this.http.post('/api/thumbnail-upload', formData);

      // upload$.subscribe();
    }
  }
}
