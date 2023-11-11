using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace CommandBlock
{
    public class Service
    {
        private readonly ILogger<Service> logger;

        public Service(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<Service>();
        }

        public async Task Execute(List<ItemCommand> itemCommands)
        {
            if (itemCommands.Count == 0)
            {
                logger.LogWarning("ItemCommands is Empty");
            }

            foreach (var itemCommand in itemCommands.OrderBy(x => x.OrderId))
            {
                try
                {
                    switch (itemCommand.Type)
                    {
                        case "FileDelete":
                            await FileDeleteExecute(itemCommand);
                            break;
                        case "FileCopy":
                            await FileCopyExecute(itemCommand);
                            break;
                        case "FileMove":
                            await FileMoveExecute(itemCommand);
                            break;
                        case "FileCompress":
                            await FileCompressExecute(itemCommand);
                            break;
                        case "FileUncompress":
                            await FileUncompressExecute(itemCommand);
                            break;
                        case "DirectoryDelete":
                            await DirectoryDeleteExecute(itemCommand);
                            break;
                        case "DirectoryCopy":
                            await DirectoryCopyExecute(itemCommand);
                            break;
                        case "DirectoryMove":
                            await DirectoryMoveExecute(itemCommand);
                            break;
                        case "DirectoryCompress":
                            await DirectoryCompressExecute(itemCommand);
                            break;
                        default:
                            logger.LogWarning("ItemCommand {0}, Type not defined", itemCommand.OrderId);
                            await Task.Delay(1);
                            break;
                    }
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Error ItemCommand {0}", itemCommand.OrderId);
                }
            }
        }

        public async Task FileDeleteExecute(ItemCommand itemCommand)
        {
            var result = false;

            File.Delete($@"{itemCommand.Parameter1}");

            await Task.Delay(itemCommand.Delay);

            logger.LogInformation("ItemCommand {0}, SQL Command run successfully", itemCommand.OrderId);

            logger.LogInformation("ItemCommand {0}, result: {1}", itemCommand.OrderId, result);
        }

        public async Task FileCopyExecute(ItemCommand itemCommand)
        {
            var result = false;

            File.Copy($@"{itemCommand.Parameter1}", $@"{itemCommand.Parameter2}");

            await Task.Delay(itemCommand.Delay);

            logger.LogInformation("ItemCommand {0}, SQL Command run successfully", itemCommand.OrderId);

            logger.LogInformation("ItemCommand {0}, result: {1}", itemCommand.OrderId, result);
        }

        public async Task FileMoveExecute(ItemCommand itemCommand)
        {
            var result = false;

            File.Move($@"{itemCommand.Parameter1}", $@"{itemCommand.Parameter2}");

            await Task.Delay(itemCommand.Delay);

            logger.LogInformation("ItemCommand {0}, SQL Command run successfully", itemCommand.OrderId);

            logger.LogInformation("ItemCommand {0}, result: {1}", itemCommand.OrderId, result);
        }

        public async Task FileCompressExecute(ItemCommand itemCommand)
        {
            var result = false;

            var compressionLevel = CompressionLevel.Fastest;

            switch (itemCommand.Parameter3)
            {
                case "Optimal":
                    compressionLevel = CompressionLevel.Optimal;
                    break;
                case "NoCompression":
                    compressionLevel = CompressionLevel.NoCompression;
                    break;
                case "SmallestSize":
                    compressionLevel = CompressionLevel.SmallestSize;
                    break;
                default:
                    break;
            }

            using (var archive = ZipFile.Open(itemCommand.Parameter2, ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(itemCommand.Parameter1, Path.GetFileName(itemCommand.Parameter1), compressionLevel);
            }

            await Task.Delay(itemCommand.Delay);

            logger.LogInformation("ItemCommand {0}, SQL Command run successfully", itemCommand.OrderId);

            logger.LogInformation("ItemCommand {0}, result: {1}", itemCommand.OrderId, result);
        }

        public async Task FileUncompressExecute(ItemCommand itemCommand)
        {
            var result = false;

            ZipFile.ExtractToDirectory(itemCommand.Parameter1, itemCommand.Parameter2, true);

            await Task.Delay(itemCommand.Delay);

            logger.LogInformation("ItemCommand {0}, SQL Command run successfully", itemCommand.OrderId);

            logger.LogInformation("ItemCommand {0}, result: {1}", itemCommand.OrderId, result);
        }

        public async Task DirectoryDeleteExecute(ItemCommand itemCommand)
        {
            var result = false;

            UtilDirectory.Delete(itemCommand.Parameter1);

            await Task.Delay(itemCommand.Delay);

            logger.LogInformation("ItemCommand {0}, SQL Command run successfully", itemCommand.OrderId);

            logger.LogInformation("ItemCommand {0}, result: {1}", itemCommand.OrderId, result);
        }

        public async Task DirectoryCopyExecute(ItemCommand itemCommand)
        {
            var result = false;

            UtilDirectory.Copy(itemCommand.Parameter1, itemCommand.Parameter2, true);

            await Task.Delay(itemCommand.Delay);

            logger.LogInformation("ItemCommand {0}, SQL Command run successfully", itemCommand.OrderId);

            logger.LogInformation("ItemCommand {0}, result: {1}", itemCommand.OrderId, result);
        }
        public async Task DirectoryMoveExecute(ItemCommand itemCommand)
        {
            var result = false;

            UtilDirectory.Copy(itemCommand.Parameter1, itemCommand.Parameter2, true);
            UtilDirectory.Delete(itemCommand.Parameter1);

            await Task.Delay(itemCommand.Delay);

            logger.LogInformation("ItemCommand {0}, SQL Command run successfully", itemCommand.OrderId);

            logger.LogInformation("ItemCommand {0}, result: {1}", itemCommand.OrderId, result);
        }
        public async Task DirectoryCompressExecute(ItemCommand itemCommand)
        {
            var result = false;

            var compressionLevel = CompressionLevel.Fastest;

            switch (itemCommand.Parameter3)
            {
                case "Optimal":
                    compressionLevel = CompressionLevel.Optimal;
                    break;
                case "NoCompression":
                    compressionLevel = CompressionLevel.NoCompression;
                    break;
                case "SmallestSize":
                    compressionLevel = CompressionLevel.SmallestSize;
                    break;
                default:
                    break;
            }

            ZipFile.CreateFromDirectory(itemCommand.Parameter1, itemCommand.Parameter2, compressionLevel, false);

            await Task.Delay(itemCommand.Delay);

            logger.LogInformation("ItemCommand {0}, SQL Command run successfully", itemCommand.OrderId);

            logger.LogInformation("ItemCommand {0}, result: {1}", itemCommand.OrderId, result);
        }
    }
}
