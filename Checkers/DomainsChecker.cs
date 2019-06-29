﻿using RestSharp;
using System.IO;

namespace FB.BanChecker
{
    public class DomainsChecker
    {
        public static void Check(string apiAddress, string accessToken)
        {
            var restClient = new RestClient(apiAddress);
            var domains = File.ReadAllLines("Domains.txt");
            //Проверка доменов на забаненность
            foreach (var d in domains)
            {
                var request = new RestRequest("", Method.POST);
                request.AddParameter("access_token", accessToken);
                request.AddParameter("scrape", "true");
                request.AddParameter("id", d);
                var response = restClient.Execute(request);
                if (response.Content.Contains("disallowed")) //сайт забанен!
                {
                    var msg = $"Domain {d} was banned on Facebook!";
                    Logger.Log(msg);
                    new Mailer().SendEmailNotification(msg, "Subj!");
                }
                else
                {
                    Logger.Log($"Domain {d} is not banned.");
                }
            }
        }
    }
}