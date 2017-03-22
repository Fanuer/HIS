import { RecipeListComponent } from './components/recipe-list.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    { path: '', component: RecipeListComponent }
];

export const RecipeRoutes = RouterModule.forChild(routes);