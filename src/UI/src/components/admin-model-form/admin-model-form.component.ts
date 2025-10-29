import { Component, ChangeDetectionStrategy, inject, signal, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { filter, finalize, switchMap } from 'rxjs/operators';
import { DataService } from '../../services/data.service';
import { SolarSystemModel } from '../../models/solar-system-model.model';

@Component({
  selector: 'app-admin-model-form',
  template: `
    <div class="container mx-auto px-4 py-8 max-w-2xl">
      <a routerLink="/admin" class="inline-flex items-center text-yellow-400 hover:underline mb-6">
        <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
        Back to Dashboard
      </a>

      @if (isLoading()) {
        <div class="flex justify-center items-center p-8">
          <div class="animate-spin rounded-full h-16 w-16 border-b-4 border-yellow-500"></div>
        </div>
      }

      <div class="bg-gray-800 rounded-lg shadow-lg p-8">
        <h1 class="text-3xl font-bold mb-6 text-gray-100">{{ isEditMode() ? 'Edit' : 'Create' }} Solar System Model</h1>
        
        <form [formGroup]="modelForm" (ngSubmit)="onSubmit()">
          <div class="space-y-4">
            <div>
              <label for="name" class="block text-sm font-medium text-gray-400">Model Name</label>
              <input id="name" formControlName="name" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm">
            </div>
            
            <div>
              <label for="panelType" class="block text-sm font-medium text-gray-400">Panel Type</label>
              <select id="panelType" formControlName="panelType" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm">
                <option value="Monocrystalline">Monocrystalline</option>
                <option value="Polycrystalline">Polycrystalline</option>
                <option value="Thin-Film">Thin-Film</option>
              </select>
            </div>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
               <div>
                <label for="capacityKW" class="block text-sm font-medium text-gray-400">Capacity (kW)</label>
                <input id="capacityKW" type="number" formControlName="capacityKW" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm">
              </div>
               <div>
                <label for="basePrice" class="block text-sm font-medium text-gray-400">Base Price ($)</label>
                <input id="basePrice" type="number" formControlName="basePrice" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm">
              </div>
            </div>

            <div>
              <label for="imageUrl" class="block text-sm font-medium text-gray-400">Image URL</label>
              <input id="imageUrl" formControlName="imageUrl" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm">
            </div>

            <div>
              <label for="description" class="block text-sm font-medium text-gray-400">Description</label>
              <textarea id="description" formControlName="description" rows="4" class="mt-1 block w-full px-3 py-2 bg-gray-700 border border-gray-600 rounded-md shadow-sm text-gray-200 placeholder-gray-400 focus:outline-none focus:ring-yellow-500 focus:border-yellow-500 sm:text-sm"></textarea>
            </div>
          </div>

          <div class="mt-8 text-right">
            <button type="submit" [disabled]="modelForm.invalid || isSubmitting()" class="inline-flex justify-center py-3 px-6 border border-transparent rounded-md shadow-sm text-lg font-bold text-gray-900 bg-yellow-500 hover:bg-yellow-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-yellow-500 disabled:bg-gray-600 disabled:text-gray-400 disabled:cursor-not-allowed">
              @if (isSubmitting()) {
                <span class="animate-spin rounded-full h-5 w-5 border-b-2 border-gray-900 mr-2"></span>
                <span>Saving...</span>
              } @else {
                <span>{{ isEditMode() ? 'Update Model' : 'Create Model' }}</span>
              }
            </button>
          </div>
        </form>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
})
export class AdminModelFormComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  private dataService = inject(DataService);

  isEditMode = signal(false);
  isLoading = signal(true);
  isSubmitting = signal(false);
  
  private modelId: string | null = null;

  modelForm = this.fb.group({
    name: ['', Validators.required],
    panelType: ['Monocrystalline', Validators.required],
    capacityKW: [0, [Validators.required, Validators.min(0.1)]],
    basePrice: [0, [Validators.required, Validators.min(1)]],
    description: ['', Validators.required],
    imageUrl: ['', Validators.required],
  });

  ngOnInit(): void {
    this.route.paramMap.pipe(
      filter(params => {
          this.modelId = params.get('id');
          this.isEditMode.set(!!this.modelId);
          return !!this.modelId;
      }),
      switchMap(params => this.dataService.getSolarSystemModelById(this.modelId!)),
      finalize(() => this.isLoading.set(false))
    ).subscribe(model => {
      if (model) {
        this.modelForm.patchValue(model);
      }
    });

    if (!this.isEditMode()) {
        this.isLoading.set(false);
    }
  }

  onSubmit() {
    if (this.modelForm.invalid) {
      return;
    }
    this.isSubmitting.set(true);

    const modelData = this.modelForm.value as Omit<SolarSystemModel, 'id'>;

    const save$ = this.isEditMode() 
      ? this.dataService.updateSolarSystemModel({ ...modelData, id: this.modelId! })
      : this.dataService.createSolarSystemModel(modelData);

    save$.pipe(
      finalize(() => this.isSubmitting.set(false))
    ).subscribe(() => {
      this.router.navigate(['/admin']);
    });
  }
}
