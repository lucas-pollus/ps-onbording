cp /tmp/src/plsql/pkg_utl_types.sql .
sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @pkg_utl_types.sql;
rm pkg_utl_types.sql