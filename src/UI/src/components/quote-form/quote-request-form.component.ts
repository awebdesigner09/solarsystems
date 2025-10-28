
import { Component, ChangeDetectionStrategy, inject, signal, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { switchMap, tap, finalize, filter } from 'rxjs/operators';
import { of, combineLatest } from 'rxjs';

import { DataService } from '../../services/data.service';
import { AuthService } from '../../services/auth.service';
import { SolarSystemModel } from '../../models/solar-system-model.model';

@Component({
  selector: 'app-quote-request-form',
  template: `
    <div class="container mx-auto px-4 py-8 max-w-4xl">
      <a routerLink="/catalog" class="inline-flex items-center text-yellow-400 hover:underline mb-6">
        <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
        Back to Catalog
      </a>

      @if (isLoading()) {
        <div class="flex justify-center items-center p-8">
          <div class="animate-spin rounded-full h-16 w-16 border-b-4 border-yellow-500"></div>
        </div>
      }

      @if (model()) {
        <div class="bg-gray-800 rounded-lg shadow-lg p-8">
          <h1 class="text-3xl font-bold mb-2 text-gray-100">Request a Quote for: {{ model()?.name }}</h1>
          <p class="text-lg text-gray-400 mb-6">Fill in your details below to get a customized quote.</p>

          @if (hasActiveQuote()) {
             <div class="p-4 mb-4 text-sm text-yellow-300 rounded-lg bg-yellow-900/50" role="alert">
                <span class="font-medium">Heads up!</span> You already have an active quote for this model. You can view it in <a routerLink="/quotes" class="font-bold hover:underline">My Quotes</a>.
            </div>
          }
          
          <form [formGroup]="quoteForm" (ngSubmit)="onSubmit()">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
              <!-- Location Details -->
              <div class="md:col-span-2">
                <h3 class="text-xl font-semibold mb-4 border-b border-gray-700 pb-2 text-gray-300">Installation Address</h3>
              </div>
              <div>
                <label for="address" class="block text-sm font-medium text-gray-400">Street Address</label>
                <input id="address" formControlName="address" [class]="fieldClasses('address')">
              </div>
              <div>
                <label for="city" class="block text-sm font-medium text-gray-400">City</label>
                <input id="city" formControlName="city" [class]="fieldClasses('city')">
              </div>
              <div>
                <label for="state" class="block text-sm font-medium text-gray-400">State</label>
                <input id="state" formControlName="state" [class]="fieldClasses('state')">
              </div>
              <div>
                <label for="zipCode" class="block text-sm font-medium text-gray-400">ZIP Code</label>
                <input id="zipCode" formControlName="zipCode" [class]="fieldClasses('zipCode')">
              </div>

              <!-- Custom Configuration -->
              <div class="md:col-span-2">
                <h3 class="text-xl font-semibold mt-6 mb-4 border-b border-gray-700 pb-2 text-gray-300">Custom Options</h3>
              </div>
              <div class="flex items-start">
                <div class="flex items-center h-5">
                  <input id="batteryStorage" formControlName="batteryStorage" type="checkbox" class="focus:ring-yellow-500 h-4 w-4 text-yellow-600 bg-gray-700 border-gray-600 rounded">
                </div>
                <div class="ml-3 text-sm">
                  <label for="batteryStorage" class="font-medium text-gray-300">Include Battery Storage</label>
                  <p class="text-gray-400">Store excess energy for use during outages or at night. (+{{ 8000 | currency }})</p>
                </div>
              </div>
               <div class="flex items-start">
                <div class="flex items-center h-5">
                  <input id="evCharger" formControlName="evCharger" type="checkbox" class="focus:ring-yellow-500 h-4 w-4 text-yellow-600 bg-gray-700 border-gray-600 rounded">
                </div>
                <div class="ml-3 text-sm">
                  <label for="evCharger" class="font-medium text-gray-300">Include EV Charger</label>
                  <p class="text-gray-400">Charge your electric vehicle with clean solar power. (+{{ 1500 | currency }})</p>
                </div>
              </div>

              <div class="md:col-span-2">
                <label for="notes" class="block text-sm font-medium text-gray-400">Additional Notes (Optional)</label>
                <textarea id="notes" formControlName="notes" rows="4" [class]="fieldClasses('notes')" placeholder="Any specific requirements or questions..."></textarea>
              </div>
            </div>

            <div class="mt-8 text-right">
              <button type="submit" [disabled]="quoteForm.invalid || isSubmitting() || hasActiveQuote()" class="inline-flex justify-center py-3 px-6 border border-transparent rounded-md shadow-sm text-lg font-bold text-gray-900 bg-yellow-500 hover:bg-yellow-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-yellow-500 disabled:bg-gray-600 disabled:text-gray-400 disabled:cursor-not-allowed">
                @if (isSubmitting()) {
                  <span class="animate-spin rounded-full h-5 w-5 border-b-2 border-gray-900 mr-2"></span>
                  <span>Submitting...</span>
                } @else {
                  <span>Submit Quote Request</span>
                }
              </button>
            </div>
          </form>
        </div>
      } @else if (!isLoading()) {
         <p class="text-center text-gray-400 mt-8">Solar system model not found.</p>
      }
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, RouterLink, ReactiveFormsModule, CurrencyPipe],
})
export class QuoteRequestFormComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  private dataService = inject(DataService);
  private authService = inject(AuthService);

  model = signal<SolarSystemModel | undefined>(undefined);
  isLoading = signal(true);
  isSubmitting = signal(false);
  hasActiveQuote = signal(false);
  
  quoteForm = this.fb.group({
    address: ['', Validators.required],
    city: ['', Validators.required],
    state: ['', Validators.required],
    zipCode: ['', [Validators.required, Validators.pattern('^\\d{5}$')]],
    batteryStorage: [false],
    evCharger: [false],
    notes: [''],
  });

  fieldClasses(fieldName: string) {
    const control = this.quoteForm.get(fieldName);
    const baseClasses = 'mt-1 block w-full px-3 py-2 border rounded-md shadow-sm placeholder-gray-400 focus:outline-none sm:text-sm bg-gray-700 text-gray-200';
    if (!control || !control.touched) {
      return `${baseClasses} border-gray-600 focus:ring-yellow-500 focus:border-yellow-500`;
    }
    return control.valid 
      ? `${baseClasses} border-green-500 focus:ring-green-500 focus:border-green-500`
      : `${baseClasses} border-red-500 focus:ring-red-500 focus:border-red-500`;
  }
  
  ngOnInit(): void {
    this.route.paramMap.pipe(
      filter(params => params.has('modelId')),
      switchMap(params => {
        const modelId = params.get('modelId')!;
        const currentUser = this.authService.currentUser();
        
        if (!currentUser) {
            return of({ model: undefined, hasQuote: false });
        }
        
        const model$ = this.dataService.getSolarSystemModelById(modelId);
        const hasQuote$ = this.dataService.hasActiveQuote(currentUser.id, modelId);
        
        return combineLatest({ model: model$, hasQuote: hasQuote$ });
      }),
      finalize(() => this.isLoading.set(false))
    ).subscribe(({ model, hasQuote }) => {
      this.model.set(model);
      this.hasActiveQuote.set(hasQuote);
    });
  }

  onSubmit() {
    if (this.quoteForm.invalid || !this.model() || !this.authService.currentUser()) {
      return;
    }

    this.isSubmitting.set(true);
    const formValue = this.quoteForm.value;

    const location = {
      address: formValue.address!,
      city: formValue.city!,
      state: formValue.state!,
      zipCode: formValue.zipCode!,
    };
    
    const config = {
      batteryStorage: formValue.batteryStorage!,
      evCharger: formValue.evCharger!,
      notes: formValue.notes || undefined,
    };
    
    this.dataService.createQuote(this.authService.currentUser()!.id, this.model()!.id, location, config)
      .pipe(finalize(() => this.isSubmitting.set(false)))
      .subscribe(() => {
        this.router.navigate(['/quotes']);
      });
  }
}
