/*
 * Class: ItemDownloadEventArgs
 * Author: Pradeep Singh
 * Date: 3 July 2016
 * Change Log:
 *      3 July 2016: Created the class.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BringDownClient.Logic.Core;

namespace BringDownClient.Logic
{
    public class ItemDownloadEventArgs:EventArgs
    {
        public RemoteAddress Source { get; private set; }
        public DataStore Destination { get; private set; }

        public ItemDownloadEventArgs(RemoteAddress source, DataStore destination) { Source = source; Destination = destination; }
    }
}
