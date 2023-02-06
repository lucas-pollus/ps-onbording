import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoriasProdutosComponent } from './component/categorias-produtos.component';

const routes: Routes = [
  {
    path: '',
    title: 'Cadastro de Categorias de Produtos',
    component: CategoriasProdutosComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CategoriasProdutosRoutingModule {}
