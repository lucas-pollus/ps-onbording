create or replace package utl_debug
is
  procedure debug_off;
  
  procedure debug_on;
  
  procedure print(pi_msg varchar2);
  
  procedure print(pi_msg   varchar2,
                  pi_value varchar2);
  
  procedure print(pi_msg   varchar2,
                  pi_value number);
                 
  procedure print(pi_msg   varchar2,
                  pi_value date);
                 
  procedure print(pi_msg   varchar2,
                  pi_value boolean);
                  
  procedure print(pi_refcursor     sys_refcursor,
                  pi_null_handling number := 0);
                  
  procedure printf(pi_msg in varchar2,
                   pi_value1 in varchar2 := null,
                   pi_value2 in varchar2 := null,
                   pi_value3 in varchar2 := null,
                   pi_value4 in varchar2 := null,
                   pi_value5 in varchar2 := null,
                   pi_value6 in varchar2 := null,
                   pi_value7 in varchar2 := null,
                   pi_value8 in varchar2 := null);                
end utl_debug;
/