/*
 * Class: LocalDataStore
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BringDownClient.Logic.Core;

namespace BringDownClient.Logic
{
    public class LocalDataStore : DataStore
    {

        #region [ Constants ]

        public const string Factory_Supported_Version = "1.0";

        #endregion

        #region overrides

        protected DirectoryInfo _directoryInformation;

        public override string Location
        {
            get
            {
                return _directoryInformation.FullName;
            }
            protected set
            {
                _directoryInformation = new DirectoryInfo(value);
            }
        }

        #endregion

        #region [ Operations ]

        public override IOutputStream CreateFile(string fileName)
        {
            IOutputStream outputStream = new LocalFileOutputStream(string.Format("{0}\\{1}", Location, fileName));
            if(FileList.Select(x=>x.ToLower()).Contains(fileName.ToLower()))
            {
                FileList.Add(fileName);
            }
            return outputStream;
        }

        public override void DeleteFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Filename is invalid!");
            if (!File.Exists(string.Format("{0}\\{1}", Location, fileName)))
                throw new FileNotFoundException("File not found!", fileName);
            File.Delete(string.Format("{0}\\{1}", Location, fileName));
        }

        #endregion

        #region [ Construction ]

        public LocalDataStore(string location)
            : base(location)
        {
            if (Directory.Exists(location))
            {
                _directoryInformation = new DirectoryInfo(location);
                refreshFileList();
            }
            else
                _directoryInformation = Directory.CreateDirectory(location);
        }

        protected virtual void refreshFileList()
        {
            FileList.Clear();
            foreach (var item in _directoryInformation.GetFiles())
            {
                
                FileList.Add(item.Name);

            }
        }

        #endregion

        #region [ Static Methods ]

        public static bool SupportsLocation(string location)
        {
            return Regex.Match(location, @"([a-z][:][\\])", RegexOptions.IgnoreCase).Success;
        }

        #endregion

    }
}
