create or replace package utl_error
is
    k_application_error constant number := -20999;
    e_application_error exception;
    pragma exception_init(e_application_error, -20999);    
    subtype t_msg is varchar2(1000);

    procedure raise(pi_msg t_msg);
end;
/