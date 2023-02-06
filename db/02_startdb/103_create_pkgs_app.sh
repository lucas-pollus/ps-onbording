cp /tmp/src/plsql/app/pkg_app_categorias_produtos.pks.sql .
cp /tmp/src/plsql/app/pkg_app_categorias_produtos.pkb.sql .

sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @pkg_app_categorias_produtos.pks.sql;
sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @pkg_app_categorias_produtos.pkb.sql;

rm pkg_app_categorias_produtos.pks.sql
rm pkg_app_categorias_produtos.pkb.sql