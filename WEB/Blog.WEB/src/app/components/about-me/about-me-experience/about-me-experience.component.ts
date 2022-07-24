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
        description: `I'm working for one of the major aeroplane manufacturers.
         Right now I'm a member of the team which is working at transition one part of the old monolith application into microservices.
          Having in mind I'm a full stack developer, I'm working both on backend and frontend, and little as a DevOps.
           The front is utilizing the latest version of angular.
            It's heavy tested and connected to the backend which was made using the latest .NET version which is right now .NET 6.
             The backend part got hundreds of unit and integration tests. The backend and frontend are deployed into Azure Kubernetes Services.
              I'm also building and maintaining CI/CD pipelines, which are used on daily basis.`
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
        description: `I've been working as a FullStack developer. My team's main task was to support the old system
        which was used by KPMG's back office to help their client. The old monolith system was written in .NET Framework 4.7.2.
        In the meanwhile, we've been creating a successor to the old system, based on microservices. The frontend part was built using Angular
        and for the backend, we've been using the latest version of Angular. It was a pleasure to work there and for sure I will miss all of
        the people whom I've got the pleasure to meet and work.`
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
        description: `At Intersys I was assigned to a team which was creating application targets for three platforms at one time:
        WEB, Desktop and Mobile. App has been developed using Xamarin and Intersys' own frontend framework.
        It was quite adventurous. Right there I've been mostly working with pure javascript, so it was a great chance to understand how it really works.
        I've been using also a lot of jQuery there - which I find i very useful tool, but in the case of big projects,
        I would rather use some framework like React/Angular.`
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
        description: `Centrum Wdorożeń Informatycznych is one of major Enova365 ERP software partner.
         I was responsible for creating data processing mechanisms and automatic reports to increase the performance of other companies'
          employees. This was my first job as a programmer, with great workmates.
           They've been very patient with me and my supervisor Tomasz B. was trying to teach me as much as he can, but unfortunately,
            I did not use all his lessons, because as usual, I was looking for a way to solve the problem.
             In retrospect, I should have listened more.`
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
        description: `I was responsible for taking care of company infrastructure and fixing problems which occurs during daily duties.
         But what do daily duties really are?
          I was enrolling new employees into company systems, creating accounts in an active directory for them,
           configuring computers and helping with problems.
            I'm happy that I've found this job - at this time I needed to find a job to continuee my studies.`
      },
    ];
  }

  getWorkPlace(position: Position): string {
    if (!position) {
      throw Error('No position provided!');
    }

    return [position.city, position.country]
      .filter(x => !!x)
      .join(', ');
  }
}
