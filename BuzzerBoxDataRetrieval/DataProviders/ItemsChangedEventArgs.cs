using System;
using System.Collections.Generic;
using BuzzerEntities.Models;

namespace BuzzerBoxDataRetrieval.DataProviders
{
    public class ItemsChangedEventArgs<T> : EventArgs
    {
        public Operations Operation { get; private set; }
        public List<T> ItemsChanged { get; private set; }

        public ItemsChangedEventArgs(Operations operation, List<T> items)
        {
            this.Operation = Operation;
            this.ItemsChanged = items;
        }
    }

    public enum Operations
    {
        Added,
        Removed,
        Updated
    }
}