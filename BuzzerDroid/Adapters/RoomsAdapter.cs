using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using BuzzerEntities.Models;
using System.Collections.Generic;

namespace BuzzerDroid.Adapters
{
    class RoomsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<RoomsAdapterClickEventArgs> ItemClick;
        public event EventHandler<RoomsAdapterClickEventArgs> ItemLongClick;
        private List<Room> items;

        public RoomsAdapter(List<Room> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.viewholder_rooms;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);
            //var id = Resource.Layout.__YOUR_ITEM_HERE;
            //itemView = LayoutInflater.From(parent.Context).
            //       Inflate(id, parent, false);

            var vh = new RoomsAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as RoomsAdapterViewHolder;
            holder.TitleTextView.Text = items[position].Title;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => items.Count;

        void OnClick(RoomsAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(RoomsAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class RoomsAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView TitleTextView { get; set; }


        public RoomsAdapterViewHolder(View itemView, Action<RoomsAdapterClickEventArgs> clickListener, Action<RoomsAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            TitleTextView = itemView.FindViewById<TextView>(Resource.Id.viewholder_rooms_title_text);
            itemView.Click += (sender, e) => clickListener(new RoomsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RoomsAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class RoomsAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}