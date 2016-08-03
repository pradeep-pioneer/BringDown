/*
 * Class: DataDownloader
 * Author: Pradeep Singh
 * Date: 3 July 2016
 * Change Log:
 *      3 July 2016: Created the class.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BringDownClient.Logic.Factory;
using BringDownClient.Logic.Core;

namespace BringDownClient.Logic
{
    /// <summary>
    /// Base class for all download operations
    /// </summary>
    public class DataDownloader : IDataDownloader
    {

        #region [ Constants ]

        /// <summary>
        /// Version of the factory that supports this object.
        /// </summary>
        public const string Factory_Supported_Version = "1.0";

        #endregion

        #region [ Data ]

        /// <summary>
        /// List of DownloadAction
        /// </summary>
        protected List<DownloadAction> _downloadActions;
        /// <summary>
        /// Items that have been completed
        /// </summary>
        protected int _itemsCompleted;
        /// <summary>
        /// Items that have failed
        /// </summary>
        protected int _itemsFailed;

        /// <summary>
        /// Current DownloadAction
        /// </summary>
        protected DownloadAction _currentAction;

        #endregion

        #region [ IDataDownloader ]

        /// <summary>
        /// Flag to indicate if the download is in progress
        /// </summary>
        public virtual bool IsDownloading { get; protected set; }

        /// <summary>
        /// Queue a download action.
        /// </summary>
        /// <param name="client">Source remote client.</param>
        /// <param name="dataStore">Destination data source.</param>
        public void QueueDownload(RemoteClient client, DataStore dataStore)
        {
            if (_downloadActions.Count(x => x.Client.Address.RawUrl == client.Address.RawUrl) > 0 && _downloadActions.Count(x => x.Store.Location == dataStore.Location) > 0)
                throw new InvalidOperationException("This operation is already queued");
            
            DownloadAction action = new DownloadAction(client, dataStore);
            _downloadActions.Add(action);
            action.DownloadStarted += action_DownloadStarted;
            action.DownloadFailed += action_DownloadFailed;
            action.DownloadCompleted += action_DownloadCompleted;
        }

        /// <summary>
        /// called when the download completes
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event arguments</param>
        void action_DownloadCompleted(object sender, ItemDownloadEventArgs e)
        {
            _itemsCompleted++;
            onItemDownloaded(e);
        }

        /// <summary>
        /// called when the download fails
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event arguments</param>
        void action_DownloadFailed(object sender, ItemDownloadEventArgs e)
        {
            _itemsFailed++;
            onItemFailed(e);
        }

        /// <summary>
        /// called when the download starts
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event arguments</param>
        void action_DownloadStarted(object sender, ItemDownloadEventArgs e)
        {
            onItemStarted(e);
        }

        #region [ Operations ]

        /// <summary>
        /// Starts the download process.
        /// </summary>
        public virtual void Start()
        {
            if (IsDownloading)
                throw new InvalidOperationException("Download Already in Progress!");
            onDownloadStarted(new DownloadEventArgs(_downloadActions.Count));
            IsDownloading = true;
            foreach (var item in _downloadActions)
            {
                _currentAction = item;
                item.Start();
            }
            IsDownloading = !IsDownloading;
            onDownloadCompleted(new DownloadEventArgs(_downloadActions.Count, _itemsCompleted, _itemsFailed));
        }

        /// <summary>
        /// Not Implemented: Stops the download process.
        /// </summary>
        public virtual void Stop()
        {
            //This action can be implemented by subclasses that might perform async downloads
            throw new NotImplementedException();
        }

        #endregion

        #region [ Events ]

        /// <summary>
        /// Raised when the Item starts downloading.
        /// </summary>
        public event EventHandler<ItemDownloadEventArgs> ItemStarted;

        /// <summary>
        /// Raises ItemStarted event.
        /// </summary>
        /// <param name="eventArgs">event arguments</param>
        protected void onItemStarted(ItemDownloadEventArgs eventArgs)
        {
            if (this.ItemStarted != null)
                this.ItemStarted(this, eventArgs);
        }

        /// <summary>
        /// Raised when the item completes downloading
        /// </summary>
        public event EventHandler<ItemDownloadEventArgs> ItemDownloaded;

        /// <summary>
        /// Raises ItemDownloaded event.
        /// </summary>
        /// <param name="eventArgs">event arguments</param>
        protected void onItemDownloaded(ItemDownloadEventArgs eventArgs)
        {
            if (this.ItemDownloaded != null)
                this.ItemDownloaded(this, eventArgs);
        }

        /// <summary>
        /// Raised when items fails downloading
        /// </summary>
        public event EventHandler<ItemDownloadEventArgs> ItemFailed;

        /// <summary>
        /// Raises ItemFailed event.
        /// </summary>
        /// <param name="eventArgs">event arguments</param>
        protected void onItemFailed(ItemDownloadEventArgs eventArgs)
        {
            if (this.ItemFailed != null)
                this.ItemFailed(this, eventArgs);
        }

        /// <summary>
        /// Raised when all downloads finish
        /// </summary>
        public event EventHandler<DownloadEventArgs> DownloadCompleted;

        /// <summary>
        /// Raises DownloadCompleted event.
        /// </summary>
        /// <param name="eventArgs">event arguments</param>
        protected void onDownloadCompleted(DownloadEventArgs e)
        {
            if (this.DownloadCompleted != null)
                this.DownloadCompleted(this, e);
        }

        /// <summary>
        /// Raised when download batch starts
        /// </summary>
        public event EventHandler<DownloadEventArgs> DownloadStarted;

        /// <summary>
        /// Raises DownloadStarted event.
        /// </summary>
        /// <param name="eventArgs">event arguments</param>
        protected void onDownloadStarted(DownloadEventArgs e)
        {
            if (this.DownloadStarted != null)
                this.DownloadStarted(this, e);
        }

        #endregion

        #endregion

        #region [ Construction ]

        /// <summary>
        /// Creates a new DataDownloader instance
        /// </summary>
        internal DataDownloader()
        {
            _downloadActions = new List<DownloadAction>();
            _itemsCompleted = 0; _itemsFailed = 0;
            _currentAction = null;
        }

        #endregion

    }
}
