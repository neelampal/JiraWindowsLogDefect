using System;
using System.IO;
using System.Net;
using RestSharp;
namespace TryJira
{
    class Working
    {
        static void Main(string[] args)
        {
            String description = "Issue Description";
            String summary = "Issue Summary";
            String project = "TPFR";
            String errorScreenshotPath = @"E:\AmazonDemo\AmazonDemo\Results\Screenshots\searchProduct.png";

            String defectID = LogDefect(summary, description, project, errorScreenshotPath);

            Console.Write(defectID);

            Console.WriteLine("wait");

            String LogDefect(String defectSummary, String defectDescription, String projectName, String errorScreenshot)
            {
                var client = new RestClient("https://spotqa.atlassian.net/rest/api/2");
                var request = new RestRequest("issue/", Method.POST);

                client.Authenticator = new HttpBasicAuthenticator("neelam@spotqa.com", "neelam123");

                var issue = new Issue
                {
                    fields =
                        new Fields
                        {
                            description = defectDescription,// "Issue Description",
                            summary = defectSummary,// "Issue Summary",
                            project = new Project { key = projectName },
                            issuetype = new IssueType { name = "Bug" }
                        }
                };

                request.AddJsonBody(issue);

                var res = client.Execute<Issue>(request);

                if (res.StatusCode == HttpStatusCode.Created)
                {
                    Console.WriteLine("Issue: {0} successfully created", res.Data.key);

                    #region Attachment            
                    request = new RestRequest(string.Format("issue/{0}/attachments", res.Data.key), Method.POST);

                    request.AddHeader("X-Atlassian-Token", "nocheck");

                    var file = File.ReadAllBytes(errorScreenshot);

                    request.AddHeader("Content-Type", "multipart/form-data");
                    request.AddFile("file", file, "error_screenshot.jpg", "application/octet-stream");

                    var res2 = client.Execute(request);

                    Console.WriteLine(res2.StatusCode == HttpStatusCode.OK ? "Attachment added!" : res2.Content);
                    #endregion
                }
                else
                    Console.WriteLine(res.Content);

                return res.Content;
            }
        }

        public class Issue
        {
            public string id { get; set; }
            public string key { get; set; }
            public Fields fields { get; set; }
        }

        public class Fields
        {
            public Project project { get; set; }
            public IssueType issuetype { get; set; }
            public string summary { get; set; }
            public string description { get; set; }
        }

        public class Project
        {
            public string id { get; set; }
            public string key { get; set; }
        }

        public class IssueType
        {
            public string id { get; set; }
            public string name { get; set; }
        }

    }
    }
