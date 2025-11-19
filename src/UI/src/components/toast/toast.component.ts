import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../services/toast.service';
import { ToastType } from '../../models/toast.model';

@Component({
  selector: 'app-toast',
  template: `
    <div class="fixed bottom-0 right-0 p-4 space-y-3 z-50 w-full max-w-sm">
      @for (toast of toastService.toasts(); track toast.id) {
        <div 
          role="alert"
          class="relative flex items-center p-4 pr-10 rounded-lg shadow-xl text-white animate-fade-in-up"
          [class]="toastClasses(toast.type)">

          <div class="flex-shrink-0">
            @switch(toast.type) {
              @case('success') {
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
              }
              @case('info') {
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
              }
              @case('warning') {
                <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"></path></svg>
              }
              @case('error') {
                 <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
              }
            }
          </div>
          
          <div class="ml-3 font-medium">
            <ng-container *ngIf="toast.navigationLink; else simpleToast">
              <a [routerLink]="toast.navigationLink" (click)="remove(toast.id)" [class]="toastClasses(toast.type) + ' cursor-pointer'">
                {{ toast.message }}
                <!-- Optional: Add an icon to indicate it's a link -->
              </a>
            </ng-container>

            <ng-template #simpleToast>
              <div [class]="toastClasses(toast.type)">
                {{ toast.message }}
                <button (click)="remove(toast.id)"></button>
              </div>
            </ng-template>
          </div>

          <button (click)="toastService.remove(toast.id)" class="absolute top-1.5 right-1.5 p-1.5 rounded-full inline-flex h-8 w-8 text-white/70 hover:text-white hover:bg-white/20 focus:outline-none focus:ring-2 focus:ring-white">
            <span class="sr-only">Dismiss</span>
            <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd"></path></svg>
          </button>
        </div>
      }
    </div>
  `,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToastComponent {
  toastService = inject(ToastService);

  toastClasses(type: ToastType): string {
    switch (type) {
      case 'info':
        return 'bg-blue-600';
      case 'success':
        return 'bg-green-600';
      case 'warning':
        return 'bg-yellow-500';
      case 'error':
        return 'bg-red-600';
    }
  }
}
