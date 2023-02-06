declare
  v_count pls_integer;
  v_cmd   varchar2(4000);
begin
  select count(*)
  into   v_count
  from   user_sequences u
  where  u.sequence_name = upper('seq_id_tcategorias_produtos');
  
  if v_count = 0 then
    v_cmd := '
      create sequence seq_id_tcategorias_produtos
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
