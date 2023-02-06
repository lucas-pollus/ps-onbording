create or replace package body utl_query
is
    subtype t_offset      is pls_integer;

    function format_query( 
        pi_sql_query t_sql_query,
        pi_param_1    varchar2 default null, 
        pi_param_2    varchar2 default null,
        pi_param_3    varchar2 default null,
        pi_param_4    varchar2 default null,
        pi_param_5    varchar2 default null,
        pi_param_6    varchar2 default null,
        pi_param_7    varchar2 default null,
        pi_param_8    varchar2 default null,
        pi_param_9    varchar2 default null,
        pi_param_10   varchar2 default null,
        pi_param_11   varchar2 default null,
        pi_param_12   varchar2 default null,
        pi_param_13   varchar2 default null,
        pi_param_14   varchar2 default null,
        pi_param_15   varchar2 default null,
        pi_param_16   varchar2 default null,
        pi_param_17   varchar2 default null,
        pi_param_18   varchar2 default null,
        pi_param_19   varchar2 default null,
        pi_param_20   varchar2 default null
    ) return t_sql_query
    is
        v_result t_sql_query := pi_sql_query;
    begin
        v_result := replace(v_result,'{1}',pi_param_1);
        v_result := replace(v_result,'{2}',pi_param_2);
        v_result := replace(v_result,'{3}',pi_param_3);
        v_result := replace(v_result,'{4}',pi_param_4);
        v_result := replace(v_result,'{5}',pi_param_5);
        v_result := replace(v_result,'{6}',pi_param_6);
        v_result := replace(v_result,'{7}',pi_param_7);
        v_result := replace(v_result,'{8}',pi_param_8);
        v_result := replace(v_result,'{9}',pi_param_9);
        v_result := replace(v_result,'{10}',pi_param_10);
        v_result := replace(v_result,'{11}',pi_param_11);
        v_result := replace(v_result,'{12}',pi_param_12);
        v_result := replace(v_result,'{13}',pi_param_13);
        v_result := replace(v_result,'{14}',pi_param_14);
        v_result := replace(v_result,'{15}',pi_param_15);
        v_result := replace(v_result,'{16}',pi_param_16);
        v_result := replace(v_result,'{17}',pi_param_17);
        v_result := replace(v_result,'{18}',pi_param_18);
        v_result := replace(v_result,'{19}',pi_param_19);
        v_result := replace(v_result,'{20}',pi_param_20);
        return v_result;
    end format_query;

    function get_offset(
        pi_page       t_page,
        pi_page_size  t_page_size
    ) return t_offset
    is
        v_result t_offset;
    begin
        v_result := (pi_page - 1) * pi_page_size;
        return v_result;
    end get_offset;
  
    function get_total_rows(pi_sql_query t_sql_query)
    return t_total_rows
    is
        v_sql_query t_sql_query;
        v_result    t_total_rows;  
    begin
        v_sql_query := format_query(
            'select count(*) over () from ({1}) fetch first rows only',
            pi_sql_query
        );

        execute immediate v_sql_query into v_result;
        return v_result;
    exception
        when no_data_found then
            return 0;  
    end get_total_rows;
  
    function get_total_pages(
        pi_total_rows t_total_rows,
        pi_page_size  t_page_size
    ) return t_total_pages
    is
    begin
        return ceil(pi_total_rows / pi_page_size);
    end get_total_pages;  
  
    function has_next_page(
        pi_page        t_page,
        pi_total_pages t_total_pages
    ) return pls_integer
    is
    begin
        if pi_page < pi_total_pages then
            return 1;
        end if;
        return 0;
    end has_next_page;
  
    function get_result(
        pi_sql_query t_sql_query,
        pi_offset    t_offset,
        pi_page_size t_page_size
    ) return sys_refcursor
    is
        v_sql_query t_sql_query;
        v_result    sys_refcursor;
    begin
        v_sql_query := pi_sql_query ||' offset :v_offset rows fetch next :v_page_size rows only';
        open  v_result 
        for   v_sql_query
        using pi_offset,
              pi_page_size;
    
        return v_result;     
    end get_result;

    function get_list_info(pi_info_query info_rt)
        return info_list_t pipelined
    is
    begin
        pipe row(pi_info_query);
        return;
    end get_list_info;
  
    
    function convert_to_refcursor(pi_info_query info_rt)
    return sys_refcursor
    is
        v_result sys_refcursor;
        v_info_rt info_rt := pi_info_query;
    begin
        open v_result
        for select *
            from   table(get_list_info(v_info_rt));
        
        return v_result;
    end convert_to_refcursor;
  
    procedure execute_query(
        pi_sql_query  t_sql_query,
        pi_page       t_page default k_min_page,
        pi_page_size  t_page_size default k_page_size_default,
        po_result     out sys_refcursor,
        po_info_query out sys_refcursor)
    is
        v_offset     t_offset;
        v_info_query info_rt;
    begin
        v_offset                   := get_offset(pi_page,pi_page_size);
        v_info_query.total_rows    := get_total_rows(pi_sql_query);
        v_info_query.total_pages   := get_total_pages(v_info_query.total_rows,pi_page_size);
        v_info_query.has_next_page := has_next_page(pi_page, v_info_query.total_pages);
        v_info_query.page          := pi_page;
        v_info_query.page_size     := pi_page_size;
        po_info_query              := convert_to_refcursor(v_info_query);
        po_result                  := get_result(pi_sql_query,v_offset,pi_page_size);
    end execute_query;
end;
/
