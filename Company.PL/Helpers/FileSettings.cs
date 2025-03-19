namespace Company.PL.Helpers
{
    public static class FileSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Folder Path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName);

            // 2. Get File Name, And make it Unique
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Get File Path
            string filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName, fileName);
            
            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
