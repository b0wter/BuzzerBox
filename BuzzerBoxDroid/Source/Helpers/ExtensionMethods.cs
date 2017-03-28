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

namespace BuzzerBoxDroid.Source.Helpers
{
    public static class ExtensionMethods
    {
        public static bool IsLocalUserVote(this Response response, int localUserId)
        {
            if (response.Votes == null || response.Votes.Count < 1)
                return false;
            return response.Votes.Any(v => v.UserId == localUserId);
        }
    }
}