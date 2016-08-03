/*
 * Class: DownloadEventArgs
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
    public class DownloadEventArgs : EventArgs
    {
        public int TotalItems { get; private set; }
        public int ItemsCompleted { get; private set; }
        public int ItemsFailed { get; private set; }

        public DownloadEventArgs(int totalItem, int itemsCompleted = 0, int itemsFailed = 0)
        {
            TotalItems = totalItem;
            ItemsCompleted = itemsCompleted;
            ItemsFailed = itemsFailed;
        }
    }
}
