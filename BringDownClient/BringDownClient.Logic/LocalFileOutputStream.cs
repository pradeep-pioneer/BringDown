/*
 * Class: LocalFileOutputStream
 * Author: Pradeep Singh
 * Date: 3 July 2016
 * Change Log:
 *      3 July 2016: Created the class.
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
    public class LocalFileOutputStream : OutputStream
    {
        #region [ Construction ]

        public LocalFileOutputStream(string fileName)
            : base(fileName)
        {
            if (File.Exists(fileName))
                _dataStream = File.Open(fileName, FileMode.Truncate, FileAccess.Write);
            else
                _dataStream = File.Open(fileName, FileMode.Create, FileAccess.Write);
            _dataStream.Seek(0, SeekOrigin.Begin);
            Connected = true;
            CanWrite = _dataStream.CanWrite;
            Completed = false;
        }

        #endregion
    }
}
