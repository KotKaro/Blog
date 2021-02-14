import { Component } from '@angular/core';
import { Position } from '../models/position.model';

@Component({
  selector: 'app-about-me-experience',
  templateUrl: './about-me-experience.component.html',
  styleUrls: ['./about-me-experience.component.scss']
})
export class AboutMeExperienceComponent {
  getPositions(): Position[] {
    return [
      {
        period: {
          from: new Date(2021, 2),
        },
        companyName: 'eWork Group',
        position: 'Programming Consultant',
        city: 'Remote',
        description: 'Supporting clients with the process of building their applications.'
      },
      {
        period: {
          from: new Date(2020, 2),
          to: new Date(2021, 1)
        },
        companyName: 'KPMG',
        position: '.NET Developer',
        city: 'Warsaw',
        country: 'Poland',
        description: 'With team we are creating new application, looking for solutions of bussines problems related to imigration processes.'
      },
      {
        period: {
          from: new Date(2019, 5),
          to: new Date(2020, 1),
        },
        companyName: 'Intersys',
        position: 'junior software developer',
        city: 'Warsaw',
        country: 'Poland',
        description: 'At Intersys i was assigned to team which was creating application targets three platforms in one time: WEB, Desktop and Mobile. App has been devloped using Xamarin and Intersys own frontend framework. It was quite adventure.'
      },
      {
        period: {
          from: new Date(2018, 3),
          to: new Date(2019, 4),
        },
        companyName: 'CENTRUM WDROŻEŃ INFORMATYCZNYCH',
        position: 'JUNIOR .NET PROGRAMMER',
        city: 'Warsaw',
        country: 'Poland',
        description: 'At this work I\'m responsible for creating data processing mechanisms and automatical reports in order to increasing performance of other companies employees.'
      },
      {
        period: {
          from: new Date(2018, 1),
          to: new Date(2018, 2),
        },
        companyName: 'Optima Logistics',
        position: 'Junior IT specialist',
        city: 'Warsaw',
        country: 'Poland',
        description: 'I was responsible for taking care about company infrastructure and fixing problems which occurs durning daily duties.'
      },
    ];
  }

  getWorkPlace(position: Position): string {
    if (!position) {
      throw Error("No position provided!");
    }

    return [position.city, position.country]
      .filter(x => !!x)
      .join(', ');
  }
}
