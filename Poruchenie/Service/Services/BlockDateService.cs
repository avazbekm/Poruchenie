using System.IO;
using Newtonsoft.Json;
using Poruchenie.Domain.Etities;
using Poruchenie.Service.Exceptions;
using Poruchenie.Service.Interfaces;

namespace Poruchenie.Service.Services;

public class BlockDateService : IBlockDateService
{

    private readonly string Path = Data.Contants.Constant.BLOCKDATE_PATH;

    public BlockDateService()
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

    public async Task<Response<BlockedDate>> CreateAsync()
    {
        BlockedDate blockedDate = new BlockedDate();

        string endDate = DateTime.UtcNow.ToString().Substring(0,10);
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(endDate);

        blockedDate.Id = 1;
        blockedDate.BlockDate = $"{hashedPassword}{endDate}";
        blockedDate.CreatedAt= DateTime.UtcNow;

        string source = JsonConvert.SerializeObject(blockedDate, Formatting.Indented);
        File.WriteAllText(Path, source);

        return new Response<BlockedDate>
        {
            StatusCode = 200,
            Message = "Ok",
            Data = blockedDate
        };
    }

    public async Task<Response<BlockedDate>> GetAsync()
    {
        string source = File.ReadAllText(Path);
        BlockedDate blockedDate;
        if (source.Trim().StartsWith("["))
        {
            // It's a JSON array
            List<BlockedDate> blockedDates = JsonConvert.DeserializeObject<List<BlockedDate>>(source);
            blockedDate = blockedDates.FirstOrDefault(a=>a.Id==1);
        }
        else
        {
            // It's a JSON object
            blockedDate = JsonConvert.DeserializeObject<BlockedDate>(source);
        }

        return new Response<BlockedDate>
        {
            StatusCode = 200,
            Message = "Ok",
            Data = blockedDate
        };
    }

    public async Task<Response<BlockedDate>> UpdateAsync()
    {
        BlockedDate blockedDate = new BlockedDate();

        string endDate = DateTime.UtcNow.AddYears(2).ToString().Substring(0, 10);
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(endDate);

        blockedDate.Id = 1;
        blockedDate.BlockDate = $"{hashedPassword}{endDate}";
        blockedDate.CreatedAt = DateTime.UtcNow;

        string source = JsonConvert.SerializeObject(blockedDate, Formatting.Indented);
        File.WriteAllText(Path, source);

        return new Response<BlockedDate>
        {
            StatusCode = 200,
            Message = "Ok",
            Data = blockedDate
        };
    }
}