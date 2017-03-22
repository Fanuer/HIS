import { RecipeRoutes } from './recipe.routes';
import { RecipeListComponent } from './components/recipe-list.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [
        CommonModule,
        RecipeRoutes
    ],

    declarations: [
        RecipeListComponent
    ],
})

export class UnauthorizedModule { }