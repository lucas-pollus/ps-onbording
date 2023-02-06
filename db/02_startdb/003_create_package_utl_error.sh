cp /tmp/src/plsql/pkg_utl_error.pks.sql .
cp /tmp/src/plsql/pkg_utl_error.pkb.sql .
sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @pkg_utl_error.pks.sql;
sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @pkg_utl_error.pkb.sql;
rm pkg_utl_error.pks.sql
rm pkg_utl_error.pkb.sql