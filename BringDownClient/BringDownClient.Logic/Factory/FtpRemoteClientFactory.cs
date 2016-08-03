/*
 * Class: FtpRemoteClientFactory
 * Author: Pradeep Singh
 * Date: 4 July 2016
 * Change Log:
 *      4 July 2016: Created the class.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BringDownClient.Logic.Factory
{
    /// <summary>
    /// Adds the capability to generate FtpRemoteClient to the ObjectsFactory class.
    /// </summary>
    internal static class FtpRemoteClientFactory
    {
        /// <summary>
        /// Gets FtpRemoteClient.
        /// </summary>
        /// <param name="factory">ObjectsFactory instance</param>
        /// <param name="remoteAddress">remote FTP address</param>
        /// <returns>a FtpRemoteClient instance</returns>
        internal static FtpRemoteClient GetFtpRemoteClient(this ObjectsFactory factory, string remoteAddress)
        {
            if (factory.FactoryVersion < new Version(HttpRemoteClient.Factory_Supported_Version))
                throw new NotSupportedException("factory doesn't support this version");
            RemoteAddress address = new RemoteAddress(remoteAddress);
            if (address.Protocol.ToLower() == "ftp")
                return new FtpRemoteClient(address);
            else
                throw new NotSupportedException("Protocol Not Supported!");
        }

        /// <summary>
        /// Gets FtpRemoteClient.
        /// </summary>
        /// <param name="factory">ObjectsFactory instance</param>
        /// <param name="remoteAddress">remote FTP address instance</param>
        /// <returns>a FtpRemoteClient instance</returns>
        internal static FtpRemoteClient GetFtpRemoteClient(this ObjectsFactory factory, RemoteAddress remoteAddress)
        {
            if (factory.FactoryVersion < new Version(HttpRemoteClient.Factory_Supported_Version))
                throw new NotSupportedException("factory doesn't support this version");
            if (remoteAddress.Protocol.ToLower() == "ftp")
                return new FtpRemoteClient(remoteAddress);
            else
                throw new NotSupportedException("Protocol Not Supported!");
        }
    }
}
