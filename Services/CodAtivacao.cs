namespace apiBlog.Services;

public static class CodAtivacao{
    public static string GerarCodigo(this string codigo){
    
    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    var random = new Random();
    var result = new string(
        Enumerable.Repeat(chars, 8)
                  .Select(s => s[random.Next(s.Length)])
                  .ToArray());

    return result;
    }
}