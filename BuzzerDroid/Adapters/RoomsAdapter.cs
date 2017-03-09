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
using Android.Support.V7.Widget;
using BuzzerEntities.Models;

namespace BuzzerDroid.Adapters
{
    internal class RoomsAdapter : RecyclerView.Adapter
    {
        public List<Room> Rooms { get; private set; } = new List<Room>();

        public override int ItemCount => Rooms.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            throw new NotImplementedException();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            throw new NotImplementedException();
        }
    }
}