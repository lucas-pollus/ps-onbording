import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'categorias-produtos',
    loadChildren: () =>
      import('./categorias-produtos/categorias-produtos.module').then(
        (m) => m.CategoriasProdutosModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
