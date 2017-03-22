import { Routes, RouterModule } from '@angular/router';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    {path: 'unauthorized', loadChildren: './pages/unauthorized/unauthorized.module#UnauthorizedModule'}
];

export const AppRoutes = RouterModule.forRoot(routes);
