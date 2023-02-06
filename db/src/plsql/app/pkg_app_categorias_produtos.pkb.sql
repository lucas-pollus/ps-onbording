create or replace package body app_categorias_produtos
is
  function existe(
    pi_id tcategorias_produtos.id%type
  ) return boolean
  is
    v_existe pls_integer;
  begin
    select count(*)
    into   v_existe
    from   tcategorias_produtos
    where  id = pi_id;
    
    if v_existe > 0 then
      return true;
    end if;
    
    return false;
  end existe;
  
  function obter_por_id(
    pi_id tcategorias_produtos.id%type
  ) return sys_refcursor
  is
    v_result sys_refcursor;
  begin
    open v_result
    for select id
             , descricao
        from   tcategorias_produtos
        where  id = pi_id;
    
    return v_result;    
  end obter_por_id;
  
  function criar(
    pi_descricao tcategorias_produtos.descricao%type
  ) return sys_refcursor
  is
    v_id tcategorias_produtos.id%type;
  begin
    insert into tcategorias_produtos
    (id
    ,descricao
    )
    values
    (seq_id_tcategorias_produtos.nextval
    ,pi_descricao
    )
    returning id into v_id;
    
    return obter_por_id(v_id);
    
  exception
    when dup_val_on_index then
      utl_error.raise('A descrição informada já existe.');
  end criar;
  
  procedure atualizar(
    pi_id        tcategorias_produtos.id%type,
    pi_descricao tcategorias_produtos.descricao%type
  )
  is
  begin
    update tcategorias_produtos c
    set    c.descricao = pi_descricao
         , c.dt_modificacao = sysdate
    where  c.id = pi_id;
    
  exception
    when dup_val_on_index then
      utl_error.raise('A descrição informada já existe.');
  end atualizar;
  
  procedure remover(pi_id tcategorias_produtos.id%type)
  is
  begin
    delete from tcategorias_produtos where id = pi_id;
  end remover;
  
  procedure listar_todas(
    pi_page       utl_query.t_page,
    pi_page_size  utl_query.t_page_size,
    po_result     out sys_refcursor,
    po_info_query out sys_refcursor
  )
  is
    v_sql utl_query.t_sql_query;
  begin
    v_sql := 'select id, descricao from tcategorias_produtos';
    
    utl_query.execute_query(
      v_sql,
      pi_page,
      pi_page_size,
      po_result,
      po_info_query
    );
    
  end listar_todas;
  
end app_categorias_produtos;
/
