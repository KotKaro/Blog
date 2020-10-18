import { Component } from '@angular/core';

@Component({
  selector: 'app-about-me-profiles',
  templateUrl: './about-me-profiles.component.html',
  styleUrls: ['./about-me-profiles.component.scss']
})
export class AboutMeProfilesComponent {
  getProfiles(): any[] {
    return [
      {
        link: 'https://github.com/KotKaro',
        iconClass: 'fa fa-github',
        name: 'Github'
      },
      {
        link: 'https://bitbucket.org/kotkarol/',
        iconClass: 'fa fa-bitbucket',
        name: 'BitBucket'
      },
      {
        link: 'https://www.linkedin.com/in/karol-kot-68653a150/',
        iconClass: 'fa fa-linkedin',
        name: 'LinkedIn'
      },
      {
        link: 'https://dev.azure.com/ice1speak/',
        iconClass: 'fa fa-cloud',
        name: 'DevOps'
      },
    ];
  }
}
