create or replace package app_categorias_produtos
is
  function existe(
    pi_id tcategorias_produtos.id%type
  ) return boolean;
  
  function obter_por_id(
    pi_id tcategorias_produtos.id%type
  ) return sys_refcursor;
  
  function criar(
    pi_descricao tcategorias_produtos.descricao%type
  ) return sys_refcursor;
  
  procedure atualizar(
    pi_id        tcategorias_produtos.id%type,
    pi_descricao tcategorias_produtos.descricao%type
  );
  
  procedure remover(pi_id tcategorias_produtos.id%type);
  
  procedure listar_todas(
    pi_page       utl_query.t_page,
    pi_page_size  utl_query.t_page_size,
    po_result     out sys_refcursor,
    po_info_query out sys_refcursor
  );
  
end app_categorias_produtos;
/
