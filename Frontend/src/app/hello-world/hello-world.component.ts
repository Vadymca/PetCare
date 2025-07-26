import { CommonModule, isPlatformServer } from '@angular/common';
import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-hello-world',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="showMessage">
      <h1>Hello World!</h1>
      <p>Rendered on {{ renderLocation }}</p>
    </div>
    <div *ngIf="!showMessage">
      <p>Message hidden</p>
    </div>
  `,
})
export class HelloWorldComponent implements OnInit {
  showMessage = false;
  renderLocation = 'client';

  constructor(
    private route: ActivatedRoute,
    @Inject(PLATFORM_ID) private platformId: object
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.showMessage = params['show'] === 'true';
    });

    this.renderLocation = isPlatformServer(this.platformId)
      ? 'server'
      : 'client';
  }
}
