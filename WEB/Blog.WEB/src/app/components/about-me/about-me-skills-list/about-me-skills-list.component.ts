import {Component, Input} from '@angular/core';
import {Skill} from '../models/skill.model';

@Component({
  selector: 'app-about-me-skills-list',
  templateUrl: './about-me-skills-list.component.html',
  styleUrls: ['./about-me-skills.component-list.scss']
})
export class AboutMeSkillsListComponent {
  @Input() skills: Skill[];
}
