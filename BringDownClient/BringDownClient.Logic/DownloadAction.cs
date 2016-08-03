/*
 * Class: DownloadAction
 * Author: Pradeep Singh
 * Date: 3 July 2016
 * Change Log:
 *      3 July 2016: Created the class.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BringDownClient.Logic.Core;

namespace BringDownClient.Logic
{
    public class DownloadAction
    {

        #region [ Data ]

        public virtual RemoteClient Client { get; protected set; }
        public virtual DataStore Store { get; protected set; }

        public virtual int BufferSize { get; protected set; }

        #endregion

        #region [ Construction ]

        public DownloadAction(RemoteClient client, DataStore store)
        {
            if (client == null || store == null)
            {
                string paramName = client == null ? "client" : "store";
                throw new ArgumentNullException(string.Format("{0} is null!", paramName), paramName);
            }
            Client = client;
            Store = store;
            BufferSize = 16;
        }

        public DownloadAction(RemoteClient client, DataStore store, int bufferSize)
            : this(client, store)
        {
            BufferSize = bufferSize;
        }

        #endregion

        #region [ Events ]

        public event EventHandler<ItemDownloadEventArgs> DownloadStarted;
        public event EventHandler<ItemDownloadEventArgs> DownloadCompleted;
        public event EventHandler<ItemDownloadEventArgs> DownloadFailed;

        #endregion

        #region [ Operations ]

        public void Start()
        {
            onDownloadStart();
            try
            {
                using (IOutputStream stream = Store.CreateFile(Client.Address.ResourceName))
                {
                    int bytesRead;
                    byte[] buffer = new byte[BufferSize];
                    while ((bytesRead = Client.Read(buffer, 0, BufferSize)) > 0)
                    {
                        stream.Write(buffer, 0, bytesRead);
                    }
                }
                onDownloadComplete();
            }
            catch (Exception ex)
            {
                onDownloadFail();
                throw ex;
            }
        }

        private void onDownloadFail()
        {
            try
            {
                Store.DeleteFile(Client.Address.ResourceName);
            }
            catch(FileNotFoundException)
            { }
            if (DownloadFailed != null)
                DownloadFailed(this, new ItemDownloadEventArgs(Client.Address, Store));
        }

        private void onDownloadStart()
        {
            if (DownloadStarted != null)
                DownloadStarted(this, new ItemDownloadEventArgs(Client.Address, Store));
        }

        private void onDownloadComplete()
        {
            if (DownloadCompleted != null)
                DownloadCompleted(this, new ItemDownloadEventArgs(Client.Address, Store));
        }

        #endregion

    }
}
