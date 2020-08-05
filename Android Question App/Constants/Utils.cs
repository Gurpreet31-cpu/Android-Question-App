using System.Net;

namespace Android_Question_App
{
    public class Utils
    {

        public string base_url = "https://www.reddit.com/subreddits/search.json?q=";

        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("https://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

    



    }

 
}