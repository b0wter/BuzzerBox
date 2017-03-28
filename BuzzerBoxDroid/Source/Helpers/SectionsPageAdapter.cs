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
using Android.Support.V4.App;
using Java.Lang;
using BuzzerBoxDroid.Source.Fragments;
using BuzzerBoxDroid.Source.DataProviders;

namespace BuzzerBoxDroid.Source.Helpers
{
    public class SectionsPageAdapter : FragmentPagerAdapter
    {
        public SectionsPageAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            //
        }

        public override int Count => 3;

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            var fragment = QuestionsViewFragment.NewInstance(position + 1, DebugDataProvider.Questions);
            return fragment;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            switch (position)
            {
                case 0:
                    return new Java.Lang.String("Active");
                case 1:
                    return new Java.Lang.String("Game");
                case 2:
                    return new Java.Lang.String("History");
            }
            return null;
        }
    }
}