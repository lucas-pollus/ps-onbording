cp /tmp/src/schema/tables/tcategorias_produtos.sql .
cp /tmp/src/schema/tables/tcategorias_produtos_pk.sql .
cp /tmp/src/schema/tables/tcategorias_produtos_uk.sql .

sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @tcategorias_produtos.sql;
sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @tcategorias_produtos_pk.sql;
sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @tcategorias_produtos_uk.sql;

rm tcategorias_produtos.sql
rm tcategorias_produtos_pk.sql
rm tcategorias_produtos_uk.sql