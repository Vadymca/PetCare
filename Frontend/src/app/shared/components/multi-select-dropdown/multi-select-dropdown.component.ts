import { CommonModule, UpperCasePipe } from '@angular/common';
import {
  Component,
  effect,
  EventEmitter,
  Input,
  OnInit,
  Output,
  signal,
} from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PrimarySmallOrangeButtonComponent } from '../buttons/orange/primary-small-orange-button.component';
import { IconComponent } from '../icon.component';

@Component({
  selector: 'app-multi-select-dropdown',
  standalone: true,
  imports: [
    CommonModule,
    TranslateModule,
    UpperCasePipe,
    PrimarySmallOrangeButtonComponent,
    IconComponent,
  ],
  templateUrl: './multi-select-dropdown.component.html',
  styleUrl: './multi-select-dropdown.component.css',
})
export class MultiSelectDropdownComponent implements OnInit {
  @Input() options: string[] = [];
  @Input() label = 'Select';
  @Output() selectionChange = new EventEmitter<string[]>();
  isOpen = signal(false);
  selected = signal<string[]>([]);

  isDisabled = signal(false);
  constructor() {
    effect(() => {
      this.isDisabled.set(this.selected().length === 0);
    });
  }
  ngOnInit() {
    this.selected.set(this.options);
  }
  selectAll() {
    if (this.selected().length < this.options.length) {
      this.selected.set(this.options);
    } else {
      this.selected.set([]);
    }
  }
  confirmSelection() {
    this.selectionChange.emit(this.selected());

    this.isOpen.set(false);
  }

  toggleDropdown() {
    this.isOpen.update(v => !v);
  }
  toggleOption(option: string) {
    const current = [...this.selected()];
    if (current.includes(option)) {
      this.selected.set(current.filter(o => o !== option));
    } else {
      current.push(option);
      this.selected.set(current);
    }
  }
  isActive() {
    return this.isOpen() || this.selected().length < this.options.length;
  }
}
