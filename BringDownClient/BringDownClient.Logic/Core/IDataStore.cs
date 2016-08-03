/*
 * Interface: IDataStore
 * Author: Pradeep Singh
 * Date: 3 July 2016
 * Change Log:
 *      3 July 2016: Created the interface.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BringDownClient.Logic.Core
{
    public interface IDataStore
    {
        string Location { get; }
        bool IsRemote { get; }
        ICredentials Credentials { get; }
        IOutputStream CreateFile(string fileName);
        void DeleteFile(string fileName);
        List<string> FileList { get; }

    }
}
