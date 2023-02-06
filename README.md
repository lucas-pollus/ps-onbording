# PS.Onboarding

O _PS Onbording_ é um projeto simples para iniciação de novos desenvolvedores.

A `stack` utilizada é uma `webapi` construída em [dotnet](https://dotnet.microsoft.com). O cliente dessa `webapi` é uma aplicação construída em [angular](https://angular.io/). A arquitetura de programação utilizada nesse projeto é o `data-centric` onde a lógica de negócio está armazenada no banco de dados. O banco de dados escolhido é o [oracle](https://www.oracle.com/database/) utilizando a linguagem de programação [plsql](https://www.oracle.com/br/database/technologies/appdev/plsql.html).

Outro detalhe importante a se destacar é que utilizamos nesse projeto um arquitetura chamada [vertial slice](https://jimmybogard.com/vertical-slice-architecture/). Isso faz com que toda funcionalidade fique agrupada em um único ponto da aplicação.

## O projeto

O projeto consiste uma aplicação [crud](https://pt.wikipedia.org/wiki/CRUD) de cadastro de categorias de produtos. Posteriormente adicionaremos no projeto uma nova funcionalidade que é o cadastro de produtos.

As regras de negócios são:

- Não é permitido haver duas categorias com a mesma descrição
- A descrição deve ter no mínimo 3 caracteres
- A descrição deve ter no máximo 100 caracteres

## Executando o projeto

Para executar o projeto em seu computador, faça o clone do repositório:

```
git clone https://github.com/lucas-pollus/ps-onbording.git
```

Abra o projeto pelo [Visual Studio Code](https://code.visualstudio.com/) e abrir, escolha a opção de executar o `dev container`. Esse processo irá criar os containers de desenvolvimento e do banco de dados e inicializar o ambiente com todo ferramental necessário. Após a conclusão, será necessário instalar as dependências dos projeto.

Navegue até o diretório `src/PS.Web` e digite o seguinte comando:

```
dotnet restore
```

Depois navegue até o diretório `src/PS.Web.Test` e também digite o comando:

```
dotnet restore
```

Agora navegue até o diretório `src/PS.Web/ClientApp` e digite o seguinte comando:

```
npm install
```

Agora para executar o projeto, navegue até o diretório `src/PS.Web` e digite o comando:

```
dotnet run
```

Abra o navegador e digite a _url_ da aplicação [https://localhost:7141](https://localhost:7141). Automaticamente a aplicação irá executar `build` do projeto da `spa angular` e fará o redirecionamento para o endereço [https://localhost:44462](https://localhost:44462). Clique no item de menu `Categorias` e você poderá cadastar/editar uma categoria de produto.

## Adicionando uma nova feature

Agora vamos adicionar uma nova funcionalidade em nosso que será o cadastro de produtos. Abaixo algumas regras de negócio:

- Não é permitido haver dois produtos com a mesma descrição
- A descrição deve ter no mínimo 3 caracteres
- A descrição deve ter no máximo 100 caracteres
- O produto deve ter uma categoria associada

### Criando a tabela no banco de dados

Dentro do diretório `db/src/schema/tables` crie o arquivo `tprodutos.sql` com o seguinte conteúdo:

```sql
declare
  v_count pls_integer;
  v_cmd   varchar2(4000);
begin
  select count(*)
  into   v_count
  from   user_tables u
  where  u.table_name = upper('tprodutos');

  if v_count = 0 then
    v_cmd := 'create table tprodutos(
      id number(10) not null,
      descricao varchar2(100) not null,
      categoria_id number(10) not null,
      dt_criacao date default sysdate,
      dt_modificacao date default sysdate
    )';

    execute immediate v_cmd;

  end if;
end;
/
```

Execute o script para criar a tabela.

Agora vamos adicionar uma `constraint` que representará a chave primária da nossa tabela. Crie outro arquivo no mesmo diretório com o nome `tprodutos_pk.sql` com o seguinte conteúdo:

```sql
declare
  v_count pls_integer;
  v_cmd   varchar2(4000);
begin
  select count(*)
  into   v_count
  from   user_tables u
  where  u.table_name = upper('tprodutos');

  if v_count = 0 then
    utl_error.raise('Falha ao executa script: não existe tabela.');
  end if;

  select count(*)
  into   v_count
  from   user_constraints u
  where  u.constraint_name = upper('tprodutos_pk');

  if v_count = 0 then
    v_cmd := '
      alter table tprodutos
      add constraint tprodutos_pk primary key (id)
    ';

    execute immediate v_cmd;
  end if;
end;
/
```

Agora vamos adicionar uma nova `constraint` que representará a chave única para não permitir a inclusão de um produto com descrição duplicada. Crie o arquivo `tprodutos_uk.sql` com o seguinte conteúdo:

```sql
declare
  v_count pls_integer;
  v_cmd   varchar2(4000);
begin
  select count(*)
  into   v_count
  from   user_tables u
  where  u.table_name = upper('tprodutos');

  if v_count = 0 then
    utl_error.raise('Falha ao executa script: não existe tabela.');
  end if;

  select count(*)
  into   v_count
  from   user_constraints u
  where  u.constraint_name = upper('tprodutos_uk');

  if v_count = 0 then
    v_cmd := '
      alter table tprodutos
      add constraint tprodutos_uk unique (descricao)
    ';

    execute immediate v_cmd;
  end if;
end;
/
```

Por fim, vamos adicionar uma `constraint` onde vamos referenciar a coluna `categoria_id` com a tabela `tcategorias_produtos`. Crie o arquivo `tprodutos_fk_001.sql` com o seguinte conteúdo:

```sql
declare
  v_count pls_integer;
  v_cmd   varchar2(4000);
begin
  select count(*)
  into   v_count
  from   user_tables u
  where  u.table_name = upper('tprodutos');

  if v_count = 0 then
    utl_error.raise('Falha ao executa script: não existe tabela.');
  end if;

  select count(*)
  into   v_count
  from   user_constraints u
  where  u.constraint_name = upper('tprodutos_fk_001');

  if v_count = 0 then
    v_cmd := '
      alter table tprodutos
      add constraint tprodutos_fk_001 foreign key (categoria_id)
      references tcategorias_produtos (id)
    ';

    execute immediate v_cmd;
  end if;
end;
/
```

Agora, navegue até o diretório `db/src/schema/sequences` e crie um arquivo com o nome `seq_id_tprodutos.sql`. Esse será o sequenciador para coluna `id` da nossa tabela de produtos. O conteúdo é o seguinte:

```sql
declare
  v_count pls_integer;
  v_cmd   varchar2(4000);
begin
  select count(*)
  into   v_count
  from   user_sequences u
  where  u.sequence_name = upper('seq_id_tprodutos');

  if v_count = 0 then
    v_cmd := '
      create sequence seq_id_tprodutos
      minvalue 1
      maxvalue 999999999999999999999999999
      start with 1
      increment by 1
      nocache
    ';

    execute immediate v_cmd;
  end if;
end;
/
```

Lembre-se de executar os scripts para criação dos objetos no banco de dados.

### Criando o pacote de operação CRUD

Dentro do diretório `db/src/plsql/app` crie dois arquivos. O primeiro com o nome `pkg_app_produtos.pks.sql` e segundo `pkg_app_produtos.pkb.sql`.

O arquivo `pkg_app_produtos.pks.sql` será a especificação das operações CRUD que nossa aplicação irá executar. O conteúdo deve ser como desmonstrado abaixo:

```sql
create or replace package app_produtos
is
  function existe(
    pi_id tprodutos.id%type
  ) return boolean;

  function obter_por_id(
    pi_id tprodutos.id%type
  ) return sys_refcursor;

  function criar(
    pi_descricao tprodutos.descricao%type,
    pi_categoria_id tprodutos.categoria_id%type
  ) return sys_refcursor;

  procedure atualizar(
    pi_id        tprodutos.id%type,
    pi_descricao tprodutos.descricao%type,
    pi_categoria_id tprodutos.categoria_id%type
  );

  procedure remover(pi_id tprodutos.id%type);

  procedure listar_todas(
    pi_page       utl_query.t_page,
    pi_page_size  utl_query.t_page_size,
    po_result     out sys_refcursor,
    po_info_query out sys_refcursor
  );

end app_produtos;
/
```

O conteúdo do segundo arquivo `pkg_app_produtos.pkb.sql` será a implementação dessas operações. Vamos fazer uma por uma:

#### Function `existe`:

```sql
create or replace package body app_produtos
is

  function existe(
    pi_id tprodutos.id%type
  ) return boolean
  is
    v_existe pls_integer;
  begin
    select count(*)
    into   v_existe
    from   tprodutos
    where  id = pi_id;

    if v_existe > 0 then
      return true;
    end if;

    return false;
  end existe;

end app_produtos;
/
```

#### Function `obter_por_id`:

```sql
create or replace package body app_produtos
is
  -- codigo omitido

  function obter_por_id(
    pi_id tprodutos.id%type
  ) return sys_refcursor
  is
    v_result sys_refcursor;
  begin
    open v_result
    for select id
             , descricao
        from   tprodutos
        where  id = pi_id;

    return v_result;
  end obter_por_id;

end app_produtos;
/
```

#### Function `criar`:

```sql
create or replace package body app_produtos
is
  -- codigo omitido

  function criar(
    pi_descricao    tprodutos.descricao%type,
    pi_categoria_id tprodutos.categoria_id%type
  ) return sys_refcursor
  is
    v_id tprodutos.id%type;
  begin
    insert into tprodutos
    (id
    ,descricao
    ,categoria_id
    )
    values
    (seq_id_tprodutos.nextval
    ,pi_descricao
    ,pi_categoria_id
    )
    returning id into v_id;

    return obter_por_id(v_id);

  exception
    when dup_val_on_index then
      utl_error.raise('A descrição informada já existe.');
  end criar;
end app_produtos;
/
```

#### Procedure `atualizar`:

```sql
create or replace pacakge body app_produtos
is
  -- codigo omitido

  procedure atualizar(
    pi_id           tprodutos.id%type,
    pi_descricao    tprodutos.descricao%type,
    pi_categoria_id tprodutos.categoria_id%type
  )
  is
  begin
    update tprodutos p
    set    p.descricao    = pi_descricao
         , p.categoria_id = pi_categoria_id
         , p.dt_modificacao = sysdate
    where  p.id = pi_id;

  exception
    when dup_val_on_index then
      utl_error.raise('A descrição informada já existe.');
  end atualizar;
end app_produtos;
/
```

#### Procedure `remover`:

```sql
create or replace package body app_produtos
is
  --codigo omitido

  procedure remover(pi_id tprodutos.id%type)
  is
  begin
    delete from tprodutos where id = pi_id;
  end remover;

end app_produtos;
/
```

#### Procedure `listar_todos`:

```sql
create or replace package body app_produtos
is
  -- codigo omitido

  procedure listar_todos(
    pi_page       utl_query.t_page,
    pi_page_size  utl_query.t_page_size,
    po_result     out sys_refcursor,
    po_info_query out sys_refcursor
  )
  is
    v_sql utl_query.t_sql_query;
  begin
    v_sql := 'select id, descricao, categoria_id from tprodutos';

    utl_query.execute_query(
      v_sql,
      pi_page,
      pi_page_size,
      po_result,
      po_info_query
    );

  end listar_todos;
end app_produtos;
/
```

### Implementando o Repositório das operações CRUD:

Dentro de `src/PS.Web/Features` crie um novo diretório `Produtos` e os seguintes sub-diretórios: `Endpoints` e `Models`.

Dentro do diretório `Models` crie o arquivo `ProdutoOutput.cs` com o seguinte conteúdo:

```csharp
namespace PS.Web.Features.Produtos.Models;

public record ProdutoOutput
{
    public int Id { get; init; }
    public string Descricao { get; init; }
    public int CategoriaId { get; init; }
}
```

Crie outro arquivo com nome `CriarProdutoCommand.cs` com o seguinte conteúdo:

```csharp
namespace PS.Web.Features.Produtos.Models;

public record CriarProdutoCommand
{
    public string Descricao { get; init; }
    public int CategoriaId { get; init; }
}
```

Crie outro arquivo com nome `AtualizarProdutoCommand.cs` com o seguinte conteúdo:

```csharp
namespace PS.Web.Features.Produtos.Models;

public record AtualizarProdutoCommand
{
    public string Descricao { get; set; }
    public int CategoriaId { get; init; }
}
```

Por fim, crie outro arquivo com nome `ListarTodosProdutosOutput.cs` com o seguinte conteúdo:

```csharp
using PS.Data.Models;

namespace PS.Web.Features.Produtos.Models;

public record ListarTodosProdutosOutput : IQueryOutput
{
    public int Id { get; init; }
    public string Descricao { get; set; }
    public int CategoriaId { get; set; }
}
```

Agora dentro do diretório `src/PS.Web/Features/Produtos` crie um arquivo com nome `IRepositorioProdutos.cs` com o seguinte conteúdo:

```csharp
using PS.Data.Models;
using PS.FluentResult;
using PS.Web.Features.Produtos.Models;

namespace PS.Web.Features.Produtos;

public interface IRepositorioProdutos
{
    Task<Result<ProdutoOutput>> Criar(CriarProdutoCommand cmd);
    Task<Result> Atualizar(int id, AtualizarProdutoCommand cmd);
    Task<Result<ProdutoOutput>> ObterPorId(int id);
    Task<Result> Remover(int id);
    Task<Result<QueryOutput<ListarTodosProdutosOutput>>> ListarTodos(QueryInput query);
}
```

Crie o arquivo `RepositorioProdutos.cs` para implementar as chamados do banco de dados:

```csharp
using PS.Data;
using PS.Data.Models;
using PS.FluentResult;
using PS.Web.Features.Produtos.Models;

namespace PS.Web.Features.Produtos;

public sealed class RepositorioProdutos : IRepositorioProdutos
{
    private readonly IDbHelper dbHelper;

    public RepositorioProdutos(IDbHelper dbHelper)
    {
        this.dbHelper = dbHelper;
    }

    public Task<Result> Atualizar(int id, AtualizarProdutoCommand cmd)
    {
        return this.dbHelper.ExecuteAsync(
            "app_produtos.atualizar",
            new
            {
                pi_id = id,
                pi_descricao = cmd.Descricao,
                pi_categoria_id = cmd.CategoriaId
            }
        );
    }

    public Task<Result<ProdutoOutput>> Criar(CriarProdutoCommand cmd)
    {
        return this.dbHelper.CreateAsync<ProdutoOutput>(
            "app_produtos.criar",
            new
            {
                pi_descricao = cmd.Descricao,
                pi_categoria_id = cmd.CategoriaId
            }
        );
    }

    public Task<Result<QueryOutput<ListarTodosProdutosOutput>>> ListarTodos(QueryInput query)
    {
        return this.dbHelper.PagedList<ListarTodosProdutosOutput>(
            "app_produtos.listar_todos",
            new
            {
                pi_page = query.Page,
                pi_page_size = query.PageSize
            }
        );
    }

    public Task<Result<ProdutoOutput>> ObterPorId(int id)
    {
        return this.dbHelper.GetAsync<ProdutoOutput>(
            "app_produtos.obter_por_id",
            new
            {
                pi_id = id
            }
        );
    }

    public Task<Result> Remover(int id)
    {
        return this.dbHelper.ExecuteAsync(
            "app_produtos.remover",
            new
            {
                pi_id = id
            }
        );
    }
}
```

Edite o arquivo `AddRepositoriosServices.cs` dentro de `src/PS.Web/Services` e adicione a injeção de depedência do repositório:

```csharp
// codigo omitido
using PS.Web.Features.Produtos;

namespace PS.Web.Services;

public static partial class ServicesExtensions
{
    public static void AddRepositoriosServices(this IServiceCollection services)
    {
        // codigo omitido
        services.AddScoped<IRepositorioProdutos, RepositorioProdutos>();
    }
}
```

### Implementando os `endpoints`:

Dentro do diretório `src/PS.Web/Features/Produtos/Endpoints` crie o arquivo `CriarCategoriaProdutoEndpoint.cs` com o seguinte conteúdo:

```csharp
using FastEndpoints;
using FluentValidation;
using PS.Web.Features.Produtos.Models;

namespace PS.Web.Features.Produtos.Endpoints;
```

Adicione o `record` que irá representar a requisição para criação do novo produto:

```csharp
public record CriarProdutoRequest
{
    public string Descricao { get; init; }
    public int CategoriaId { get; init; }

    public static implicit operator CriarProdutoCommand(CriarProdutoRequest req)
    {
        return new()
        {
            Descricao = req.Descricao,
            CategoriaId = req.CategoriaId
        };
    }
}
```

Após isso adicione a `class` que fará algumas validações:

```csharp
public class CriarProdutoValidator : Validator<CriarProdutoRequest>
{
    public CriarProdutoValidator()
    {
        RuleFor(p => p.Descricao)
            .NotEmpty()
            .WithMessage("A descrição não pode ser vazio.")
            .MinimumLength(3)
            .WithMessage("A descrição deve ter no mínimo 3 caracteres.")
            .MaximumLength(100)
            .WithMessage("A descrição deve ter no máximo 100 caracteres.");
    }
}
```

Agora adicione o `record` que irá representar a resposta para nossa requisição em caso de sucesso:

```csharp
public record CriarProdutoResponse
{
    public int Id { get; init; }
    public string Descricao { get; init; }
    public int CategoriaId { get; init; }

    public static implicit operator CriarProdutoResponse(ProdutoOutput dto)
    {
        return new()
        {
            Id = dto.Id,
            Descricao = dto.Descricao,
            CategoriaId = dto.CategoriaId
        };
    }
}
```

Por fim, no topo do arquivo, adicione a `class` que será nosso `endpoint`:

```csharp
public class CriarProdutoEndpoint : Endpoint<CriarProdutoRequest, CriarProdutoResponse>
{
    private readonly IRepositorioProdutos repo;

    public CriarCategoriaProdutoEndpoint(IRepositorioProdutos repo)
    {
        this.repo = repo;
    }

    public override void Configure()
    {
        Post("/api/v1/produtos");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CriarProdutoRequest req, CancellationToken ct)
    {
        var result = await repo.Criar(req);
        if (result.IsFailure)
            ThrowError(result.Error);

        await SendOkAsync(result.Value, ct);
    }
}
```

Nosso próximo `endpoint` será para atualizar o produto, crie o arquivo `AtualizarProdutoEndpoint.cs`:

```csharp
using FastEndpoints;
using PS.Web.Features.Produtos.Models;

namespace PS.Web.Features.Produtos.Endpoints;
```

Adicione o `record` que irá representar a requisição para atualizar o produto:

```csharp
public record AtualizarProdutoRequest
{
    public int ProdutoId { get; init; }
    public string Descricao { get; set; }
    public string CategoriaId { get; set; }

    public static implicit operator AtualizarProdutoCommand(AtualizarProdutoRequest req)
    {
        return new()
        {
            Descricao = req.Descricao,
            CategoriaId = req.CategoriaId
        };
    }
}
```

Por fim, no topo do arquivo, adicione a `class` que será nosso `endpoint`:

```csharp
public class AtualizarProdutoEndpoint : Endpoint<AtualizarProdutoRequest>
{
    private readonly IRepositorioProdutos repo;

    public AtualizarProdutoEndpoint(IRepositorioProdutos repo)
    {
        this.repo = repo;
    }

    public override void Configure()
    {
        Put("/api/v1/produtos/{ProdutoId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AtualizarCategoriaProdutoRequest req, CancellationToken ct)
    {
        var result = await repo.Atualizar(req.ProdutoId, req);
        if (result.IsFailure)
            ThrowError(result.Error);

        await SendNoContentAsync(ct);
    }
}
```

Próximo `endpoint` será para obter um produto por id. Crie o arquivo `ObterProdutoPorId.cs`:

```csharp
using FastEndpoints;
using PS.Web.Features.Produtos.Models;

namespace PS.Web.Features.Produtos.Endpoints;
```

Adicione o `record` que irá representar a resposta da nossa requisição:

```csharp
public record ObterProdutoPorIdResponse
{
    public int Id { get; init; }
    public string Descricao { get; init; }
    public int CategoriaId { get; init; }

    public static implicit operator ObterProdutoPorIdResponse(ProdutoOutput dto)
    {
        return new()
        {
            Id = dto.Id,
            Descricao = dto.Descricao,
            CategoriaId = dto.CategoriaId
        };
    }
}
```

No topo do arquivo, adicione a `class` que será nosso `endpoint`:

```csharp
public class ObterProdutoPorIdEndpoint : EndpointWithoutRequest<ObterProdutoPorIdResponse>
{
    private readonly IRepositorioProdutos repo;

    public ObterProdutoPorIdEndpoint(IRepositorioProdutos repo)
    {
        this.repo = repo;
    }

    public override void Configure()
    {
        Get("/api/v1/produtos/{ProdutoId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var produtoId = Route<int>("ProdutoId");
        var result = await repo.ObterPorId(produtoId);
        if (result.IsFailure)
            ThrowError(result.Error);

        await SendOkAsync(result.Value, ct);
    }
}
```

Agora o `endpoint` que irá remover um produto ao passar seu id. Crie o arquivo `RemoverProdutoEndpoint.cs`:

```csharp
using FastEndpoints;

namespace PS.Web.Features.Produtos.Endpoints;

public class RemoverProdutoEndpoint : EndpointWithoutRequest
{
    private readonly IRepositorioProdutos repo;

    public RemoverProdutoEndpoint(IRepositorioProdutos repo)
    {
        this.repo = repo;
    }

    public override void Configure()
    {
        Delete("/api/v1/produtos/{ProdutoId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var produtoId = Route<int>("ProdutoId");
        var result = await repo.Remover(produtoId);
        if (result.IsFailure)
            ThrowError(result.Error);

        await SendNoContentAsync(ct);
    }
}
```

Por último, adicione o `endpoint` que irá listar todos os produtos com paginação:

```csharp
using FastEndpoints;
using PS.Web.Features.Produtos.Models;
using PS.Web.Models;

namespace PS.Web.Features.Produtos.Endpoints;
```

Adicione o `record` que irá representar a resposta da nossa requisição:

```csharp
public record ListarTodosProdutosResponse
{
    public int Id { get; init; }
    public string Descricao { get; init; }
    public int CategoriaId { get; init; }

    public static implicit operator ListarTodosProdutosResponse(ListarTodosProdutosOutput dto)
    {
        return new()
        {
            Id = dto.Id,
            Descricao = dto.Descricao,
            CategoriaId = dto.CategoriaId
        };
    }
}
```

No topo do arquivo, adicione `class` que representará nosso `endpoint`:

```csharp
public class ListarTodosProdutosEndpoint : Endpoint<QueryRequest, QueryResponse<ListarTodosProdutosResponse>>
{
    private readonly IRepositorioProdutos repo;

    public ListarTodosProdutosEndpoint(IRepositorioProdutos repo)
    {
        this.repo = repo;
    }

    public override void Configure()
    {
        Get("/api/v1/produtos");
        AllowAnonymous();
    }

    public override async Task HandleAsync(QueryRequest req, CancellationToken ct)
    {
        var result = await repo.ListarTodos(req);
        if (result.IsFailure)
            ThrowError(result.Error);

        var response = result.Value.ToResponse<ListarTodosProdutosOutput, ListarTodosProdutosResponse>();

        await SendOkAsync(response, ct);

    }
}
```

### Adicionando testes

Dentro do diretório `src/PS.Web.Test/Features` crie o diretório `Produtos`. Após isso crie o arquivo `CriarProdutoEndpointTest.cs`:

```csharp
using PS.Web.Features.CategoriasProdutos;
using PS.Web.Features.CategoriasProdutos.Models;
using PS.Web.Features.Produtos;
using PS.Web.Features.Produtos.Endpoints;
using PS.Web.Models;

namespace PS.Web.Test.Features.Produtos;

public class CriarProdutoEndpointTest : BaseTest
{
    private IRepositorioCategoriasProdutos repoCategoria;
    private int categoriaId;
    private int id;

    [OneTimeSetUp]
    public async Task Setup()
    {
        repoCategoria = serviceProvider.GetService<IRepositorioCategoriasProdutos>();
        var cmd = new CriarCategoriaProdutoCommand()
        {
            Descricao = "Cadeiras - Test"
        };

        var result = await repoCategoria.Criar(cmd);
        result.EnsureSuccess();
        categoriaId = result.Value.Id;
    }

    [Test]
    public async Task StatusIsOk()
    {
        var model = new CriarProdutoRequest()
        {
            Descricao = "Cadeira Lia LX - Test",
            CategoriaId = categoriaId
        };

        var response = await client
            .Request("api/v1/produtos")
            .PostJsonAsync(model);

        response.StatusCode.Should().Be((int)HttpStatusCode.OK);

        var body = await response
            .GetJsonAsync<CriarProdutoResponse>();

        id = body.Id;

        body.Descricao.Should().BeEquivalentTo(model.Descricao);
        body.CategoriaId.Should().Be(model.CategoriaId);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        var repo = serviceProvider.GetService<IRepositorioProdutos>();
        await repo.Remover(id);
        await repoCategoria.Remover(categoriaId);
    }
}

public class FalhaAoCriarProdutoEndpointTest : BaseTest
{
    [Test]
    public async Task StatusIsBadRequest()
    {
        var modelInvalido = new CriarProdutoRequest()
        {
            Descricao = "AB", // Descrição inválida
            CategoriaId = 0 // Categoria inválida
        };

        var response = await client
            .Request("api/v1/produtos")
            .AllowAnyHttpStatus()
            .PostJsonAsync(modelInvalido);

        response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        var body = await response.GetJsonAsync<ErrorOutput>();
        body.DetailedMessage.Should().ContainAny("A descrição deve ter no mínimo 3 caracteres.");
    }
}
```

Agora crie o arquivo `AtualizarProdutoEndpointTest.cs`:

```csharp
using PS.Web.Features.CategoriasProdutos;
using PS.Web.Features.CategoriasProdutos.Models;
using PS.Web.Features.Produtos;
using PS.Web.Features.Produtos.Endpoints;

namespace PS.Web.Test.Features.Produtos;

public class AtualizarProdutoEndpointTest : BaseTest
{
    private IRepositorioCategoriasProdutos repoCategoria;
    private IRepositorioProdutos repo;
    private int categoriaId;
    private int id;

    [OneTimeSetUp]
    public async Task Setup()
    {
        repoCategoria = serviceProvider.GetService<IRepositorioCategoriasProdutos>();
        var cmd = new CriarCategoriaProdutoCommand()
        {
            Descricao = "Cadeiras - Test"
        };
        var result = await repoCategoria.Criar(cmd);
        result.EnsureSuccess();
        categoriaId = result.Value.Id;

        repo = serviceProvider.GetService<IRepositorioProdutos>();
        var cmd2 = new CriarProdutoCommand()
        {
            Descricao = "Cadeira Lia LX - Test",
            CategoriaId = categoriaId
        };
        var result2 = await repo.Criar(cmd2);
        result2.EnsureSuccess();
        id = result2.Value.Id;

    }

    [Test]
    public async Task Success()
    {
        var model = new AtualizarProdutoRequest()
        {
            Descricao = "Cadeira Lia LX Atualizado - Test",
            CategoriaId = categoriaId
        };

        var response = await client
            .Request("api/v1/produtos")
            .AppendPathSegment(id)
            .PutJsonAsync(model);

        response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        var result = await repo.ObterPorId(id);

        result.Value.Descricao.Should().BeEquivalentTo(model.Descricao);
        result.Value.CategoriaId.Should().Be(model.CategoriaId);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await repo.Remover(id);
        await repoCategoria.Remover(categoriaId);
    }
}
```

Próximo passo, vamos criar arquivo `ListarTodosProdutosEndpointTest.cs`:

```csharp
using PS.Web.Features.CategoriasProdutos;
using PS.Web.Features.CategoriasProdutos.Models;
using PS.Web.Features.Produtos;
using PS.Web.Features.Produtos.Models;
using PS.Web.Features.Produtos.Endpoints;
using PS.Web.Models;

namespace PS.Web.Test.Features.Produtos;

public class ListarTodosProdutosEndpointTest : BaseTest
{
    private IRepositorioCategoriasProdutos repoCategoria;
    private IRepositorioProdutos repo;
    private int categoriaId;
    private int id;

    [OneTimeSetUp]
    public async Task Setup()
    {
        repoCategoria = serviceProvider.GetService<IRepositorioCategoriasProdutos>();
        var cmd = new CriarCategoriaProdutoCommand()
        {
            Descricao = "Cadeiras - Test"
        };
        var result = await repoCategoria.Criar(cmd);
        result.EnsureSuccess();
        categoriaId = result.Value.Id;

        repo = serviceProvider.GetService<IRepositorioProdutos>();
        var cmd2 = new CriarProdutoCommand()
        {
            Descricao = "Cadeira Lia LX - Test",
            CategoriaId = categoriaId
        };
        var result2 = await repo.Criar(cmd2);
        result2.EnsureSuccess();
        id = result2.Value.Id;

    }

    [Test]
    public async Task StatusIsOk()
    {
        var response = await client
            .Request("api/v1/produtos")
            .GetAsync();

        response.StatusCode.Should().Be((int)HttpStatusCode.OK);

        var body = await response.GetJsonAsync<QueryResponse<ListarTodosProdutosResponse>>();
        body.EnsureValuesContainsRows();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await repo.Remover(id);
        await repoCategoria.Remover(id);
    }
}
```

Agora crie o arquivo `ObterProdutoPorIdEndpointTest`:

```csharp
using PS.Web.Features.CategoriasProdutos;
using PS.Web.Features.CategoriasProdutos.Models;
using PS.Web.Features.Produtos;
using PS.Web.Features.Produtos.Models;
using PS.Web.Features.Produtos.Endpoints;

namespace PS.Web.Test.Features.Produtos;

public class ObterProdutoPorIdEndpointTest : BaseTest
{
    private IRepositorioCategoriasProdutos repoCategoria;
    private IRepositorioProdutos repo;
    private int categoriaId;
    private int id;

    [OneTimeSetUp]
    public async Task Setup()
    {
        repoCategoria = serviceProvider.GetService<IRepositorioCategoriasProdutos>();
        var cmd = new CriarCategoriaProdutoCommand()
        {
            Descricao = "Cadeiras - Test"
        };
        var result = await repoCategoria.Criar(cmd);
        result.EnsureSuccess();
        categoriaId = result.Value.Id;

        repo = serviceProvider.GetService<IRepositorioProdutos>();
        var cmd2 = new CriarProdutoCommand()
        {
            Descricao = "Cadeira Lia LX - Test",
            CategoriaId = categoriaId
        };
        var result2 = await repo.Criar(cmd2);
        result2.EnsureSuccess();
        id = result2.Value.Id;
    }

    [Test]
    public async Task StatusIsOk()
    {
        var response = await client
            .Request("api/v1/produtos")
            .AppendPathSegment(id)
            .GetAsync();

        response.StatusCode.Should().Be((int)HttpStatusCode.OK);

        var body = await response.GetJsonAsync<ObterProdutoPorIdResponse>();
        body.Id.Should().Be(id);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await repo.Remover(id);
        await repoCategoria.Remover(id);
    }
}
```

Agora crie o arquivo `RemoverProdutoEndpointTest.cs`:

```csharp
using PS.Web.Features.CategoriasProdutos;
using PS.Web.Features.CategoriasProdutos.Models;
using PS.Web.Features.Produtos;
using PS.Web.Features.Produtos.Models;

namespace PS.Web.Test.Features.Produtos;

public class RemoverProdutoEndpointTest : BaseTest
{
    private IRepositorioCategoriasProdutos repoCategoria;
    private IRepositorioProdutos repo;
    private int categoriaId;
    private int id;

    [OneTimeSetUp]
    public async Task Setup()
    {
        repoCategoria = serviceProvider.GetService<IRepositorioCategoriasProdutos>();
        var cmd = new CriarCategoriaProdutoCommand()
        {
            Descricao = "Cadeiras - Test"
        };
        var result = await repoCategoria.Criar(cmd);
        result.EnsureSuccess();
        categoriaId = result.Value.Id;

        repo = serviceProvider.GetService<IRepositorioProdutos>();
        var cmd2 = new CriarProdutoCommand()
        {
            Descricao = "Cadeira Lia LX - Test",
            CategoriaId = categoriaId
        };
        var result2 = await repo.Criar(cmd2);
        result2.EnsureSuccess();
        id = result.Value.Id;
    }

    [Test]
    public async Task Success()
    {
        var response = await client
            .Request("api/v1/produtos")
            .AppendPathSegment(id)
            .DeleteAsync();

        response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await repoCategoria.Remover(categoriaId);
    }
}
```

Para rodar os testes, dentro do diretório `src/PS.Web.Test/` execute o comando:

```
dotnet test
```

### Implementando o `front-end (SPA)`

Navegue até o diretório `src/PS.Web/ClientApp/src/app/` e crie um diretório `produtos`. Depois crie mais dois sub-diretórios: `component` e `models`.

Dentro de `models` crie o arquivo `produto.model.ts`:

```ts
export interface Produto {
  id: number;
  descricao: string;
  categoriaId: number;
}
```

Depois, crie o arquivo `criar-produto.request.ts`:

```ts
export interface CriarProdutoRequest {
  descricao: string;
  categoriaId: number;
}
```

Após isso, crie o arquivo `atualizar-produto.request.ts`:

```ts
export interface AtualizarProdutoRequest {
  descricao: string;
  categoriaId: number;
}
```

Agora, dentro do diretório acima (`produtos/`) crie o arquivo `produtos.service.ts`:

```ts
import { Injectable } from "@angular/core";
import { QueryResponse } from "./../shared/models/query.response";
import { ApiService } from "./../shared/services/api.service";

import { AtualizarProdutoRequest } from "./models/atualizar-produto.request";
import { Produto } from "./models/produto.model";

import { CriarProdutoRequest } from "./models/criar-produto.request";

const url = "api/v1/produtos";

@Injectable()
export class ProdutosService {
  constructor(private apiService: ApiService) {}

  criar(request: CriarProdutoRequest) {
    return this.apiService.post(url, request);
  }

  atualizar(produtoId: number, request: AtualizarProdutoRequest) {
    return this.apiService.put(`${url}/${produtoId}`, request);
  }

  remover(produtoId: number) {
    return this.apiService.delete(`${url}/${produtoId}`);
  }

  obterPorId(produtoId: number) {
    return this.apiService.get<Produto>(`${url}/${produtoId}`);
  }

  listarTodas() {
    return this.apiService.get<QueryResponse<Produto>>(url);
  }
}
```

Agora crie o `module` para agrupar os componentes e serviços de produto. Para isso dentro do diretório `produtos/` crie o arquivo `produtos.module.ts`:

```ts
/* ...imports necessários*/

@NgModule({
  imports: [SharedModule],
  declarations: [],
  providers: [ProdutosService],
})
export class ProdutosModule {}
```

Agora vamos criar nosso componente para cadastro de produtos. Para isso, dentro do diretório `produtos/component` crie o arquivo `produtos.component.ts` e `produtos.component.html`.

O conteúdo de `produtos.component.ts` será:

```ts
/* ...imports necessários*/

@Component({
  templateUrl: "./produtos.component.html",
})
export class ProdutosComponent {
  form!: UntypedFormGroup;
  editando = false;

  constructor(
    private service: ProdutosService,
    private fb: UntypedFormBuilder
  ) {}

  private _criarForm() {}

  private _criar();
  private _atualizar();

  salvar() {}

  remover() {}

  editar() {}

  listar() {}
}
```

Implemente o método privado `_criarForm()`. Crie um campo privado `_formDefaultValue` que representa os valores defaults do formulário. Coloque a chamada do método `_criarForm()` no `ngOnInit()`:

```ts
/* ...imports necessários*/

@Component({
  templateUrl: "./produtos.component.html",
})
export class ProdutosComponent implements OnInit {
  form!: UntypedFormGroup;
  editando = false;

  // Adicione um objeto que representa o form default
  private _formDefaultValue = {
    descricao: "",
    categoriaId: "",
  };

  constructor(
    private service: ProdutosService,
    private fb: UntypedFormBuilder
  ) {}

  ngOnInit() {
    this._criarForm();
  }

  private _criarForm() {
    this.form = this.fb.group({
      descricao: [this._formDefaultValue.descricao, Validators.required],
      categoriaId: [this._formDefaultValue.categoriaId, Validators.required],
    });
  }

  /* demais implementações*/
}
```

Implemente o método privado `_criar()`. Crie um campo público `items` do tipo `Produto[]`.

```ts
/* ...imports necessários*/

@Component({
    templateUrl: './produtos.component.html'
})
export class ProdutosComponent implements OnInit {
    form!: UntypedFormGroup;
    items: Produto[];
    /* ...demais campos*/

    constructor(
        private service: ProdutosService,
        private fb: UntypedFormBuilder
    ) {}

    ngOnInit() {...} // omitido

    private _criarForm() {...} // omitido

    private _criar() {
        const cmd = this.form.getRawValue();
        this.service.incluir(cmd).subscribe((produtoCriado) => {
            this.items.push(produtoCriado);
            this.items = [...this.items];
        });
    }

    /* demais implementações*/
}
```

Implemente os métodos públicos `abrirModal()` e `fecharModal()`. Adicione um campo público que irá referenciar o modal no `template`.

```ts
/* ...imports necessários*/

@Component({
    templateUrl: './produtos.component.html'
})
export class ProdutosComponent implements OnInit {
    editando = false;
    @ViewChild(PoModalComponent, { static: true }) poModal!: PoModalComponent;
    tituloModal = 'Novo Produto';
    /* ...demais campos*/

    constructor(
        private service: ProdutosService,
        private fb: UntypedFormBuilder
    ) {}

    ngOnInit() {...} // omitido

    private _criarForm() {...} // omitido

    private _criar() {...} // omitido

    abrirModal() {
        this.editando
        ? (this.tituloModal = 'Editar Produto')
        : (this.tituloModal = 'Novo Produto');

        this.poModal.open();
    }

    fecharModal() {
        if (this.editando) {
            this.editando = false;
        }
        this.poModal.close();
    }

    /* demais implementações*/
}
```

Implemente o método público `salvar()`:

```ts
/* ...imports necessários*/

@Component({
  templateUrl: "./produtos.component.html",
})
export class ProdutosComponent implements OnInit {
  form!: UntypedFormGroup;
  editando = false;
  @ViewChild(PoModalComponent, { static: true }) poModal!: PoModalComponent;
  tituloModal = "Novo Produto";
  /* ...demais campos*/

  constructor(
    private service: ProdutosService,
    private fb: UntypedFormBuilder
  ) {}

  ngOnInit() {
    /*...omitido*/
  }

  private _criarForm() {
    /*...omitido*/
  }

  private _criar() {
    /*...omitido*/
  }

  private _atualizar() {} //vazio

  salvar() {
    this.editando ? this._atualizar() : this._criar();
    this.form.reset({
      ...this._formDefaultValue,
    });
    this.fecharModal();
  }

  abrirModal() {
    /*...omitido*/
  }

  fecharModal() {
    /*...omitido*/
  }

  /* demais implementações*/
}
```

Implemente o template do `component` adicionando o formulário e o modal. Edite o arquivo `produtos.component.html`:

```html
<div class="po-row">
  <po-button
    class="po-md-1"
    p-label="Novo"
    p-icon="fa fa-plus"
    (p-click)="abrirModal()"
  ></po-button>
</div>
<po-divider></po-divider>
<!-- MODAL - FORM -->
<po-modal [p-title]="tituloModal" p-size="md" [p-click-out]="true">
  <form [formGroup]="form">
    <div class="po-row">
      <po-input
        class="po-md-12"
        formControlName="descricao"
        p-label="Descrição"
        p-required
      ></po-input>
    </div>
    <div class="po-row">
      <po-number
        class="po-md-4"
        formControlName="categoriaId"
        p-label="CategoriaId"
      ></po-number>
    </div>
  </form>
  <po-modal-footer [p-disabled-align]="false">
    <po-divider></po-divider>
    <po-button
      [p-danger]="true"
      p-icon="fa fa-right-from-bracket"
      p-label="Cancelar"
      (p-click)="fecharModal()"
    >
    </po-button>
    <po-button
      p-label="Salvar"
      p-icon="fa fa-floppy-disk"
      [p-disabled]="this.form.invalid"
      (p-click)="salvar()"
    >
    </po-button>
  </po-modal-footer>
</po-modal>
```

Implemente a tabela que irá exibir nossos produtos. Primeiro crie um campo público `columns` do tipo `PoTableColumn[]` e um método privado `_configurarColunas()`. Não esqueça de colocar a chamada da função dentro do método `ngOnInit()`:

```ts
/* ...imports necessários*/

@Component({
  templateUrl: "./produtos.component.html",
})
export class ProdutosComponent implements OnInit {
  columns!: PoTableColumn[];
  /* ...demais campos*/

  constructor(
    private service: ProdutosService,
    private fb: UntypedFormBuilder
  ) {}

  ngOnInit() {
    // demais chamadas omitidas;
    this._configurarColunas();
  }

  private _configurarColunas() {
    this.columns = [
      { property: "id", label: "Id", width: "10%" },
      {
        property: "descricao",
        type: "string",
        label: "Descrição",
        width: "50%",
      },
      {
        property: "categoriaId",
        type: "number",
        label: "CategoriaId",
        width: "30%",
      },
    ];
  }

  /* demais implementações*/
}
```

Implemente as `actions` de nossa tabela. Para isso crie um campo público com o nome de `actions` do tipo `PoTableAction[]` e um método privado `_configurarAcoes()`. Não se esqueça de colocar a chamada da função dentro do método `ngOnInit()`:

```ts
/* ...imports necessários*/

@Component({
  templateUrl: "./produtos.component.html",
})
export class ProdutosComponent implements OnInit {
  actions!: PoTableAction[];
  /* ...demais campos*/

  constructor(
    private service: ProdutosService,
    private fb: UntypedFormBuilder
  ) {}

  ngOnInit() {
    // demais chamadas omitidas;
    this._configurarAcoes();
  }

  private _configurarAcoes() {
    this.actions = [
      { action: this.editar.bind(this), label: "Editar", icon: "fa fa-pencil" },
      {
        action: this.remover.bind(this),
        label: "Remover",
        icon: "fa fa-trash-can",
      },
    ];
  }

  /* demais implementações*/
}
```

Implemente o método público `listar()`. Adicione a chamada dessa função dentro do `ngOnInit()`:

```ts
/* ...imports necessários*/

@Component({
  templateUrl: "./produtos.component.html",
})
export class ProdutosComponent implements OnInit {
  items: Produtos[];
  /* ...demais campos*/

  constructor(
    private service: ProdutosService,
    private fb: UntypedFormBuilder
  ) {}

  ngOnInit() {
    // demais chamadas omitidas;
    this.listar();
  }

  listar() {
    this.service
      .listarTodas()
      .subscribe((result) => (this.items = result.values));
  }

  /* demais implementações*/
}
```

Edite o arquivo de `template` e adicione a tabela:

```html
<!-- template modal e form omitido -->

<!-- TABLE -->
<po-table
  [p-actions]="actions"
  [p-columns]="columns"
  [p-items]="items"
  [p-height]="400"
  [p-actions-right]="true"
></po-table>
```

Implemente o método público `editar()`. Para isso adicione um campo privado para guardar o item selecionado com nome `_itemSelecionado` do tipo `Produto`:

```ts
/* ...imports necessários*/

@Component({
  templateUrl: "./produtos.component.html",
})
export class ProdutosComponent implements OnInit {
  private _itemSelecionado: Produto;
  /* ...demais campos*/

  constructor(
    private service: ProdutosService,
    private fb: UntypedFormBuilder
  ) {}

  editar(item: Produto) {
    this.editando = true;
    this._itemSelecionado = item;
    this.form.patchValue({
      descricao: item.descricao,
      categoriaId: item.categoriaId,
    });
    this.abrirModal();
  }

  /* demais implementações*/
}
```

Implemente o método privado `_atualizar()`. Esse método irá atualizar o item selecionado no back-end, e atualizará também o item na tabela. Adicione um campo público que será a referência a tabela. Para isso utilize a anotação `@ViewChild`:

```ts
/* ...imports necessários*/

@Component({
  templateUrl: "./produtos.component.html",
})
export class ProdutosComponent implements OnInit {
  @ViewChild(PoTableComponent, { static: true }) poTable!: PoTableComponent;
  /* ...demais campos*/

  constructor(
    private service: ProdutosService,
    private fb: UntypedFormBuilder
  ) {}

  private _atualizar() {
    const cmd = this.form.getRawValue();
    this.service.atualizar(this._itemSelecionado.id, cmd).subscribe(() => {
      const index = this.items.findIndex(
        (item) => item.id === this._itemSelecionado.id
      );
      const itemAtualizado = { ...cmd, id: this._itemSelecionado.id };
      this.poTable.updateItem(index, itemAtualizado);
      this.items = [...this.poTable.items];
      this.editando = false;
    });
  }

  /* demais implementações*/
}
```

Implemente o método `remover()`. Esse método irá remover o item no back-end e também na tabela:

```ts
/* ...imports necessários*/

@Component({
  templateUrl: "./produtos.component.html",
})
export class ProdutosComponent implements OnInit {
  @ViewChild(PoTableComponent, { static: true }) poTable!: PoTableComponent;
  /* ...demais campos*/

  constructor(
    private service: ProdutosService,
    private fb: UntypedFormBuilder
  ) {}

  remover(item: Produto) {
    this.service.remover(item.id).subscribe(() => {
      const index = this.items.findIndex((el) => el.id === item.id);
      this.poTable.removeItem(index);
      this.items = [...this.poTable.items];
    });
  }

  /* demais implementações*/
}
```

Edite o arquivo `produtos.module.ts` e adicione o `component` nas `declarations`:

```ts
/* ...imports necessários*/

@NgModule({
  imports: [SharedModule],
  declarations: [ProdutosComponent],
  providers: [ProdutosService],
})
export class ProdutosModule {}
```

Agora vamos configurar o roteamento da página `produtos`. Para isso crie o arquivo `produtos-routing.module.ts` dentro do diretório `produtos/`:

```ts
/*...imports omitidos*/

const routes: Routes = [
  // ...rotas omitidas
  {
    path: "produtos",
    title: "Cadastro de Produtos",
    component: ProdutosComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProdutosRoutingModule {}
```

Importe o `routing-module` dentro do `module` de produtos. Edite o arquivo `produtos.module.ts`:

```ts
/* ...imports necessários*/

@NgModule({
  imports: [SharedModule, ProdutoRoutingModule],
  declarations: [ProdutosComponent],
  providers: [ProdutosService],
})
export class ProdutosModule {}
```

Agora edite o arquivo de `routing` princial da aplicação `app-routing.module.ts`:

```ts
// imports omitidos

const routes: Routes = [
  // outras rotas omitidas
  {
    path: "produtos",
    loadChildren: () =>
      import("./produtos/produtos.module").then((m) => m.ProdutosModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
```

Adicione um novo menu dentro do arquivo `app.component.ts`:

```ts
// imports omitidos

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
})
export class AppComponent implements OnInit, OnDestroy {
  menus: PoMenuItem[] = [];

  // propriedades omitidas

  constructor(private router: Router, private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {}

  ngOnDestroy(): void {}

  private _setMenus() {
    this.menus = [
      // menus omitidos
      {
        label: "Produtos",
        link: "/produtos",
        shortLabel: "Produtos",
        icon: "fa fa-box",
      },
    ];
  }

  // demais implementações omitidas
}
```

Para executar a aplicação, certifique-se que você está no diretório: `src/PS.Web/` e rode o comando:

```
dotnet run
```

## Próximos passos:

Que tal alguns desafios e melhorias?

(1) Se você observar nosso código até o momento ele não está validando os inputs dentro do `plsql`. Por exemplo:

```sql
-- trecho extraido de db/src/plsql/app/pkg_app_produtos.pkb.sql

  function criar(
    pi_descricao    tprodutos.descricao%type,
    pi_categoria_id tprodutos.categoria_id%type
  ) return sys_refcursor
  is
    v_id tprodutos.id%type;
  begin
    /* Não estamos validando o pi_descricao e o pi_categoria_id */
    insert into tprodutos
    (id
    ,descricao
    ,categoria_id
    )
    values
    (seq_id_tprodutos.nextval
    ,pi_descricao
    ,pi_categoria_id
    )
    returning id into v_id;

    return obter_por_id(v_id);

  exception
    when dup_val_on_index then
      utl_error.raise('A descrição informada já existe.');
  end criar;

```

O que acha de implementar uma verificação se esses parâmetro não devem ser nulos. Você também poderá validar se a categoria informada existe. Para isso você pode utilizar a função já existente `app_categorias_produtos.existe(pi_categoria_id)`;

Depois de implementar essa regra de negócio, crie um teste para validar se tudo está ok.

(2) Você deve ter observado que passamos no front-end o id da categoria manualmente. Que tal se a gente pudesse ter um combo listando as categorias existente e daí o usuário pudesse escolhe uma dessas categorias. Um exemplo disso podemos encontrar [aqui](https://po-ui.io/documentation/po-combo).

Observe que esse componente pode receber um endpoint onde podemos passar um array, porém você tem que seguir as regras dele. Além disso você precisa criar uma função no plsql que ao passar uma descrição ele filtra os resultados. Então podemos criar uma `app_produtos.listar_por_descricao(pi_descricao)`.

(3) Outra coisa que podemos melhorar em nosso projeto é visualizar na tabela de produtos qual é descrição de categoria em vez de mostrar o id da categoria. Aí você vai precisar usar conhecimentos de `joins` ao realizar uma consulta `sql`. Você poderá criar uma `procedure` com o nome `app_produtos.listar_todos_com_categoria(...)`. Lembre-se também de criar um endpoint específico para esse tipo de consulta.

O objetivo desses desafios é fazer você raciocinar em como você pode implementar essas melhorias, por isso não fique com medo de tentar. :wink:
