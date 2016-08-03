/*
 * Class: OutputStream
 * Author: Pradeep Singh
 * Date: 3 July 2016
 * Change Log:
 *      3 July 2016: Created the class.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BringDownClient.Logic.Core;

namespace BringDownClient.Logic
{
    public abstract class OutputStream : IOutputStream
    {

        #region [ IOutputStream ]

        public virtual void Write(byte[] buffer, int offset, int length)
        {
            if (_dataStream == null)
                throw new NullReferenceException("Data stream not initialized!");
            if (!(_dataStream.CanWrite) || (!CanWrite))
                throw new InvalidOperationException("Cannot write to data stream");
            _dataStream.Write(buffer, offset, length);
            _dataStream.Flush();
        }

        public virtual string FileName { get; protected set; }

        public virtual bool CanWrite { get; protected set; }

        public virtual bool Completed { get; set; }

        public virtual bool Connected { get; protected set; }

        public virtual ICredentials Credentials { get; protected set; }

        public virtual void Close() { _dataStream.Close(); }

        #endregion

        #region [ Data ]

        protected Stream _dataStream;

        #endregion

        #region [ Construction ]

        protected OutputStream(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("Filename is null!");
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Filename is empty!");
            FileName = fileName;
        }

        #endregion

        #region [ IDisposable ]

        public virtual void Dispose()
        {
            _dataStream.Flush();
            _dataStream.Close();
            _dataStream.Dispose();
        }

        #endregion

    }
}
