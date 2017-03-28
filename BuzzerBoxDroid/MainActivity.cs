using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.AppCompat;
using BuzzerBoxDroid.Source.Helpers;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using System.Collections.Generic;
using BuzzerEntities.Models;
using Newtonsoft.Json;

namespace BuzzerBoxDroid
{
    [Activity(Label = "BuzzerDroid", Icon = "@mipmap/ic_launcher")]
    public class MainActivity : AppCompatActivity
    {
        private SectionsPageAdapter sectionsPagerAdapter;
        private ViewPager viewPager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            sectionsPagerAdapter = new SectionsPageAdapter(SupportFragmentManager);

            viewPager = FindViewById<ViewPager>(Resource.Id.container);
            viewPager.Adapter = sectionsPagerAdapter;

            var tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        private List<Room> CreateDummyRooms()
        {
            var rooms = JsonConvert.DeserializeObject<List<Room>>(DebugData.DummyRoomsString);
            return rooms;
        }
    }
}

