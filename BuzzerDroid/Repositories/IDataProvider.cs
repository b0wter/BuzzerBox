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

namespace BuzzerDroid.Repositories
{
    interface IDataProvider<T> where T : BaseModel
    {
        Task<List<T>> LoadInitialItems(int number);
        Task<List<T>> LoadNextItems(int offset, int number);
    }
}