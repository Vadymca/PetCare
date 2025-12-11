import { CommonModule } from '@angular/common';
import { Component, EventEmitter, input, Output } from '@angular/core';
import { News } from '../../../core/models/news';

@Component({
  selector: 'app-news-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './news-card.component.html',
  styleUrl: './news-card.component.css',
})
export class NewsCardComponent {
  news = input.required<News>();
  odd = input.required<boolean>();
  @Output() cardClick = new EventEmitter();

  onCardClick() {
    this.cardClick.emit(this.news());
  }
}
