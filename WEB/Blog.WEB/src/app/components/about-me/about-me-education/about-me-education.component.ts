import { Component } from '@angular/core';

@Component({
  selector: 'app-about-me-education',
  templateUrl: './about-me-education.component.html',
  styleUrls: ['./about-me-education.component.scss']
})
export class AboutMeEducationComponent {
  getEducations(): any[] {
    return [
      {
        years: '2013 - 2016',
        grade: 'High school',
        schoolName: 'I LO IM. TADEUSZ KOŚCIUSZKI',
        schoolCity: 'Łuków',
        schoolCountry: 'Poland',
        description: 'I choose math-geographical-english class. I think that was the best time of my life.'
      },
      {
        years: '2016 - Present',
        grade: 'Bachelor of computer science',
        schoolName: 'POLISH-JAPAN ACADEMY OF COMPUTER SCIENCE',
        schoolCity: 'Warsaw',
        schoolCountry: 'Poland',
        description: 'I\'m studying informatics, I want to be a specialist at data processing and optimalization.'
      }
    ];
  }
}
