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
using System.Threading.Tasks;

namespace BringDownClient.Logic.Factory
{
    /// <summary>
    /// Adds the capability to generate DataDownloader to the ObjectsFactory class.
    /// </summary>
    internal static class DataDownloaderFactory
    {
        /// <summary>
        /// Gets the DataDownloader
        /// </summary>
        /// <param name="factory">ObjectsFactory instance</param>
        /// <returns>a DataDownloader instance.</returns>
        internal static DataDownloader GetDownloader(this ObjectsFactory factory)
        {
            if (factory.FactoryVersion < new Version(LocalDataStore.Factory_Supported_Version))
                throw new NotSupportedException("factory doesn't support this version");
            return new DataDownloader();
        }
    }
}
