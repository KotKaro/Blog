import {Component} from '@angular/core';
import {Skill} from '../models/skill.model';

@Component({
  selector: 'app-about-me-skills',
  templateUrl: './about-me-skills.component.html',
  styleUrls: ['../about-me.component.scss']
})
export class AboutMeSkillsComponent {
  get leftSkills(): Skill[] {
    return [
      {
        name: '.NET/C#',
        level: 75
      },
      {
        name: 'SQL',
        level: 55
      },
      {
        name: 'DATA ANALYSIS',
        level: 55
      },
      {
        name: 'Angular',
        level: 70
      },
      {
        name: 'Azure',
        level: 60
      },
      {
        name: 'CSS',
        level: 40
      },
      {
        name: 'Python',
        level: 30
      },
    ];
  }

  get rightSkills(): Skill[] {
    return [
      {
        name: 'HTML',
        level: 65
      },
      {
        name: 'Docker',
        level: 40
      },
      {
        name: 'Java',
        level: 20
      },
      {
        name: 'Domain Driven Design',
        level: 40
      },
      {
        name: 'Test Driven Development',
        level: 50
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
