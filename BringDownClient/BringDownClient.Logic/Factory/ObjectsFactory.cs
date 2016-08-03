/*
 * Class: ObjectsFactory
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
using BringDownClient.Logic;
using System.Reflection;
using System.Net;
using BringDownClient.Logic.Core;
using BringDownClient.Logic.Factory;

namespace BringDownClient.Logic.Factory
{
    /// <summary>
    /// ObjectsFactory class is the factory for generating objects that will be used by consumers of the library
    /// </summary>
    public class ObjectsFactory
    {
        
        #region [ Data ]

        /// <summary>
        /// Version of the factory.
        /// </summary>
        private const string Version = "1.0";

        /// <summary>
        /// Gets the Version Information for factory
        /// </summary>
        public Version FactoryVersion { get { return new Version(Version); } }

        #endregion

        #region [ Singleton ]

        /// <summary>
        /// Gets the Factory reference.
        /// </summary>
        public static ObjectsFactory Factory { get; private set; }

        static ObjectsFactory()
        {
            Factory = new ObjectsFactory();
        }

        /// <summary>
        /// Creates a new Instance of the factory
        /// </summary>
        private ObjectsFactory()
        {
            
        }

        #endregion

        #region [ Operations ]
        /// <summary>
        /// Returns a DataDownloader object
        /// </summary>
        /// <returns>a IDataDownloader instance</returns>
        public IDataDownloader GetDataDownloader()
        {
            return this.GetDownloader();
        }

        /// <summary>
        /// Returns a RemoteClient based on the protocol.
        /// </summary>
        /// <param name="url">remote url</param>
        /// <returns>a RemoteClient instance</returns>
        public RemoteClient GetRemoteClient(string url)
        {
            return GetRemoteClient(new RemoteAddress(url));
            
        }

        /// <summary>
        /// Returns a RemoteClient based on the protocol.
        /// </summary>
        /// <param name="address">remote address</param>
        /// <returns>a RemoteClient instance</returns>
        public RemoteClient GetRemoteClient(RemoteAddress address)
        {
            if (HttpRemoteClient.SupportsProtocol(address.Protocol))
                return this.GetHttpRemoteClient(address);
            if (FtpRemoteClient.SupportsProtocol(address.Protocol))
                return this.GetFtpRemoteClient(address);
            throw new NotSupportedException("address is not supported");
        }

        /// <summary>
        /// Returns a DataStore based on the location.
        /// </summary>
        /// <param name="location">location for the data store</param>
        /// <returns>a DataStore instance</returns>
        public DataStore GetDataStore(string location)
        {
            if (LocalDataStore.SupportsLocation(location))
                return this.GetLocalDataStore(location);
            throw new NotSupportedException("location is not supported!");
        }

        #endregion

    }
}
