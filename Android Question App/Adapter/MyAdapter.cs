using Android.Support.V7.Widget;
using Android.Widget;
using Android.Views;
using System.Collections;


namespace Android_Question_App
{
    public class MyAdapter : RecyclerView.Adapter
    {
  
        ArrayList items;
        AdapterCallBack clickListener;
      
        public MyAdapter(ArrayList items, LoginActivity loginActivity, RecyclerView mRecyclerView, AdapterCallBack clickListener)
        { 
         
            this.items = items;
            this.clickListener = clickListener;
         
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
          View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_view, parent, false);
            var vh = new MyViewHolder(itemView);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as MyViewHolder;
            holder.txt.Text = (string)items[position];
           holder.txt.Click += (sender, e) =>
            {
               
                var subredditName = (string)items[position];                            
                clickListener.onMethodCallbackAsync(subredditName);                // shows the sidebar of a subreddit in the list, when clicked

            };
        }
   

        public override int ItemCount
        {
            get
            {
                return items.Count;
            }
        }

        public class MyViewHolder : RecyclerView.ViewHolder
        {
            public TextView txt;
            public MyViewHolder(View itemView) : base(itemView)
            {
                txt = itemView.FindViewById<TextView>(Resource.Id.nameTxt);

            }
        }
    }
}