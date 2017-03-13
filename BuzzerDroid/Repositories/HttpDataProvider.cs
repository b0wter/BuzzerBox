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
using System.Threading.Tasks;
using BuzzerDroid.Repositories;
using BuzzerEntities.Models;

namespace BuzzerDroid.Repositories
{
    internal class HttpDataProvider<T> : IDataProvider<T> where T : BaseModel
    {
        public Task<List<T>> LoadInitialItems(int number)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> LoadNextItems(int offset, int number)
        {
            throw new NotImplementedException();
        }
    }
}