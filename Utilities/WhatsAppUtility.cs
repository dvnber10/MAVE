using RestSharp;

namespace MAVE.Utilities
{
    public class WhatsAppUtility
    {
        public async Task<int> SendMessage(string message, string number)
        {
            try
            {
                var url = "https://api.ultramsg.com/instance68430/messages/chat";
                var client = new RestClient(url);

                var request = new RestRequest(url, Method.Post);    
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("token", "joq7ej50otgx64sd");
                request.AddParameter("to", number);
                request.AddParameter("body", message);

                RestResponse response = await client.ExecuteAsync(request);
                var output = response.Content;
                Console.WriteLine(output);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
