using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using BuzzerEntities.Models;

namespace BuzzerDroid
{
    [Activity(Label = "BuzzerDroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private RecyclerView roomsRecyclerView;
        private RecyclerView.LayoutManager roomsRecyclerViewLayoutManager;
        private RoomsAdapter roomsAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);





            // Set our view from the "main" layout resource
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_roomsActivity, menu);
            return base.OnPrepareOptionsMenu(menu);
        }
    }
}

