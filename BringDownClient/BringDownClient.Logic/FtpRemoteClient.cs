/*
 * Class: FtpRemoteClient
 * Author: Pradeep Singh
 * Date: 4 July 2016
 * Change Log:
 *      4 July 2016: Created the class.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BringDownClient.Logic.Core;

namespace BringDownClient.Logic
{
    public class FtpRemoteClient : RemoteClient
    {
        
        #region [ Constants ]

        public const string Factory_Supported_Version = "1.0";

        #endregion

        #region [ Data ]

        /// <summary>
        /// Internal Web Request instance
        /// </summary>
        protected FtpWebRequest _webRequest;

        /// <summary>
        /// Credentials cache object. We could have stored a 
        /// single <seealso cref="System.Net.NetworkCredential"/> object but
        /// <seealso cref="System.Net.WebRequest"/> has an internal redirection mechanism and
        /// storing a <seealso cref="System.Net.CredentialCache"/> object
        /// helps in having the ability to specify credentials for each
        /// host name that can be a part of redirection process.
        /// </summary>
        protected CredentialCache _credentialCache;

        #endregion

        #region [ Static Methods ]

        public static bool SupportsProtocol(string protocol)
        {
            return (protocol.ToLower() == "ftp");
        }

        #endregion

        #region [ Construction ]

        /// <summary>
        /// Creates a basic HttpRemoteClient.
        /// </summary>
        /// <param name="address">address of the remote resource.</param>
        internal FtpRemoteClient(RemoteAddress address)
            : this(address, null)
        {

        }

        /// <summary>
        /// Creates a HttpRemoteClient with authentication type and credential mapping for host redirections.
        /// </summary>
        /// <param name="address">address of the remote resource.</param>
        /// <param name="authenticationType">authentication type</param>
        /// <param name="credentials">credential to host mapping cache.</param>
        internal FtpRemoteClient(RemoteAddress address, NetworkCredential credentials = null)
            : base(address)
        {
            //create a new web request
            if (address == null)
                throw new ArgumentNullException("Remote address is null!");
            if (!address.IsValid)
                throw new ArgumentException("Remote address is invalid!");
            Address = address;
            setupWebRequest(credentials);
        }

        /// <summary>
        /// Setup the Web Request to communicate with remote server
        /// </summary>
        /// <param name="authenticationType">authentication type</param>
        /// <param name="credentials">credential to host mapping cache.</param>
        protected virtual void setupWebRequest(NetworkCredential credentials)
        {
            _webRequest = (FtpWebRequest)FtpWebRequest.Create(Address.RawUrl);
            //Add the credentials
            if (credentials != null)
                _webRequest.Credentials = credentials;
            _dataStream = _webRequest.GetResponse().GetResponseStream();
            CanRead = true;
        }

        #endregion

    }
}
