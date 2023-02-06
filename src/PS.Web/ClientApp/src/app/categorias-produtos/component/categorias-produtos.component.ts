import { Component, OnInit, ViewChild } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { CategoriaProduto } from './../models/categoria-produto.model';

import {
  PoModalComponent,
  PoTableAction,
  PoTableColumn,
  PoTableComponent,
} from '@po-ui/ng-components';
import { CategoriasProdutosService } from './../categorias-produtos.service';

@Component({ templateUrl: './categorias-produtos.component.html' })
export class CategoriasProdutosComponent implements OnInit {
  form!: UntypedFormGroup;
  editando = false;
  items: CategoriaProduto[] = [];
  private _itemSelecionado!: CategoriaProduto;
  @ViewChild(PoModalComponent, { static: true }) poModal!: PoModalComponent;
  tituloModal = 'Nova Categoria de Produto';
  @ViewChild(PoTableComponent, { static: true }) poTable!: PoTableComponent;

  columns: PoTableColumn[] = [];
  actions: PoTableAction[] = [];

  private _formDefaultValue = {
    descricao: '',
  };

  constructor(
    private service: CategoriasProdutosService,
    private fb: UntypedFormBuilder
  ) {}

  ngOnInit(): void {
    this._criarForm();
    this._configurarColunas();
    this._configurarAcoes();
    this.listar();
  }

  private _criarForm() {
    this.form = this.fb.group({
      descricao: [this._formDefaultValue.descricao, Validators.required],
    });
  }

  private _configurarColunas() {
    this.columns = [
      {
        property: 'id',
        label: 'Id',
        width: '5%',
      },
      {
        property: 'descricao',
        type: 'string',
        label: 'Descrição',
      },
    ];
  }

  private _configurarAcoes() {
    this.actions = [
      { action: this.editar.bind(this), label: 'Editar', icon: 'fa fa-pencil' },
      {
        action: this.remover.bind(this),
        label: 'Remover',
        icon: 'fa fa-trash-can',
      },
    ];
  }

  private _criar() {
    const req = this.form.getRawValue();
    this.service.criar(req).subscribe((categoriaCriada) => {
      this.items.push(categoriaCriada);
      this.items = [...this.items];
    });
  }

  abrirModal() {
    this.editando
      ? (this.tituloModal = 'Editar Categoria de Produto')
      : (this.tituloModal = 'Nova Categoria de Produto');

    this.poModal.open();
  }

  fecharModal() {
    if (this.editando) {
      this.editando = false;
    }
    this.poModal.close();
  }

  private _atualizar() {
    const req = this.form.getRawValue();
    this.service.atualizar(this._itemSelecionado.id, req).subscribe(() => {
      const index = this.items.findIndex(
        (item) => item.id === this._itemSelecionado.id
      );
      const itemAtualizado = { ...req, id: this._itemSelecionado.id };
      this.poTable.updateItem(index, itemAtualizado);
      this.items = [...this.poTable.items];
      this.editando = false;
    });
  }

  salvar() {
    this.editando ? this._atualizar() : this._criar();
    this.form.reset({
      ...this._formDefaultValue,
    });
    this.fecharModal();
  }

  remover(item: CategoriaProduto) {
    this.service.remover(item.id).subscribe(() => {
      const index = this.items.findIndex((el) => el.id === item.id);
      this.poTable.removeItem(index);
      this.items = [...this.poTable.items];
    });
  }

  editar(item: CategoriaProduto) {
    this.editando = true;
    this._itemSelecionado = item;
    this.form.patchValue({
      descricao: item.descricao,
    });
    this.abrirModal();
  }

  listar() {
    this.service
      .listarTodas()
      .subscribe((result) => (this.items = result.values));
  }
}
