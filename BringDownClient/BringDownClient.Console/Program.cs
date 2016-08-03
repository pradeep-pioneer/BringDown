/*
 * Class: Program
 * Author: Pradeep Singh
 * Date: 2 July 2016
 * Change Log:
 *      2 July 2016: Created the class.
 *      3 July 2016: Added the http download feature
 *      4 July 2016: Added the ftp download feature
 *      4 July 2016: Added the file read operation and download feature
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BringDownClient.Logic;
using BringDownClient.Logic.Factory;
using System.IO;
using BringDownClient.Logic.Core;

namespace BringDownClient.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                    DisplayUsageInfo();
                else
                {
                    foreach (var item in args)
                    {
                        ProcessFile(item);
                    }
                    System.Console.WriteLine("\n\nFinished, press any key to exit...");
                    System.Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("\nOOPS! something went wrong\nMessage: {0}\nStackTrace: {1}\n\nPress any key to exit...", ex.Message, ex.StackTrace);
                System.Console.ReadKey();
            }
        }

        private static void ProcessFile(string fileName)
        {
            if (!File.Exists(fileName))
                System.Console.WriteLine("Input file not found");
            else
            {
                System.Console.WriteLine("reading file {0} for download actions...", fileName);
                IDataDownloader dataDownloader = ObjectsFactory.Factory.GetDataDownloader();
                using (StreamReader reader = new StreamReader(File.Open(fileName, FileMode.Open)))
                {
                    while (!reader.EndOfStream)
                    {
                        string entry = reader.ReadLine();
                        if ((!string.IsNullOrEmpty(entry)) && (!string.IsNullOrWhiteSpace(entry)))
                        {
                            string[] parameters = entry.Split('|');
                            if (parameters.Length < 2)
                                System.Console.WriteLine("Not a valid entry: [{0}]", entry);
                            else
                            {
                                RemoteClient client = ObjectsFactory.Factory.GetRemoteClient(parameters[0]);
                                DataStore store = ObjectsFactory.Factory.GetDataStore(new DirectoryInfo(parameters[1]).FullName);
                                dataDownloader.QueueDownload(client, store);
                            }
                        }
                    }
                }
                dataDownloader.DownloadStarted += dataDownloader_DownloadStarted;
                dataDownloader.DownloadCompleted += dataDownloader_DownloadCompleted;
                dataDownloader.ItemStarted += dataDownloader_ItemStarted;
                dataDownloader.ItemFailed += dataDownloader_ItemFailed;
                dataDownloader.ItemDownloaded+=dataDownloader_ItemDownloaded;
                dataDownloader.Start();
                System.Console.WriteLine("File: {0} completed...", fileName);
            }
        }

        private static void dataDownloader_ItemDownloaded(object sender, ItemDownloadEventArgs e)
        {
            System.Console.WriteLine("***File Completed***\nDestination: {0}\nFrom: {1}", e.Destination.Location, e.Source.RawUrl);
        }

        static void dataDownloader_ItemFailed(object sender, ItemDownloadEventArgs e)
        {
            System.Console.WriteLine("***File Failed***\nDestination: {0}\nFrom: {1}", e.Destination.Location, e.Source.RawUrl);
        }

        static void dataDownloader_ItemStarted(object sender, ItemDownloadEventArgs e)
        {
            System.Console.WriteLine("***File Started***\nDestination: {0}\nFrom: {1}", e.Destination.Location, e.Source.RawUrl);
        }

        static void dataDownloader_DownloadCompleted(object sender, DownloadEventArgs e)
        {
            System.Console.WriteLine("\n\n***Download Completed***\nTotal Items: {0}\nCompleted Items: {1}\nFailed Items: {2}", e.TotalItems, e.ItemsCompleted, e.ItemsFailed);
        }

        static void dataDownloader_DownloadStarted(object sender, DownloadEventArgs e)
        {
            System.Console.WriteLine("\n\nDownload Started: Downloading {0} items...", e.TotalItems);
        }

        private static void DisplayUsageInfo()
        {
            System.Console.WriteLine("************************************************");
            System.Console.WriteLine("Bringdown is a small utility for downloading");
            System.Console.WriteLine("Files from remote sources, it currently supports");
            System.Console.WriteLine("Downloading from HTTP and FTP sources");
            System.Console.WriteLine("Parameters: <fileName>\n<fileName>: name of the file");
            System.Console.WriteLine("containing the mappings of remote and local urls");
            System.Console.WriteLine("\n\n\npress any key to exit...");
            System.Console.ReadKey();
        }
    }
}
