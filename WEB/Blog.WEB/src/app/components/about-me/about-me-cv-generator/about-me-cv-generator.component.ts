import {Component} from '@angular/core';
import {jsPDF} from 'jspdf';
declare var require: any;

@Component({
  selector: 'app-about-me-cv-generator',
  templateUrl: './about-me-cv-generator.component.html',
  styleUrls: ['./about-me-cv-generator.component.scss']
})
export class AboutMeCvGeneratorComponent {
  async downloadCv(): Promise<void> {
    const doc = new jsPDF('p','pt','a4');


    const template = `
<!DOCTYPE html>
<html lang="en" style="height: 100%">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.2/css/all.min.css" integrity="sha512-1sCRPdkRXhBV2PBLUdRb4tMg1w2YPf37qatUFeS7zlBy7jJI8Lf4VHwWfZZfpXtYSLy85pkm9GaYVYMfw5BC1A==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <style>
        @page  {
            size: a4;
            margin: 0;
        }
        body
        {
            width:100%;
            height:100%;
            font-family: century-gothic, sans-serif;
            font-weight: 400;
            font-style: normal;
            margin: 0;
            padding: 0;
        }
        table {
            border-spacing: 0;
            border-collapse: separate;
            width: 100%;
        }
        .left {
            width: 20%;
            background-color: #002d58;
        }
        tr > * + * {
            padding-left: 1rem;
        }
        h1 {
            font-size: 3rem;
        }
        h3 {
            font-size: 1.4rem;
            font-weight: 300;
        }
        h1,h3,h2 {
            margin-block-start: 0;
            margin-block-end: 0;
            color: #002d58;
        }
        td {
            font-weight: lighter;
            font-size: 15px;
         }

        td * {
            vertical-align: middle;
        }

        .big-dot, .normal-dot, .small-dot, .small-dot-grey, .tiny-dot {
            border-radius: 50%;
            display: inline-block;
            position: relative;
        }

        .big-dot, .normal-dot, .small-dot, .tiny-dot {
            background-color: #002d58;
        }

        .small-dot-grey {
            background-color: #dbdbdb;
        }

        .big-dot {
            height: 2.1rem;
            width: 2.1rem;
            font-size: 1.2rem;
        }

        .normal-dot {
            height: 1.6rem;
            width: 1.6rem;
            font-size: 1.2rem;
        }

        .small-dot, .small-dot-grey {
            height: 1em;
            width: 1em;
        }

        .tiny-dot {
            height: 0.7rem;
            width: 0.7rem;
        }

        .big-dot i, .normal-dot i {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            color: white;
        }

        .dot-wrapper {
            text-align:center;
            padding: 0;
            background: linear-gradient(to right,
            transparent 0%,
            transparent calc(50% - 0.81px),
            #dbdbdb calc(50% - 0.8px),
            #dbdbdb calc(50% + 0.8px),
            transparent calc(50% + 0.81px),
            transparent 100%);
        }

        .skill-level-wrapper {
            float: right;
        }
    </style>
</head>
<body>
<table>
    <colgroup>
        <col class="left" />
        <col />
        <col />
    </colgroup>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td></td>
        <td><h1>Karol Kot</h1></td>
    </tr>
    <tr>
        <td></td>
        <td colspan="2"><h3>.NET Fullstack Developer</h3></td>
    </tr>
    <tr>
        <td></td>
        <td>
            <span class="normal-dot"><i class="fas fa-map-marker-alt"></i></span>
            <span>Warsaw</span>
        </td>
        <td>
            <span class="normal-dot"><i class="fa-brands fa-linkedin-in"></i></span>
            <span>https://www.linkedin.com/in/karol-kot-68653a150/</span>
        </td>
    </tr>
    <tr>
        <td></td>
        <td>
            <span class="normal-dot"><i class="fas fa-phone"></i></span>
            <span>514 810 358</span>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td>
            <span class="normal-dot"><i class="fas fa-at"></i></span>
            <span>karol.kkot@gmail.com</span>
        </td>
        <td>
            <span class="normal-dot"><i class="fas fa-external-link"></i></span>
            <span>https://kkarolblog.web.app/about-me</span>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td></td>
        <td colspan="2" style="text-align: justify; font-size: 12.5px">I like to go beyond own limits. Each day I want to know more than at previous day and ask myself question for the next day.</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
</table>
<table>
    <colgroup>
        <col class="left" />
        <col style="width: 8%;"/>
        <col />
        <col />
    </colgroup>
    <tr>
        <td/>
        <td class="dot-wrapper">
            <span class="big-dot"><i class="fas fa-puzzle-piece" style="padding: 0"></i></span>
        </td>
        <td>
            <span><h2 style="display: inline-block">Skills</h2></span>
        </td>
        <td></td>
    </tr>
    <tr>
        <td/>
        <td class="dot-wrapper">
            <span class="tiny-dot"></span>
        </td>
        <td>.NET/C#</td>
        <td class="skill-level-wrapper">
           <div>
               <span class="small-dot"></span>
               <span class="small-dot"></span>
               <span class="small-dot"></span>
               <span class="small-dot"></span>
               <span class="small-dot-grey"></span>
           </div>
           <div>Very Good</div>
        </td>
    </tr>
    <tr>
        <td/>
        <td class="dot-wrapper">
            <span class="tiny-dot"></span>
        </td>
        <td>HTML</td>
        <td class="skill-level-wrapper">
            <div>
                <span class="small-dot"></span>
                <span class="small-dot"></span>
                <span class="small-dot"></span>
                <span class="small-dot"></span>
                <span class="small-dot-grey"></span>
            </div>
            <div>Very Good</div>
        </td>
    </tr>
    <tr>
        <td/>
        <td class="dot-wrapper">
            <span class="tiny-dot"></span>
        </td>
        <td>Docker</td>
        <td class="skill-level-wrapper">
            <div>
                <span class="small-dot"></span>
                <span class="small-dot"></span>
                <span class="small-dot"></span>
                <span class="small-dot-grey"></span>
                <span class="small-dot-grey"></span>
            </div>
            <div>Good</div>
        </td>
    </tr>
    <tr>
        <td/>
        <td class="dot-wrapper">
            <span class="tiny-dot"></span>
        </td>
        <td>Java</td>
        <td class="skill-level-wrapper">
            <div>
                <span class="small-dot"></span>
                <span class="small-dot-grey"></span>
                <span class="small-dot-grey"></span>
                <span class="small-dot-grey"></span>
                <span class="small-dot-grey"></span>
            </div>
            <div>Basic</div>
        </td>
    </tr>
</table>
</body>
</html>

`;

    let myWindows = window.open('', 'PRINT', 'height=400,with=600');
    myWindows.document.write(template);
    myWindows.document.close();
    myWindows.focus();
    myWindows.print();

    //await doc.html(template, {} as HTMLOptions)
     // .save('test.pdf');
  }
}
