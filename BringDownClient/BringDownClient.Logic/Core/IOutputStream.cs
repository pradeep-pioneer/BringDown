/*
 * Interface: IDataDownloader
 * Author: Pradeep Singh
 * Date: 3 July 2016
 * Change Log:
 *      3 July 2016: Created the interface.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BringDownClient.Logic.Core
{
    public interface IOutputStream : IDisposable
    {

        void Write(byte[] buffer, int offset, int length);
        void Close();
        ICredentials Credentials { get; }
        string FileName { get; }
        bool CanWrite { get; }
        bool Completed { get; }
        bool Connected { get; }
    }
}
