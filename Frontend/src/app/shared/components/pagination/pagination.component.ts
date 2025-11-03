import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { IconComponent } from '../icon.component';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [IconComponent],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css',
})
export class PaginationComponent implements OnChanges {
  leftArrowClick() {
    this.goToPage(this.localPage - 1);
  }
  rightArrowClick() {
    this.goToPage(this.localPage + 1);
  }
  @Input() totalPages = 1;
  @Input() currentPage = 1;
  // локальна копія, щоб не відставала
  localPage = 1;
  @Output() pageChange = new EventEmitter<number>();
  ngOnChanges(changes: SimpleChanges) {
    if (changes['currentPage']) {
      this.localPage = this.currentPage;
    }
  }
  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.localPage = page;
    this.pageChange.emit(page);
  }

  visiblePages(): (number | null)[] {
    const pages: (number | null)[] = [];

    if (this.totalPages <= 5) {
      for (let i = 1; i <= this.totalPages; i++) {
        pages.push(i);
      }
      return pages;
    }

    // Перша сторінка завжди
    pages.push(1);

    if (this.localPage > 3) {
      pages.push(null); // ...
    }

    // Діапазон навколо поточної
    const start = Math.max(2, this.localPage - 1);
    const end = Math.min(this.totalPages - 1, this.localPage + 1);

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    if (this.localPage < this.totalPages - 2) {
      pages.push(null);
    }

    // Остання сторінка
    pages.push(this.totalPages);

    return pages;
  }
}
