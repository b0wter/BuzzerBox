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

namespace BuzzerBoxDroid.Source.Models
{
    public class ResponseEx : Response
    {
        [JsonIgnore]
        public bool IsLocalUserVote { get; set; } = false;
    }
}