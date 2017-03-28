using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using BuzzerEntities.Models;
using System.Linq;
using Android.Content;

namespace BuzzerBoxDroid.Source.ViewAdapters
{
    class QuestionsViewAdapter : RecyclerView.Adapter 
    {
        public event EventHandler<BaseModelViewAdapterClickEventArgs> QuestionClicked;
        public event EventHandler<BaseModelViewAdapterClickEventArgs> QuestionLongClicked;
        public event EventHandler<ResponsesViewAdapterClickEventArgs> ResponseClicked;
        public event EventHandler<ResponsesViewAdapterClickEventArgs> ResponseLongClicked;

        public List<Question> Items { get; private set; }

        public QuestionsViewAdapter(List<Question> data)
        {
            Items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.viewholder_question;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new QuestionsViewAdapterViewHolder(itemView, OnClick, OnLongClick, parent.Context);
            vh.ResponseClicked += viewHolder_ResponseClicked;
            vh.ResponseLongClicked += viewHolder_ResponseLongClicked;
            return vh;
        }

        private void viewHolder_ResponseLongClicked(object sender, ResponsesViewAdapterClickEventArgs e)
        {
            ResponseClicked?.Invoke(sender, e);
        }

        private void viewHolder_ResponseClicked(object sender, ResponsesViewAdapterClickEventArgs e)
        {
            ResponseLongClicked?.Invoke(sender, e);
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            // Replace the contents of the view with that element
            var item = Items[position];
            var holder = viewHolder as QuestionsViewAdapterViewHolder;

            holder.TitleTextView.Text = item.Title;

            if(item.Responses != null)
                holder.VoteTextView.Text = $"{(item.Responses.Sum(r => r.Votes.Count))} Votes";

            if(item.Room != null)
                holder.RoomTextView.Text = item.Room.Title;

            holder.ResponsesViewAdapter = new ResponsesViewAdapter(item.Responses, position, item.AllowMultipleVotes);
        }

        public override int ItemCount => Items.Count;

        void OnClick(BaseModelViewAdapterClickEventArgs args) => QuestionClicked?.Invoke(this, args);
        void OnLongClick(BaseModelViewAdapterClickEventArgs args) => QuestionLongClicked?.Invoke(this, args);
    }

    public class QuestionsViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        private Context Context { get; set; }
        public List<Response> Responses {get;set;} 
        public TextView TitleTextView { get; set; }
        public TextView VoteTextView { get; set; }
        public TextView RoomTextView { get; set; }
        private RecyclerView ResponsesView { get; set; }

        private ResponsesViewAdapter responsesViewAdapter;
        public ResponsesViewAdapter ResponsesViewAdapter
        {
            get
            {
                return responsesViewAdapter;
            }
            set
            {
                UnsetEventListeners();
                responsesViewAdapter = value;
                ResponsesView.SetAdapter(responsesViewAdapter);
                SetEventListeners();
            }
        }

        public event EventHandler<ResponsesViewAdapterClickEventArgs> ResponseClicked;
        public event EventHandler<ResponsesViewAdapterClickEventArgs> ResponseLongClicked;

        private void SetEventListeners()
        {
            if(responsesViewAdapter != null)
            {
                responsesViewAdapter.ItemClick += ResponsesViewAdapter_ItemClick;
                responsesViewAdapter.ItemLongClick += ResponsesViewAdapter_ItemLongClick;
            }
        }

        private void UnsetEventListeners()
        {
            if (responsesViewAdapter != null)
            {
                responsesViewAdapter.ItemClick -= ResponsesViewAdapter_ItemClick;
                responsesViewAdapter.ItemLongClick -= ResponsesViewAdapter_ItemLongClick;
            }
        }

        private void ResponsesViewAdapter_ItemLongClick(object sender, ResponsesViewAdapterClickEventArgs e)
        {
            ResponseLongClicked?.Invoke(sender, e);
        }

        private void ResponsesViewAdapter_ItemClick(object sender, ResponsesViewAdapterClickEventArgs e)
        {
            ResponseClicked?.Invoke(sender, e);
        }

        private LinearLayoutManager responseLayoutManager;

        public QuestionsViewAdapterViewHolder(View itemView, Action<BaseModelViewAdapterClickEventArgs> clickListener, Action<BaseModelViewAdapterClickEventArgs> longClickListener, Context context) : base(itemView)
        {
            this.Context = context;
            InitControlReferences(itemView);
            InitRecyclerView();
            CreateControlHandlers(itemView, clickListener, longClickListener);
        }

        private void InitControlReferences(View itemView)
        {
            TitleTextView = itemView.FindViewById<TextView>(Resource.Id.viewholder_questions_title_text);
            VoteTextView = itemView.FindViewById<TextView>(Resource.Id.viewholder_questions_votes_text);
            RoomTextView = itemView.FindViewById<TextView>(Resource.Id.viewholder_questions_room_text);
            ResponsesView = itemView.FindViewById<RecyclerView>(Resource.Id.viewholder_questions_response_recyclerview);
        }

        private void InitRecyclerView()
        {
            if (Context == null)
                throw new InvalidOperationException("You have to set a context for the QuestionsViewAdapterViewHolder to work properly.");

            responseLayoutManager = new LinearLayoutManager(Context);
            ResponsesView.SetLayoutManager(responseLayoutManager);

            // The adapter is set externally because the responses are not available at this time.
        }

        private void CreateControlHandlers(View itemView, Action<BaseModelViewAdapterClickEventArgs> clickListener, Action<BaseModelViewAdapterClickEventArgs> longClickListener)
        {
            itemView.Click += (sender, e) => clickListener(new BaseModelViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new BaseModelViewAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class BaseModelViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}