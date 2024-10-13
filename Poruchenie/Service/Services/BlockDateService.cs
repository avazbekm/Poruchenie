using System.IO;
using Newtonsoft.Json;
using System.Security.AccessControl;
using Poruchenie.Service.Exceptions;
using Poruchenie.Service.Interfaces;
using Poruchenie.Domain.Etities;
using System.Windows;

namespace Poruchenie.Service.Services
{
    public class BlockDateService : IBlockDateService
    {
        private readonly string Path = Data.Contants.Constant.BLOCKDATE_PATH;

        public BlockDateService()
        {
            try
            {
                // Try to read the file content
                string sources = File.ReadAllText(Path);
            }
            catch (FileNotFoundException)
            {
                // File not found, create the file and set permissions
                CreateFileWithPermissions(Path);
            }
        }

        private void CreateFileWithPermissions(string filePath)
        {
            try
            {
                // Create an empty file using FileInfo
                FileInfo fileInfo = new FileInfo(filePath);
                using (StreamWriter writer = fileInfo.CreateText())
                {
                    writer.WriteLine("[]"); // Writing some empty JSON object to avoid empty file issues
                }

                // Create a new FileSecurity object to set permissions
                FileSecurity fileSecurity = new FileSecurity();

                // Define a rule for the current user
                string currentUser = Environment.UserName;

                // Add a rule to give FullControl to the current user
                fileSecurity.AddAccessRule(new FileSystemAccessRule(currentUser,
                    FileSystemRights.FullControl,
                    AccessControlType.Allow));

                // Apply the access control settings to the file using FileInfo
                fileInfo.SetAccessControl(fileSecurity);

                MessageBox.Show($"File created successfully at {filePath} with permissions.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        public async Task<Response<BlockedDate>> CreateAsync()
        {
            BlockedDate blockedDate = new BlockedDate();

            string endDate = DateTime.UtcNow.AddDays(10).ToString("yyyy-MM-dd");
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

        public async Task<Response<BlockedDate>> GetAsync()
        {
            string source = File.ReadAllText(Path);
            BlockedDate blockedDate;

            if (source.Trim().StartsWith("["))
            {
                // JSON array
                List<BlockedDate> blockedDates = JsonConvert.DeserializeObject<List<BlockedDate>>(source);
                blockedDate = blockedDates.FirstOrDefault(a => a.Id == 1);
            }
            else
            {
                // JSON object
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

            string endDate = DateTime.UtcNow.AddYears(2).ToString("yyyy-MM-dd");
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
}
