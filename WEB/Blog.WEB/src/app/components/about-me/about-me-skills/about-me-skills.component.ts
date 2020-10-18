import { Component } from '@angular/core';

@Component({
  selector: 'app-about-me-skills',
  templateUrl: './about-me-skills.component.html',
  styleUrls: ['./about-me-skills.component.scss']
})
export class AboutMeSkillsComponent {
  getLeftSkills(): any[] {
    return [
      {
        name: '.NET/C#',
        level: 70
      },
      {
        name: 'SQL',
        level: 50
      },
      {
        name: 'DATA ANALYSIS',
        level: 50
      },
      {
        name: 'Angular',
        level: 60
      },
      {
        name: 'Azure',
        level: 50
      },
      {
        name: 'CSS',
        level: 30
      },
    ];
  }

  getRightSkills(): any[] {
    return [
      {
        name: 'HTML',
        level: 65
      },
      {
        name: 'Docker',
        level: 30
      },
      {
        name: 'Java',
        level: 20
      },
      {
        name: 'CREATIVITY',
        level: 70
      },
      {
        name: 'CURIOSITY',
        level: 100
      },
    ];
  }
}
