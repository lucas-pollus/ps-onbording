cp /tmp/src/plsql/pkg_utl_query.pks.sql .
cp /tmp/src/plsql/pkg_utl_query.pkb.sql .
sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @pkg_utl_query.pks.sql;
sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @pkg_utl_query.pkb.sql;
rm pkg_utl_query.pks.sql
rm pkg_utl_query.pkb.sql