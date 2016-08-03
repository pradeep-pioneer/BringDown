/*
 * Interface: IDataDownloader
 * Author: Pradeep Singh
 * Date: 3 July 2016
 * Change Log:
 *      3 July 2016: Created the interface.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BringDownClient.Logic.Core
{
    public interface IDataDownloader
    {

        void QueueDownload(RemoteClient client, DataStore dataStore);
        void Start();

        void Stop();

        bool IsDownloading { get; }

        event EventHandler<ItemDownloadEventArgs> ItemStarted;
        
        event EventHandler<ItemDownloadEventArgs> ItemDownloaded;

        event EventHandler<ItemDownloadEventArgs> ItemFailed;

        event EventHandler<DownloadEventArgs> DownloadCompleted;

        event EventHandler<DownloadEventArgs> DownloadStarted;

    }
}
