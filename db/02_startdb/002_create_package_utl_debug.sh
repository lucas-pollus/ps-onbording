cp /tmp/src/plsql/pkg_utl_debug.pks.sql .
cp /tmp/src/plsql/pkg_utl_debug.pkb.sql .
sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @pkg_utl_debug.pks.sql;
sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @pkg_utl_debug.pkb.sql;
rm pkg_utl_debug.pks.sql
rm pkg_utl_debug.pkb.sql