/*
 * Interface: IClient
 * Author: Pradeep Singh
 * Date: 2 July 2016
 * Change Log:
 *      2 July 2016: Created the interface.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace BringDownClient.Logic.Core
{
    /// <summary>
    /// Provides an interface to read data in a sequential order.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Read the underlying stream of data.
        /// </summary>
        /// <param name="buffer">buffer for holding the data</param>
        /// <param name="offset">starting position in the buffer</param>
        /// <param name="length">number of bytes to fill in the bufffer</param>
        /// <returns></returns>
        int Read(byte[] buffer, int offset, int length);

        /// <summary>
        /// Get whether the client is connected or not
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Get whether the client can read or not
        /// </summary>
        bool CanRead { get; }
    }
}
