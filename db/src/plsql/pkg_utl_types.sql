create or replace package utl_types
is
    type t_array_numbers  is table of number index by pls_integer;
    type t_array_varchars is table of number index by pls_integer;
end;
/