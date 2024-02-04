using System.IO;
using Newtonsoft.Json;
using Poruchenie.Domain.Etities;
using Poruchenie.Service.Exceptions;
using Poruchenie.Service.Interfaces;

namespace Poruchenie.Service.Services;

public class YurdikService : IYurdikService
{

    private readonly string Path = Data.Contants.Constant.YURDIK_PATH;

    public YurdikService()
    {
        string source = File.ReadAllText(Path);
        if (string.IsNullOrEmpty(source))
            File.WriteAllText(Path, "[]");
    }
    
    public async Task<Response<Yurdik>> CreateAsync(Yurdik yurdik)
    {
        string source = File.ReadAllText(Path);
        List<Yurdik> yurdiks = JsonConvert.DeserializeObject<List<Yurdik>>(source);

        Yurdik existYurdik = yurdiks.FirstOrDefault(a => a.CountNumber.Equals(yurdik.CountNumber));
        if (existYurdik is not null)
            return new Response<Yurdik>
            {
                StatusCode = 200,
                Message = "Ok",
                Data = yurdik
            };

        Yurdik lastYurdik = yurdiks.LastOrDefault();
        if (lastYurdik is null)
            yurdik.Id = 1;
        else yurdik.Id = 1 + lastYurdik.Id;

        yurdiks.Add(yurdik);
        source = JsonConvert.SerializeObject(yurdiks, Formatting.Indented);
        File.WriteAllText(Path, source);

        return new Response<Yurdik>
        {
            StatusCode = 200,
            Message = "Ok",
            Data = yurdik
        };
    }

    public async Task<Response<Yurdik>> GetByCountNumberAsync(string count)
    {
        string source = File.ReadAllText(Path);
        List<Yurdik> yurdiks = JsonConvert.DeserializeObject<List<Yurdik>>(source);

        Yurdik existYurdik = yurdiks.FirstOrDefault(a => a.CountNumber.Equals(count));
        return new Response<Yurdik>
        {
            StatusCode = 200,
            Message = "Ok",
            Data = existYurdik
        };
    }
}
