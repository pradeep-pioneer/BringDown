/*
 * Class: DataDownloaderFactory
 * Author: Pradeep Singh
 * Date: 3 July 2016
 * Change Log:
 *      3 July 2016: Created the class.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BringDownClient.Logic.Factory
{
    /// <summary>
    /// Adds the capability to generate DataDownloader to the ObjectsFactory class.
    /// </summary>
    internal static class LocalDataStoreFactory
    {
        /// <summary>
        /// Gets LocalDataStore.
        /// </summary>
        /// <param name="factory">ObjectsFactory instance</param>
        /// <param name="location">local storage location</param>
        /// <returns></returns>
        internal static LocalDataStore GetLocalDataStore(this ObjectsFactory factory, string location)
        {
            if (factory.FactoryVersion < new Version(LocalDataStore.Factory_Supported_Version))
                throw new NotSupportedException("factory doesn't support this version");
            Match match = Regex.Match(location, @"(?<drive>[a-z][:][\\])", RegexOptions.IgnoreCase);
            if (match.Success)
                return new LocalDataStore(location);
            else
                throw new NotSupportedException("location not supported");
        }

    }
}
