namespace CommandBlock
{
    public class UtilDirectory
    {
        public static void Delete(string directory)
        {
            try
            {
                Directory.Delete(directory, true);
            }
            catch (Exception)
            {
            }
        }

        public static void Copy(string sourceDirectory, string destinationDirectory, bool recursive)
        {
            var directoryInfo = new DirectoryInfo(sourceDirectory);

            if (!directoryInfo.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory not found: {directoryInfo.FullName}");
            }

            var directories = directoryInfo.GetDirectories();

            Directory.CreateDirectory(destinationDirectory);

            foreach (var file in directoryInfo.GetFiles())
            {
                var targetFilePath = Path.Combine(destinationDirectory, file.Name);
                file.CopyTo(targetFilePath);
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDirectory in directories)
                {
                    string newDestinationDirectory = Path.Combine(destinationDirectory, subDirectory.Name);
                    Copy(subDirectory.FullName, newDestinationDirectory, true);
                }
            }
        }
    }
}
