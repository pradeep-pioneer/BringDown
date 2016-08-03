/*
 * Class: DataStore
 * Author: Pradeep Singh
 * Date: 3 July 2016
 * Change Log:
 *      3 July 2016: Created the class.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BringDownClient.Logic.Core;

namespace BringDownClient.Logic
{
    public abstract class DataStore : IDataStore
    {

        #region [ IDataStore ]

        public virtual string Location { get; protected set; }

        public virtual bool IsRemote { get; protected set; }

        public virtual System.Net.ICredentials Credentials { get; protected set; }

        public abstract IOutputStream CreateFile(string fileName);

        public abstract void DeleteFile(string fileName);

        public virtual List<string> FileList { get; set; }

        #endregion

        #region [ Construction ]

        protected DataStore(string location)
        {
            if (string.IsNullOrWhiteSpace(location) || string.IsNullOrEmpty(location))
                throw new ArgumentException("Location is invalid");
            Location = location;
            FileList = new List<string>();
        }

        #endregion

    }
}
