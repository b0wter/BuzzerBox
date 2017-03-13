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

namespace BuzzerDroid.Repositories
{
    internal static class DataProvider
    {
        public static IDataProvider<Room> RoomsProvider { get; } = new HttpDataProvider<Room>();
        public static IDataProvider<Question> QuestionsProvider { get; } = new HttpDataProvider<Question>();
    }
}