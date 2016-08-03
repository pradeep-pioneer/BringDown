/*
 * Class: RemoteClient
 * Author: Pradeep Singh
 * Date: 2 July 2016
 * Change Log:
 *      2 July 2016: Created the class.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BringDownClient.Logic.Core;

namespace BringDownClient.Logic
{
    /// <summary>
    /// Base class for Remote Clients, This class can be inherited/extended to create protocol specific reader.
    /// </summary>
    public abstract class RemoteClient : IClient
    {
        #region [ IClient ]

        /// <summary>
        /// Read the underlying stream of data.
        /// </summary>
        /// <param name="buffer">buffer for holding the data</param>
        /// <param name="offset">starting position in the buffer</param>
        /// <param name="length">number of bytes to fill in the bufffer</param>
        /// <returns>number of bytes read from the underlying stream of data</returns>
        public virtual int Read(byte[] buffer, int offset, int length)
        {
            if (_dataStream == null)
                throw new NullReferenceException("Data stream not initialized!");
            if (!(_dataStream.CanRead) || (!CanRead))
                throw new InvalidOperationException("Cannot read from data stream");
            return _dataStream.Read(buffer, offset, length);
        }

        /// <summary>
        /// Get whether the client is connected or not
        /// </summary>
        public virtual bool Connected { get; protected set; }

        /// <summary>
        /// Get whether the client can read or not
        /// </summary>
        public virtual bool CanRead { get; protected set; }

        #endregion

        #region [ Data ]
        
        /// <summary>
        /// Local reference to underlying stream of data
        /// </summary>
        protected Stream _dataStream;

        /// <summary>
        /// Remote resource information
        /// </summary>
        public RemoteAddress Address { get; protected set; }

        public int ResourceSize { get; protected set; }

        #endregion

        #region [ Construction ]

        /// <summary>
        /// Creates a new instance of RemoteClient (this class abstract but this constructor can be used to setup the RemoteAddress in inherited classes.
        /// </summary>
        /// <param name="address">remote address</param>
        protected RemoteClient(RemoteAddress address)
        {
            if (!address.IsValid) { throw new ArgumentException("Supplied address is invalid!"); }
            Address = address;
        }

        #endregion

    }
}
