/*
 * Class: HttpRemoteClient
 * Author: Pradeep Singh
 * Date: 2 July 2016
 * Change Log:
 *      2 July 2016: Created the class.
 *      2 July 2016: Added eager loading of Resource Details (size, name etc.)
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

    /// <summary>
    /// Basic http remote client, supports Authentication and Redirection credentials cache.
    /// </summary>
    public class HttpRemoteClient : RemoteClient
    {

        #region [ Constants ]

        public const string Factory_Supported_Version = "1.0";

        #endregion

        #region [ Construction ]

        /// <summary>
        /// Creates a basic HttpRemoteClient.
        /// </summary>
        /// <param name="address">address of the remote resource.</param>
        internal HttpRemoteClient(RemoteAddress address)
            : this(address, null)
        {

        }

        /// <summary>
        /// Creates a HttpRemoteClient with authentication type and credentials.
        /// </summary>
        /// <param name="address">address of the remote resource.</param>
        /// <param name="credential">remote credentials.</param>
        /// <param name="authenticationType">authentication type.</param>
        internal HttpRemoteClient(RemoteAddress address, NetworkCredential credential = null, string authenticationType = "Basic")
            : this(address,authenticationType,credential)
        {
            
        }

        /// <summary>
        /// Creates a HttpRemoteClient with authentication type and credential mapping for host redirections.
        /// </summary>
        /// <param name="address">address of the remote resource.</param>
        /// <param name="authenticationType">authentication type</param>
        /// <param name="credentials">credential to host mapping cache.</param>
        internal HttpRemoteClient(RemoteAddress address, string authenticationType = "Basic", params NetworkCredential[] credentials)
            : base(address)
        {
            //create a new web request
            if (address == null)
                throw new ArgumentNullException("Remote address is null!");
            if (!address.IsValid)
                throw new ArgumentException("Remote address is invalid!");
            Address = address;
            setupWebRequest(authenticationType, credentials);
            //Get the head information for the resource - eager loading
            loadRemoteResourceDetails();
        }

        /// <summary>
        /// Setup the Web Request to communicate with remote server
        /// </summary>
        /// <param name="authenticationType">authentication type</param>
        /// <param name="credentials">credential to host mapping cache.</param>
        protected virtual void setupWebRequest(string authenticationType = "Basic", params NetworkCredential[] credentials)
        {
            _webRequest = WebRequest.Create(Address.RawUrl);
            //Add the credentials
            foreach (var item in credentials)
            {
                if (item != null)
                {
                    _credentialCache = new CredentialCache();
                    _credentialCache.Add(new Uri(string.Format("{0}://{1}{2}", Address.Protocol, Address.HostName, Address.Port == -1 ? string.Empty : string.Format(":{0}", Address.Port))), authenticationType, item);
                    _webRequest.Credentials = _credentialCache;
                }
            }
            _dataStream = _webRequest.GetResponse().GetResponseStream();
            CanRead = true;
        }

        protected virtual void loadRemoteResourceDetails()
        {
            _webRequest.Method = "HEAD";
            WebResponse response = _webRequest.GetResponse();
            ResourceSize = string.IsNullOrEmpty(response.Headers["Content-Length"]) ? -1 : int.Parse(response.Headers["Content-Length"]);
            if (Address.ResourceName == null)
            {
                if (response.Headers["Content-Disposition"] != null)
                {
                    string[] content = response.Headers["Content-Disposition"].Split(';');
                    string fileName = content.Where(x => x.ToLower().Contains("filename")).FirstOrDefault();
                    Address.ResourceName = fileName != null ? fileName.Split('=')[1].Trim() : string.Format("{0}{1}.{2}", Address.HostName, DateTime.Now.ToString("yyyyMMddHHmmss"), "file");
                }
                else
                    Address.ResourceName = string.Format("{0}{1}.{2}", Address.HostName, DateTime.Now.ToString("yyyyMMddHHmmss"), "file");
            }
            _webRequest.Method = "GET";
        }

        #endregion

        #region [ Data ]

        /// <summary>
        /// Internal Web Request instance
        /// </summary>
        protected WebRequest _webRequest;
        
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
            return (protocol.ToLower() == "http" || protocol.ToLower() == "https");
        }

        #endregion

    }
}
