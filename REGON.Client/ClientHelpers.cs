using REGON.Client.Enums;

namespace REGON.Client;

public static class ClientHelpers
{
    public static DateTime? GetDate(string dateTime)
    {
        return !string.IsNullOrEmpty(dateTime) ? Convert.ToDateTime(dateTime) : null;
    }

    public static bool IsHappened(string dateTime)
    {
        return !string.IsNullOrEmpty(dateTime);
    }

    public static string ValueOrNull(string value)
    {
        return !string.IsNullOrEmpty(value) ? value : null;
    }

    public static void ValidateInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new Exception("[REGON] NIP/KRS/REGON is required parameter!");
        }
    }

    public static LegalForm CheckLegalForm(string companyName, string type)
    {
        return type switch
        {
            "P" when IsSpzoo(companyName) &&
                     companyName.ToUpper().Contains("SPÓŁKA KOMANDYTOWA") => LegalForm.SPZOOSK,
            "P" when IsSpzoo(companyName) => LegalForm.SPZOO,
            "P" when companyName.ToUpper().Contains("SPÓŁKA AKCYJNA") => LegalForm.SA,
            "P" when companyName.ToUpper().Contains("SPÓŁKA JAWNA") => LegalForm.SJ,
            "P" when companyName.ToUpper().Contains("SPÓŁKA KOMANDYTOWO - AKCYJNA") => LegalForm.SKA,
            "P" when companyName.ToUpper().Contains("SPÓŁKA KOMANDYTOWA") => LegalForm.SK,
            "F" or "P" when (companyName.ToUpper().Contains("SPÓŁKA CYWILNA") ||
                             companyName.ToUpper().Contains("S.C")) => LegalForm.SC,
            "F" => LegalForm.JDG,
            _ => LegalForm.ERROR
        };
    }

    public static bool IsSpzoo(string companyName)
    {
        return companyName.ToUpper().Contains("SPÓŁKA Z OGRANICZONĄ ODPOWIEDZIALNOŚCIĄ") ||
               companyName.ToUpper().Contains("SPÓŁKA Z OGRANICZONĄ ODPOWIEDZIANOŚCIĄ") ||
               companyName.ToUpper().Contains("SPÓŁKA Z OGRANICZONA ODPOWIEDZIALNOŚCIĄ");
    }

    public static bool IsActive(string companyName)
    {
        var inLiquidation = companyName.ToUpper().Contains("W LIKWIDACJI") ||
                             companyName.ToUpper().Contains("LIKWIDACJI") ||
                             companyName.ToUpper().Contains("LIKWIDACJJI") ||
                             companyName.ToUpper().Contains("LIKWIDACYJNEJ");

        var inBankruptcy = companyName.ToUpper().Contains("W UPADŁOŚCI") ||
                           companyName.ToUpper().Contains("UPADŁOŚCI") ||
                           companyName.ToUpper().Contains("LIKWIDACYJNEJ") ||
                           companyName.ToUpper().Contains("W UPADLOŚCI") ||
                           companyName.ToUpper().Contains("UPADLOŚCI");
        
        return !inLiquidation && !inBankruptcy;
    }
}