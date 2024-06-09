namespace Event_QrCode_Scanner.Helper
{
    public static class FileHelper 
    {
        // FileHelper perdoret per leximit/ndryshimit dhe fshirjes e file ne kompjuter 
        public static bool FileExists(string fullPath)
        {
            return File.Exists(fullPath);
        }
         
        public static void DeleteFile(string fullPath)
        {
            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (IOException ex)
            {
                // shfaq mesazhin kur file eshte ne perdorim ose përjashtime të tjera
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
        }
    }
}
