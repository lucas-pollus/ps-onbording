declare
  v_count pls_integer;
  v_cmd   varchar2(4000);
begin
  select count(*)
  into   v_count
  from   user_tables u
  where  u.table_name = upper('tcategorias_produtos');
  
  if v_count = 0 then
    utl_error.raise('Falha ao executa script: não existe tabela.');
  end if;
  
  select count(*)
  into   v_count
  from   user_constraints u
  where  u.constraint_name = upper('tcategorias_produtos_uk');
  
  if v_count = 0 then
    v_cmd := '
      alter table tcategorias_produtos 
      add constraint tcategorias_produtos_uk unique (descricao)
    ';
    
    execute immediate v_cmd;
  end if;
end;
/
