using System.Text.RegularExpressions;

namespace PS.Data.Extensions;

public static class StringExtensions
{
    public const char SEPARADOR_DEFAULT = (char)172;
    public static string ExtraiMensagemAscii(this string msg)
    {
        try
        {
            var inicio = msg.IndexOf(SEPARADOR_DEFAULT) + 1;
            var fim = msg.IndexOf(SEPARADOR_DEFAULT, inicio);
            return msg[inicio..fim];
        }
        catch (ArgumentOutOfRangeException)
        {
            return msg;
        }

    }

    public static string ExtraiMensagem(this string msg)
    {
        var pattern = @"(^ORA-20999:)\s(!###)([\w\s\.]*)(@@@!)";
        var msgs = Regex.Split(msg, pattern);
        if (msgs.Length > 0)
        {
            return msgs[3];
        }
        return msg;

    }
}