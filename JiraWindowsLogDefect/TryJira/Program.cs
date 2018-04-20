using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TryJira
{
    class Program
    {
        string[] args;
        public void JiraWorking()
        //static void Main(string[] args)
        {
            string connectionUrl = null, username = null, password = null, project = null;

            args = new string[] { "/url:https://spotqa.atlassian.net/", "/username:neelam@spotqa.com", "/password:neelam123", "/project:TPFR" };

            if (args.Length == 0)
            {
                Console.WriteLine("Runs on a Jira account instance to create a new bug.");
                Console.WriteLine("");
            }

            try
            {
                CheckArguments(args, out connectionUrl, out username, out password, out project);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Jira objJira = new Jira();
            objJira.JiraUrl = connectionUrl;
            String DefectSummary = "Issue logged from C#";
            String StepsToReproduce = "Creation of an issue using project keys and issue type names using the REST API";
            String IssueType = "Bug";
            String attachment = @"E:\AmazonDemo\AmazonDemo\Results\Screenshots\searchProduct.png";
            //,\"Attachment\":\"" + attachment + "\",
            objJira.JiraJson = @"{" + "\"fields\":{\"project\":{\"key\":\"" + project + "\"},\"summary\":\""+DefectSummary+"\",\"description\":\""+StepsToReproduce+ "\",\"issuetype\":{\"name\":\"" + IssueType+"\"}}}";
            objJira.JiraUserName = username;
            objJira.JiraPassword = password;
            //File aa = new File(attachment); ;
            //objJira.filePaths = aa;
            objJira.filePaths = new List<string>() { attachment };
            var defect = objJira.addJiraIssue();
            //defect.Data.key;
            
            
            String[] allVal = defect.Split(',');
            String[] defectKey = allVal[1].Split(':');
            String defectNbr = defectKey[1];
            //return defectNbr;
            Console.WriteLine(defectNbr);
            //Console.ReadKey();
        }

        private static void CheckArguments(string[] args, out string connectionUrl, out string username, out string password, out string project)
        {
            connectionUrl = null;
            username = null;
            password = null;
            project = null;

            Dictionary<string, string> argsMap = new Dictionary<string, string>();
            foreach (var arg in args)
            {
                if (arg[0] == '/' && arg.IndexOf(':') > 1)
                {
                    string key = arg.Substring(1, arg.IndexOf(':') - 1);
                    string value = arg.Substring(arg.IndexOf(':') + 1);

                    switch (key)
                    {
                        case "url":
                            connectionUrl = value;
                            break;

                        case "username":
                            username = value;
                            break;

                        case "password":
                            password = value;
                            break;

                        case "project":
                            project = value;
                            break;
                        default:
                            throw new ArgumentException("Unknown argument", key);
                    }
                }
            }

            if (connectionUrl == null || username == null || password == null || project == null)
                throw new ArgumentException("Missing required arguments");

        }
    }
}