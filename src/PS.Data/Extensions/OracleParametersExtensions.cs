using Dapper.Oracle;
using PS.Data.Exceptions;

namespace PS.Data.Extensions;

public static class OracleParametersExtensions
{
    public static void AddArray<T>(this OracleDynamicParameters oraParams, string name, IEnumerable<T> values)
    {
        var type = typeof(T);
        if (type != typeof(int) && type != typeof(string))
        {
            throw new ArgumentException($"Operação é valida somente para tipos int ou string. Tipo informado: {typeof(T)}");
        }

        if (!values.Any())
        {
            throw new DataException("Pelo menos um elemento deve ser passado.");
        }
        var dbType = type == typeof(int) ? OracleMappingType.Int32 : OracleMappingType.Varchar2;
        oraParams.Add(name, values.ToArray(), dbType: dbType, collectionType: OracleMappingCollectionType.PLSQLAssociativeArray);
    }
}