import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { TeamMember } from '../../../core/models/teamMember';
import { TeamMemberComponent } from '../team-member/team-member.component';

@Component({
  selector: 'app-team',
  standalone: true,
  imports: [TranslateModule, TeamMemberComponent],
  templateUrl: './team.component.html',
  styleUrl: './team.component.css',
})
export class TeamComponent {
  onFilledHeartClick() {
    throw new Error('Method not implemented.');
  }
  onFavoriteDesignerCange(member: TeamMember) {
    this.designers.forEach(designer => {
      if (designer.id === member.id) {
        designer.isFavorite = !designer.isFavorite;
      }
    });
  }
  onFavoriteDeveloperCange(member: TeamMember) {
    this.developers.forEach(developer => {
      if (developer.id === member.id) {
        developer.isFavorite = !developer.isFavorite;
      }
    });
  }
  designers: TeamMember[] = [
    {
      id: '1',
      name: 'OLGA',
      role: 'UA_UX_WEB_DESIGNER',
      videoUrl: '../../../assets/images/team/olga.mp4',
      linkedInUrl: 'https://www.linkedin.com/in/olga-tomchuk-649637395/',
      isFavorite: true,
    },
    {
      id: '2',
      name: 'KATERINA',
      role: 'UA_UX_MOBILE_DESIGNER',
      videoUrl: '../../../assets/images/team/katerina.mp4',
      linkedInUrl:
        'https://www.linkedin.com/in/%D0%BA%D0%B0%D1%82%D0%B5%D1%80%D0%B8%D0%BD%D0%B0-%D1%86%D0%B2%D1%96%D0%B3%D1%83%D0%BD-543759374/',
      isFavorite: true,
    },
  ];
  developers: TeamMember[] = [
    {
      id: '3',
      name: 'ALLA',
      role: 'FRONTEND_DEVELOPER',

      videoUrl: '../../../assets/images/team/alla.mp4',
      linkedInUrl: 'https://www.linkedin.com/in/alla-kokhaniuk-aa0833272/',
      isFavorite: true,
    },

    {
      id: '4',
      name: 'VADIM',
      role: 'BACKEND_DEVELOPER',
      videoUrl: '../../../assets/images/team/vadim.mp4',
      linkedInUrl: 'https://www.linkedin.com/in/vadim-ancuta-5433a6316/',
      isFavorite: true,
    },
  ];
}
