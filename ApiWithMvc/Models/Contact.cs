using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace ApiWithMvc.Models
{
    public class Contact
    {
        public string Sid { get; set; }
        public string Phone_Number { get; set; }
        public string Friendly_Name { get; set; }

        public string GetValidation()
        {
            var client = new RestClient("https://api.twilio.com/2010-04-01");
            var request = new RestRequest($"Accounts/{EnvironmentVariables.AccountSID}/OutgoingCallerIds.json", Method.POST);
            client.Authenticator = new HttpBasicAuthenticator(EnvironmentVariables.AccountSID, EnvironmentVariables.AuthToken);
            request.AddParameter("PhoneNumber", Phone_Number);
            request.AddParameter("FriendlyName", Friendly_Name);
            var response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
			JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            string result = JsonConvert.DeserializeObject<string>(jsonResponse["validation_code"].ToString());
            return result;
        }

        public static List<Contact> GetContacts()
        {
            var client = new RestClient("https://api.twilio.com/2010-04-01");
            var request = new RestRequest($"Accounts/{EnvironmentVariables.AccountSID}/OutgoingCallerIds.json", Method.GET);
            client.Authenticator = new HttpBasicAuthenticator(EnvironmentVariables.AccountSID, EnvironmentVariables.AuthToken);
            var response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            List<Contact> contactList = JsonConvert.DeserializeObject<List<Contact>>(jsonResponse["outgoing_caller_ids"].ToString());
            return contactList;
        }

        private static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
