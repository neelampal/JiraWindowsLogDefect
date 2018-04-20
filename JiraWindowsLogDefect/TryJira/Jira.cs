using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSXML2;

namespace TryJira
{
    class Jira
    {
        public String JiraUserName { get; set; }
        public String JiraPassword { get; set; }
        public String JiraUrl { get; set; }
        public String JiraJson { get; set; }
        public IEnumerable<string> filePaths { get; set; }
        private XMLHTTP60 JiraService = new XMLHTTP60();

        public String addJiraIssue()
        {
            JiraService.open("POST", JiraUrl + "rest/api/2/issue/");
            JiraService.setRequestHeader("Content-Type", "application/json");
            JiraService.setRequestHeader("X-Atlassian-Token", "no-check");
            JiraService.setRequestHeader("Accept", "application/json");
            JiraService.setRequestHeader("Authorization", "Basic " + GetEncodedCredentials());
            JiraService.send(JiraJson);
            String response = JiraService.responseText;
            JiraService.abort();
            return response;
        }

        private string GetEncodedCredentials()
        {
            string mergedCredentials = string.Format("{0}:{1}", JiraUserName, JiraPassword);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }
    }
}