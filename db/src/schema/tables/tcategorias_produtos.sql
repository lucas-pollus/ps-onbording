declare
  v_count pls_integer;
  v_cmd   varchar2(4000);
begin
  select count(*)
  into   v_count
  from   user_tables u
  where  u.table_name = upper('tcategorias_produtos');
  
  if v_count = 0 then
    v_cmd := 'create table tcategorias_produtos(
      id number(10) not null,
      descricao varchar2(100) not null,
      dt_criacao date default sysdate,
      dt_modificacao date default sysdate
    )';
    
    execute immediate v_cmd;
    
  end if;
end;
/
