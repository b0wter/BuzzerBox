using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.OS;
using Android.Util;
using BuzzerBoxDroid.Source.Models;
using BuzzerEntities.Models;

namespace BuzzerBoxDroid.Source.ViewAdapters
{
    public class ResponsesViewAdapter : RecyclerView.Adapter
    {
        private static string TAG = "ResponsesViewAdapter";
        public event EventHandler<ResponsesViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<ResponsesViewAdapterClickEventArgs> ItemLongClick;
        public List<Response> Items { get; private set; }
        private int questionPosition;
        private bool allowsMultipleVotes;

        public ResponsesViewAdapter(List<BuzzerEntities.Models.Response> data, int questionPosition, bool allowsMultipleVotes)
        {
            this.allowsMultipleVotes = allowsMultipleVotes;
            this.questionPosition = questionPosition;
            Items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Log.Debug(TAG, "OnCreateViewHolder");
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.viewholder_response;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);
            var vh = new ResponsesViewAdapterViewHolder(itemView, OnClick, OnLongClick, questionPosition);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            Log.Debug(TAG, "OnBindViewHolder");
            var item = Items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ResponsesViewAdapterViewHolder;

            if (allowsMultipleVotes)
            {
                holder.CheckBox.Text = item.Title;
                holder.CheckBox.Visibility = ViewStates.Visible;
                holder.RadioButton.Visibility = ViewStates.Gone;

            }
            else
            {
                holder.RadioButton.Text = item.Title;
                holder.CheckBox.Visibility = ViewStates.Gone;
                holder.RadioButton.Visibility = ViewStates.Visible;
            }
        }

        public override int ItemCount => Items.Count;

        void OnClick(ResponsesViewAdapterClickEventArgs args) => HandleItemClickEvent(args);
        void OnLongClick(ResponsesViewAdapterClickEventArgs args) => HandleItemLongClickEvent(args);

        private void HandleItemClickEvent(ResponsesViewAdapterClickEventArgs args)
        {
            if (allowsMultipleVotes)
            {
                // Checkboxes
            }
            else
            {
                // Radiobuttons
                // Items.ForEach(item => item.IsLocalUserVote)
            }

            ItemClick?.Invoke(this, args);
        }

        private void HandleItemLongClickEvent(ResponsesViewAdapterClickEventArgs args)
        {
            ItemLongClick?.Invoke(this, args);
        }

    }

    public class ResponsesViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TitleTextView { get; set; }
        public RadioButton RadioButton { get; set; }
        public CheckBox CheckBox { get; set; }
        private int questionPosition;

        public ResponsesViewAdapterViewHolder(View itemView, Action<ResponsesViewAdapterClickEventArgs> clickListener, Action<ResponsesViewAdapterClickEventArgs> longClickListener, int questionPosition) : base(itemView)
        {
            this.questionPosition = questionPosition;
            //TitleTextView = itemView.FindViewById<TextView>(Resource.Id.viewholder_respons_title_text);
            RadioButton = itemView.FindViewById<RadioButton>(Resource.Id.viewholder_response_radiobutton);
            CheckBox = itemView.FindViewById<CheckBox>(Resource.Id.viewholder_response_checkbox);
            itemView.Click += (sender, e) => clickListener(new ResponsesViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition, QuestionPosition = questionPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ResponsesViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition, QuestionPosition = questionPosition });
        }
    }

    public class ResponsesViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public int QuestionPosition { get; set; }
    }
}