cp /tmp/src/schema/sequences/seq_id_tcategorias_produtos.sql .

sqlplus -s HOPPER/FlowMatic@//localhost/XEPDB1 @seq_id_tcategorias_produtos.sql;

rm seq_id_tcategorias_produtos.sql