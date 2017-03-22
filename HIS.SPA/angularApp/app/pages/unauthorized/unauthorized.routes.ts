import { Routes, RouterModule } from '@angular/router';

import { UnauthorizedComponent } from './components/unauthorized.component';

const routes: Routes = [
    { path: '', component: UnauthorizedComponent }
];

export const UnauthorizedRoutes = RouterModule.forChild(routes);