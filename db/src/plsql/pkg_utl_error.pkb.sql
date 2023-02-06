create or replace package body utl_error
is
    procedure raise(pi_msg t_msg)
    is
        v_msg t_msg;
    begin
        v_msg := '!###'||pi_msg||'@@@!';
        raise_application_error(k_application_error,v_msg,true);
    end raise;
end;
/
