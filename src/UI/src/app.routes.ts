import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { SolarSystemCatalogComponent } from './components/catalog/solar-system-catalog.component';
import { QuoteRequestFormComponent } from './components/quote-form/quote-request-form.component';
import { CustomerQuotesComponent } from './components/customer-quotes/customer-quotes.component';
import { OrderPlacementComponent } from './components/order-placement/order-placement.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { AdminModelFormComponent } from './components/admin-model-form/admin-model-form.component';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';

export const APP_ROUTES: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegistrationComponent },
  { 
    path: 'catalog', 
    component: SolarSystemCatalogComponent,
    canActivate: [authGuard] 
  },
  { 
    path: 'request-quote/:modelId', 
    component: QuoteRequestFormComponent,
    canActivate: [authGuard] 
  },
  { 
    path: 'quotes', 
    component: CustomerQuotesComponent,
    canActivate: [authGuard] 
  },
  { 
    path: 'order/:quoteId', 
    component: OrderPlacementComponent,
    canActivate: [authGuard] 
  },
  { 
    path: 'admin',
    canActivate: [adminGuard],
    children: [
      { path: '', component: AdminDashboardComponent, pathMatch: 'full' },
      { path: 'model/new', component: AdminModelFormComponent },
      { path: 'model/edit/:id', component: AdminModelFormComponent }
    ]
  },
  { path: '', redirectTo: '/catalog', pathMatch: 'full' },
  { path: '**', redirectTo: '/catalog' }
];
