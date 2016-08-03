/*
 * Class: DataDownloaderFactory
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

namespace BringDownClient.Logic.Factory
{
    /// <summary>
    /// Adds the capability to generate DataDownloader to the ObjectsFactory class.
    /// </summary>
    internal static class HttpRemoteClientFactory
    {
        /// <summary>
        /// Gets FtpRemoteClient.
        /// </summary>
        /// <param name="factory">ObjectsFactory instance</param>
        /// <param name="remoteAddress">remote HTTP/HTTPS address</param>
        /// <returns>a HttpRemoteClient instance</returns>
        internal static HttpRemoteClient GetHttpRemoteClient(this ObjectsFactory factory, string remoteAddress)
        {
            if (factory.FactoryVersion < new Version(HttpRemoteClient.Factory_Supported_Version))
                throw new NotSupportedException("factory doesn't support this version");
            RemoteAddress address = new RemoteAddress(remoteAddress);
            if (address.Protocol.ToLower() == "http" || address.Protocol.ToLower() == "https")
                return new HttpRemoteClient(address);
            else
                throw new NotSupportedException("Protocol Not Supported!");
        }

        /// <summary>
        /// Gets FtpRemoteClient.
        /// </summary>
        /// <param name="factory">ObjectsFactory instance</param>
        /// <param name="remoteAddress">remote HTTP/HTTPS address</param>
        /// <returns>a HttpRemoteClient instance</returns>
        internal static HttpRemoteClient GetHttpRemoteClient(this ObjectsFactory factory, RemoteAddress remoteAddress)
        {
            if (factory.FactoryVersion < new Version(HttpRemoteClient.Factory_Supported_Version))
                throw new NotSupportedException("factory doesn't support this version");
            if (remoteAddress.Protocol.ToLower() == "http" || remoteAddress.Protocol.ToLower() == "https")
                return new HttpRemoteClient(remoteAddress);
            else
                throw new NotSupportedException("Protocol Not Supported!");
        }
    }
}
