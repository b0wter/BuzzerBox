using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using BuzzerEntities.Models;
using Newtonsoft.Json;
using Android.Support.V7.Widget;
using BuzzerBoxDroid.Source.ViewAdapters;
using BuzzerBoxDroid.Source.DataProviders;

namespace BuzzerBoxDroid.Source.Fragments
{
    public class QuestionsViewFragment : Fragment
    {
        private const string ARG_SECTION_NUMBER = "section_number";
        private const string ARG_ITEMS_LIST = "items_list";

        private RecyclerView questionsRecyclerView;
        private QuestionsViewAdapter questionsViewAdapter;
        private RecyclerView.LayoutManager questionsLayoutManager;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        private void InitQuestionsRecyclerView(string serializedQuestions)
        {
            var questions = JsonConvert.DeserializeObject<List<Question>>(serializedQuestions);
            InitQuestionsRecyclerView(questions);
        }

        private void InitQuestionsRecyclerView(List<Question> questions)
        {
            questionsRecyclerView = View.FindViewById<RecyclerView>(Resource.Id.questions_fragment_recyclerview);
            questionsRecyclerView.HasFixedSize = false;

            questionsLayoutManager = new LinearLayoutManager(this.Context);
            questionsRecyclerView.SetLayoutManager(questionsLayoutManager);

            questionsViewAdapter = new QuestionsViewAdapter(questions);
            questionsViewAdapter.QuestionClicked += QuestionsViewAdapter_ItemClick;
            questionsViewAdapter.QuestionLongClicked += QuestionsViewAdapter_ItemLongClick;
            questionsViewAdapter.ResponseClicked += QuestionsViewAdapter_ResponseClicked;
            questionsViewAdapter.ResponseLongClicked += QuestionsViewAdapter_ResponseLongClicked;
            questionsRecyclerView.SetAdapter(questionsViewAdapter);
        }

        private void QuestionsViewAdapter_ResponseLongClicked(object sender, ResponsesViewAdapterClickEventArgs e)
        {
            Log.Debug("QuestionsViewFragment", $"Question #{e.QuestionPosition}, Response #{e.Position} long clicked.");
        }

        private void QuestionsViewAdapter_ResponseClicked(object sender, ResponsesViewAdapterClickEventArgs e)
        {
            Log.Debug("QuestionsViewFragment", $"Question #{e.QuestionPosition}, Response #{e.Position} clicked.");
        }

        private void QuestionsViewAdapter_ItemLongClick(object sender, BaseModelViewAdapterClickEventArgs e)
        {
            Log.Debug("QuestionsViewFragment", $"Question #{e.Position} long clicked");
        }

        private void QuestionsViewAdapter_ItemClick(object sender, BaseModelViewAdapterClickEventArgs e)
        {
            Log.Debug("QuestionsViewFragment", $"Question #{e.Position} clicked");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.questions_fragment, container, false);
        }

        public static QuestionsViewFragment NewInstance(int sectionNumber, List<Question> items)
        {
            var fragment = new QuestionsViewFragment();
            Bundle args = new Bundle();
            args.PutInt(ARG_SECTION_NUMBER, sectionNumber);
            args.PutString(ARG_ITEMS_LIST, JsonConvert.SerializeObject(items));
            fragment.Arguments = args;
            return fragment;
        }

        public override void OnStart()
        {
            base.OnStart();

            InitQuestionsRecyclerView(DebugDataProvider.Questions);
        }
    }
}