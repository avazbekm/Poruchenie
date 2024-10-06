using System.IO;
using Newtonsoft.Json;
using Poruchenie.Domain.Etities;
using Poruchenie.Service.Exceptions;
using Poruchenie.Service.Interfaces;

namespace Poruchenie.Service.Services;

public class JismoniyService : IJismoniyService
{

    private readonly string Path = Data.Contants.Constant.JISMONIY_PATH;

    public JismoniyService()
    {
        try
        {
            string sources = File.ReadAllText(Path);
        }
        catch (FileNotFoundException)
        {
            // Faylni yaratish
            using (StreamWriter writer = File.CreateText(Path))
            { }
        }
        string source = File.ReadAllText(Path);
        if (string.IsNullOrEmpty(source))
            File.WriteAllText(Path, "[]");
    }

    public async Task<Response<Jismoniy>> CreateAsync(Jismoniy jismoniy)
    {
        string source = File.ReadAllText(Path);
        List<Jismoniy> jismoniys = JsonConvert.DeserializeObject<List<Jismoniy>>(source);

        Jismoniy existJismoniy = jismoniys.FirstOrDefault(a => a.CountNumber.Equals(jismoniy.CountNumber));
        if (existJismoniy is not null)
        {
            existJismoniy.Name = jismoniy.Name;
            existJismoniy.CountNumber = jismoniy.CountNumber;
            existJismoniy.Bank = jismoniy.Bank;
            existJismoniy.MFO = jismoniy.MFO;
            existJismoniy.PINFL = jismoniy.PINFL;
            existJismoniy.Rahbar = jismoniy.Rahbar;

            source = JsonConvert.SerializeObject(jismoniys, Formatting.Indented);
            File.WriteAllText(Path, source);

            return new Response<Jismoniy>
            {
                StatusCode = 403,
                Message = "This jismoniy is already exist",
                Data = jismoniy
            };
        }

        Jismoniy lastJismoniy = jismoniys.LastOrDefault();
        if (lastJismoniy is null)
            jismoniy.Id = 1;
        else jismoniy.Id = 1 + lastJismoniy.Id;

        jismoniys.Add(jismoniy);
        source = JsonConvert.SerializeObject(jismoniys, Formatting.Indented);
        File.WriteAllText(Path, source);

        return new Response<Jismoniy>
        {
            StatusCode = 200,
            Message = "Ok",
            Data = jismoniy
        };
    }

    public async Task<Response<Jismoniy>> GetByCountNumberAsync(string count)
    {
        string source = File.ReadAllText(Path);
        List<Jismoniy> jismoniys = JsonConvert.DeserializeObject<List<Jismoniy>>(source);

        Jismoniy existJismoniy = jismoniys.FirstOrDefault(a => a.CountNumber.Equals(count));
        return new Response<Jismoniy>
        {
            StatusCode = 200,
            Message = "Ok",
            Data = existJismoniy
        };
    }
}
