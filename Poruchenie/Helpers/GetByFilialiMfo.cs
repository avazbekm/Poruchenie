using System.IO;
using Newtonsoft.Json;
using Poruchenie.Domain.Etities;

namespace Poruchenie.Helpers;

public static class GetByFilialiMfo
{
    private readonly static string Path = "../../../Helpers/bank_filiali.txt";

    public static string GetBankNameByMfo(string mfo)
    {
        string source = File.ReadAllText(Path);
        List<BankFiliali> banks = JsonConvert.DeserializeObject<List<BankFiliali>>(source);

        BankFiliali existBank = banks.FirstOrDefault(a => a.Filialkodi.Equals(mfo));
        if (existBank is not null)
            return existBank.Filialnomi;
        return "";
    }
}
