import { Injectable } from '@angular/core';
import { QueryResponse } from './../shared/models/query.response';
import { ApiService } from './../shared/services/api.service';
import { AtualizarCategoriaProdutoRequest } from './models/atualizar-categoria-produto.request';
import { CategoriaProduto } from './models/categoria-produto.model';

import { CriarCategoriaProdutoRequest } from './models/criar-categoria-produto.request';

const url = 'api/v1/categorias-produtos';

@Injectable()
export class CategoriasProdutosService {
  constructor(private apiService: ApiService) {}

  criar(request: CriarCategoriaProdutoRequest) {
    return this.apiService.post(url, request);
  }

  atualizar(
    categoriaProdutoId: number,
    request: AtualizarCategoriaProdutoRequest
  ) {
    return this.apiService.put(`${url}/${categoriaProdutoId}`, request);
  }

  remover(categoriaProdutoId: number) {
    return this.apiService.delete(`${url}/${categoriaProdutoId}`);
  }

  obterPorId(categoriaProdutoId: number) {
    return this.apiService.get<CategoriaProduto>(
      `${url}/${categoriaProdutoId}`
    );
  }

  listarTodas() {
    return this.apiService.get<QueryResponse<CategoriaProduto>>(url);
  }
}
