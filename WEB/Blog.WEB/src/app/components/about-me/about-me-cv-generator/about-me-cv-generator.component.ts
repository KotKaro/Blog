import {Component} from '@angular/core';
import {HTMLOptions, jsPDF} from 'jspdf';

@Component({
  selector: 'app-about-me-cv-generator',
  templateUrl: './about-me-cv-generator.component.html',
  styleUrls: ['./about-me-cv-generator.component.scss']
})
export class AboutMeCvGeneratorComponent {
  async downloadCv(): Promise<void> {
    const doc = new jsPDF();

    const template = `

<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>My Website</title>
    <link rel="icon" href="./favicon.ico" type="image/x-icon">
    <style>
      @page {
          size: A4;
      }
        </style>
  </head>
  <body>
  <div style="display: flex">
      <div style="flex: 20%; background-color: blue">
          Karol
      </div>
      <div style="flex: 80%">
          TEST
      </div>
  </div>
  </body>
</html>
`;

    await doc.html(template, {} as HTMLOptions)
      .save('test.pdf');
  }
}
