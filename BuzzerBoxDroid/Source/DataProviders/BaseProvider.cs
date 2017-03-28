using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BuzzerEntities.Models;
using System.Threading.Tasks;

namespace BuzzerBoxDroid.Source.DataProviders
{
    internal abstract class BaseProvider<T> where T : BaseModel
    {
        public abstract Task<List<T>> LoadItems();
    }
}