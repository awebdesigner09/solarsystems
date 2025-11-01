import { Component, ChangeDetectionStrategy, inject, signal, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { DataService } from '../../services/data.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-solar-system-catalog',
  template: `
    <div class="container mx-auto px-4 py-8">
      <div class="flex justify-between items-center mb-6">
        @if (authService.isAdmin()) {
          <h1 class="text-3xl font-bold text-gray-100">Catalog</h1>
        }
        @else{
          <h1 class="text-3xl font-bold text-gray-100">Start with Solar NOW!</h1>
        }
        
        @if (authService.isAdmin()) {
          <a routerLink="/admin/model/new" class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-bold rounded-md shadow-sm text-gray-900 bg-yellow-500 hover:bg-yellow-600">
            <svg class="w-5 h-5 mr-2" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
              <path fill-rule="evenodd" d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z" clip-rule="evenodd" />
            </svg>
            Add New Model
          </a>
        }
       
      </div>

      @if (isLoading()) {
        <div class="flex justify-center items-center p-8">
          <div class="animate-spin rounded-full h-16 w-16 border-b-4 border-yellow-500"></div>
        </div>
      }

      @if (models().length > 0) {
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          @for (model of models(); track model.id) {
            <div class="bg-gray-800 rounded-lg shadow-lg overflow-hidden flex flex-col">
              <img class="h-48 w-full object-cover" [src]="'assets/images/' + model.imageUrl" [alt]="model.name">
              <div class="p-6 flex flex-col flex-grow">
                <h2 class="text-xl font-bold text-gray-100">{{ model.name }}</h2>
                <p class="text-sm text-yellow-400">{{ model.panelType }} | {{ model.capacityKW }} kW</p>
                <p class="mt-2 text-gray-400 text-sm flex-grow">{{ model.description }}</p>
                <div class="mt-4">
                  <p class="text-lg font-semibold text-gray-200">Starting from</p>
                  <p class="text-2xl font-bold text-green-400">{{ model.basePrice | currency:'USD':'symbol':'1.0-0' }}</p>
                </div>
              </div>
              <div class="px-6 pb-4 bg-gray-800">
                @if (authService.isAdmin()) {
                   <div class="flex justify-between items-center mt-2">
                      <a [routerLink]="['/admin/model/edit', model.id]" class="text-sm font-medium text-blue-400 hover:underline">Edit</a>
                      <button (click)="deleteModel(model.id)" class="text-sm font-medium text-red-400 hover:underline">Delete</button>
                   </div>
                } @else {
                  <a [routerLink]="['/request-quote', model.id]" class="block w-full text-center bg-yellow-500 text-gray-900 font-bold py-2 px-4 rounded hover:bg-yellow-600 transition duration-300">
                    Request a Quote
                  </a>
                }
              </div>
            </div>
          }
        </div>
      } @else if (!isLoading()) {
        <p class="text-center text-gray-400 mt-8">No solar system models available at the moment.</p>
      }
    </div>

    <!-- Delete Confirmation Modal -->
    @if (showDeleteConfirm()) {
      <div class="fixed inset-0 bg-gray-900 bg-opacity-75 flex items-center justify-center z-50 transition-opacity duration-300 ease-in-out">
        <div class="bg-gray-800 rounded-lg shadow-xl p-6 w-full max-w-md mx-4 transform transition-all duration-300 ease-in-out scale-100">
          <h3 class="text-xl font-bold text-gray-100 mb-4">Confirm Deletion</h3>
          <p class="text-gray-400 mb-6">
            Are you sure you want to delete this solar system model? This action cannot be undone.
          </p>
          <div class="flex justify-end space-x-4">
            <button (click)="cancelDelete()" class="px-4 py-2 rounded-md text-gray-300 bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-gray-800 focus:ring-gray-500">
              Cancel
            </button>
            <button (click)="confirmDelete()" class="px-4 py-2 rounded-md font-semibold text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-gray-800 focus:ring-red-500">
              Delete
            </button>
          </div>
        </div>
      </div>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, RouterLink, CurrencyPipe],
})
export class SolarSystemCatalogComponent implements OnInit {
  private dataService = inject(DataService);
  authService = inject(AuthService);

  models = this.dataService.models;
  isLoading = signal(true);
  showDeleteConfirm = signal(false);
  modelToDelete = signal<string | null>(null);

  ngOnInit(): void {
    this.dataService.getSolarSystemModels()
      .pipe(finalize(() => this.isLoading.set(false)))
      .subscribe();
  }

  deleteModel(id: string): void {
    this.modelToDelete.set(id);
    this.showDeleteConfirm.set(true);
  }

  confirmDelete(): void {
    const id = this.modelToDelete();
    if (id) {
      this.dataService.deleteSolarSystemModel(id).subscribe(() => {
        this.cancelDelete();
      });
    }
  }

  cancelDelete(): void {
    this.modelToDelete.set(null);
    this.showDeleteConfirm.set(false);
  }
}
