/*
 * Class: RemoteAddress
 * Author: Pradeep Singh
 * Date: 2 July 2016
 * Change Log:
 *      2 July 2016: Created the class.
 *      2 July 2016: Changed access modifier to internal for setter for ResourceName property.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BringDownClient.Logic.Core;

namespace BringDownClient.Logic
{
    /// <summary>
    /// Provides logic for parsing the Remote Urls
    /// </summary>
    public class RemoteAddress
    {

        #region [ Construction ]

        /// <summary>
        /// Returns a new instance of RemoteAddress object based on the Url.
        /// </summary>
        /// <param name="rawUrl">Raw string url for the remote resource.</param>
        public RemoteAddress(string rawUrl)
        {
            RawUrl = rawUrl;
            Regex protocolParser = new Regex(@"^(?<proto>[a-z]{3,6})://(?<host>[a-z0-9\-.]{1,50})(?<portDelimiter>[:]{1})?(?<port>[a-z0-9]{1,5})?/",
                          RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(150));
            Match protocolMatch = protocolParser.Match(rawUrl);
            
            if (!protocolMatch.Success)
            {
                IsValid = false;
                throw new UriFormatException("The uri does not contain a protocol descripter.");
            }
            else
            {
                Protocol = string.IsNullOrEmpty(protocolParser.Match(rawUrl).Result("${proto}")) ? string.Empty : protocolParser.Match(rawUrl).Result("${proto}");
                HostName = string.IsNullOrEmpty(protocolParser.Match(rawUrl).Result("${host}")) ? string.Empty : protocolParser.Match(rawUrl).Result("${host}");
                protocolAndHostCheck();
                try
                {
                    string port = protocolParser.Match(rawUrl).Result("${port}");
                    Port = string.IsNullOrEmpty(port) ? -1 : int.Parse(port);
                }
                catch(Exception ex)
                {
                    IsValid = false;
                    throw new UriFormatException("The port number specified is not valid!", ex);
                }
            }
            Match fileMatch = Regex.Match(rawUrl, @"(?=\w+\.\w{3,4}$).+", RegexOptions.IgnoreCase);
            if (fileMatch.Success)
                ResourceName = fileMatch.Value;
            
            IsValid = true;
        }

        /// <summary>
        /// Sanity check for Protocol and Host
        /// </summary>
        private void protocolAndHostCheck()
        {
            
            if (string.IsNullOrEmpty(Protocol))
            {
                IsValid = false;
                throw new UriFormatException("The protocol is missing!");
            }
            if (string.IsNullOrEmpty(Protocol))
            {
                IsValid = false;
                throw new UriFormatException("The hostname is missing!");
            }
        }

        #endregion

        #region [ Data ]
        /// <summary>
        /// Gets the Raw URL representation.
        /// </summary>
        public string RawUrl { get; protected set; }

        /// <summary>
        /// Gets the Protocol String.
        /// </summary>
        public string Protocol { get; protected set; }

        /// <summary>
        /// Gets the Port number
        /// </summary>
        public int Port { get; protected set; }

        /// <summary>
        /// Gets the Identifier for the remote resource (fileName generally).
        /// </summary>
        public string ResourceName { get; internal set; }

        /// <summary>
        /// Gets remote Host Name (e.g.: www.test.com)
        /// </summary>
        public string HostName { get; protected set; }

        /// <summary>
        /// Gets if the object is valid or not.
        /// </summary>
        public bool IsValid { get; protected set; }

        #endregion

    }
}
