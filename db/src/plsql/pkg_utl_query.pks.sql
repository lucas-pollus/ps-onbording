create or replace package utl_query is

    subtype t_page        is number(5);
    subtype t_page_size   is number(5);
    subtype t_total_pages is number(5);
    subtype t_total_rows  is number(5);
    subtype t_sql_query   is varchar2(32767);
    
    type info_rt is record(
        page          number(5),
        page_size     number(5),
        total_rows    number(5),
        total_pages   number(5),
        has_next_page number(1)
    );

    type info_list_t is table of info_rt; 
        
    k_min_page          t_page := 1;
    k_page_size_default t_page_size := 10;
    
    function format_query(
        pi_sql_query t_sql_query,
        pi_param_1 varchar2 default null,
        pi_param_2 varchar2 default null,
        pi_param_3 varchar2 default null,
        pi_param_4 varchar2 default null,
        pi_param_5 varchar2 default null,
        pi_param_6 varchar2 default null,
        pi_param_7 varchar2 default null,
        pi_param_8 varchar2 default null,
        pi_param_9 varchar2 default null,
        pi_param_10 varchar2 default null,
        pi_param_11 varchar2 default null,
        pi_param_12 varchar2 default null,
        pi_param_13 varchar2 default null,
        pi_param_14 varchar2 default null,
        pi_param_15 varchar2 default null,
        pi_param_16 varchar2 default null,
        pi_param_17 varchar2 default null,
        pi_param_18 varchar2 default null,
        pi_param_19 varchar2 default null,
        pi_param_20 varchar2 default null
    ) return t_sql_query;
    
    function get_list_info(pi_info_query info_rt)
        return info_list_t pipelined;
    
    procedure execute_query(
        pi_sql_query  t_sql_query,
        pi_page       t_page default k_min_page,
        pi_page_size  t_page_size default k_page_size_default,
        po_result     out sys_refcursor,
        po_info_query out sys_refcursor
    );
end;
/
