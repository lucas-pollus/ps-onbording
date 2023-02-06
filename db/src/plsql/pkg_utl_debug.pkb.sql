create or replace package body utl_debug
is
  m_debugging boolean := false;
  
  procedure debug_off
  is
  begin
    m_debugging := false;
  end;
  
  procedure debug_on
  is
  begin
    m_debugging := true;
  end;
  
  procedure print(pi_msg varchar2)
  is
    l_text varchar2(32000);
  begin

    if (m_debugging) then
      l_text:=to_char(sysdate, 'dd.mm.yyyy hh24:mi:ss') || ': ' || nvl(pi_msg, '(null)');
    
      loop
        exit when l_text is null;
        dbms_output.put_line(substr(l_text,1,250));
        l_text:=substr(l_text, 251);
      end loop;
    end if;

  end print;
  
  procedure print(pi_msg   varchar2,
                  pi_value varchar2)
  is
  begin

    print(pi_msg || ': ' || pi_value);
  
  end print;
  
  procedure print (pi_msg varchar2,
                   pi_value number)
  is
  begin

    print(pi_msg || ': ' || nvl(to_char(pi_value), '(null)'));
  
  end print;
  
  procedure print(pi_msg   varchar2,
                  pi_value date)
  is
  begin

    print(pi_msg || ': ' || nvl(to_char(pi_value, 'dd.mm.yyyy hh24:mi:ss'), '(null)'));
  
  end print;
  
  procedure print(pi_msg   varchar2,
                  pi_value boolean)
  as
    l_str varchar2(20);
  begin

    if pi_value is null then
      l_str := '(null)';
    elsif pi_value = true then
      l_str := 'true';
    else
      l_str := 'false';
    end if;

    print (pi_msg || ': ' || l_str);
  
  end print;
  
  procedure print(pi_refcursor     sys_refcursor,
                  pi_null_handling number := 0)
  is
    l_xml      xmltype;
    l_context  dbms_xmlgen.ctxhandle;
    l_clob     clob;

    l_null_self_argument_exc exception;
    pragma exception_init (l_null_self_argument_exc, -30625);
  begin
    l_context:=dbms_xmlgen.newcontext(pi_refcursor);
    
    /*
      # DROP_NULLS CONSTANT NUMBER:= 0; (Default) Leaves out the tag for NULL elements.
      # NULL_ATTR CONSTANT NUMBER:= 1; Sets xsi:nil="true".
      # EMPTY_TAG CONSTANT NUMBER:= 2; Sets, for example, <foo/>.
    */
    
    dbms_xmlgen.setnullhandling(l_context, pi_null_handling);
    l_xml:=dbms_xmlgen.getxmltype(l_context, dbms_xmlgen.none);
    print('Number of rows in ref cursor', dbms_xmlgen.getnumrowsprocessed(l_context));
    
    begin
      l_clob:=l_xml.getclobval();
      if length(l_clob) > 32000 then
        print('Size of XML document (anything over 32K will be truncated)', length(l_clob));
      end if;
      print(pi_msg => substr(l_clob,1,32000));
    exception
      when l_null_self_argument_exc then
        print('Empty dataset.');
    end;
    
  end print;
  
  procedure printf(pi_msg in varchar2,
                   pi_value1 in varchar2 := null,
                   pi_value2 in varchar2 := null,
                   pi_value3 in varchar2 := null,
                   pi_value4 in varchar2 := null,
                   pi_value5 in varchar2 := null,
                   pi_value6 in varchar2 := null,
                   pi_value7 in varchar2 := null,
                   pi_value8 in varchar2 := null)
  is
    l_text varchar2(32000);
  begin
    if m_debugging then
  
      l_text:=pi_msg;
    
      l_text:=replace(l_text, '{1}', nvl(pi_value1,'(blank)'));
      l_text:=replace(l_text, '{2}', nvl(pi_value2,'(blank)'));
      l_text:=replace(l_text, '{3}', nvl(pi_value3,'(blank)'));
      l_text:=replace(l_text, '{4}', nvl(pi_value4,'(blank)'));
      l_text:=replace(l_text, '{5}', nvl(pi_value5,'(blank)'));
      l_text:=replace(l_text, '{6}', nvl(pi_value6,'(blank)'));
      l_text:=replace(l_text, '{7}', nvl(pi_value7,'(blank)'));
      l_text:=replace(l_text, '{8}', nvl(pi_value8,'(blank)'));

      print (l_text);
  
    end if;
    
  end printf;
  
end utl_debug;
/