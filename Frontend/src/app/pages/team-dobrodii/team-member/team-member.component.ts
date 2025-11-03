import { LowerCasePipe, UpperCasePipe } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { TeamMember } from '../../../core/models/teamMember';
import { PrimaryLargeButtonComponent } from '../../../shared/components/buttons/blue/primary-large-button.component';
import { RoundFilledWhiteBlueButtonWithIconComponent } from '../../../shared/components/buttons/round-filled-white-blue-button-with-icon.component';
import { RoundWhiteBlueButtonWithIconComponent } from '../../../shared/components/buttons/round-white-blue-button-with-icon.component';

@Component({
  selector: 'app-team-member',
  standalone: true,
  imports: [
    LowerCasePipe,
    TranslateModule,
    RoundFilledWhiteBlueButtonWithIconComponent,
    RoundWhiteBlueButtonWithIconComponent,
    UpperCasePipe,
    PrimaryLargeButtonComponent,
  ],
  templateUrl: './team-member.component.html',
  styleUrl: './team-member.component.css',
})
export class TeamMemberComponent {
  @Input() member!: TeamMember;
  @Output() favoriteChange = new EventEmitter();
  openLinkedin(): void {
    window.open(this.member.linkedInUrl, '_blank');
    // або: location.href = url; // якщо потрібно в тій самій вкладці
  }

  onBlankHeartClick() {
    this.favoriteChange.emit(this.member);
  }
  onFilledHeartClick() {
    this.favoriteChange.emit(this.member);
  }
}
