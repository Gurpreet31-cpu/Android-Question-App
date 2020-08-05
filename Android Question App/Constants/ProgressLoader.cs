

using Android.App;
using Android.Content;


namespace Android_Question_App
{
    public class ProgressLoader
    {
        ProgressDialog progress;
        public ProgressLoader(Context context)
        {
            progress = new ProgressDialog(context);
            progress.Indeterminate = true;

            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading... Please wait...");
            progress.SetCancelable(false);
        }

        public void showProgressBar()
        {
            progress.Show();
        }

        public void hideProgressBar()
        {
            progress.Hide();
        }
    }
}