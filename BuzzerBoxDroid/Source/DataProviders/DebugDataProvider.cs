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
using Newtonsoft.Json;
using BuzzerBoxDroid.Source.Helpers;

namespace BuzzerBoxDroid.Source.DataProviders
{
    internal static class DebugDataProvider
    {
        public static List<Room> Rooms
        {
            get
            {
                var rooms = JsonConvert.DeserializeObject<List<Room>>(DebugData.DummyRoomsString);
                return rooms;
            }
        }

        public static List<Question> Questions
        {
            get
            {
                var questions = Rooms.OrderByDescending(r => r.Questions.Count).First().Questions;
                return questions;
            }
        }

        public static User User
        {
            get
            {
                return JsonConvert.DeserializeObject<User>(DebugData.DummyLoggedInUserString);
            }
        }
    }
}