import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { CategoriasProdutosRoutingModule } from './categorias-produtos-routing.module';
import { CategoriasProdutosService } from './categorias-produtos.service';

import { CategoriasProdutosComponent } from './component/categorias-produtos.component';

@NgModule({
  imports: [SharedModule, CategoriasProdutosRoutingModule],
  declarations: [CategoriasProdutosComponent],
  providers: [CategoriasProdutosService],
})
export class CategoriasProdutosModule {}
