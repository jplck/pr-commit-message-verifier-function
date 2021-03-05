using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using AzureDevOpsHelperLib;
using AzureDevOpsHelperLib.Model;

namespace PRCommitStatusCheck
{

    public sealed class PullRequestVerificationStatus
    {
        public static readonly string Success = "succeeded";
        public static readonly string Failed = "failed";
        public static readonly string Pending = "pending";
    }

    public static class PRTriggerFunc
    {
        [FunctionName("PRTriggerFunc")]
        public static async Task Run(
            [ServiceBusTrigger("prevents", Connection = "ServiceBusConnection")] string eventData,
            ILogger log)
        {
            try
            {
                var pullRequest = JsonConvert.DeserializeObject<PullRequestEvent>(eventData);
                var resource = pullRequest.Resource;
                var prId = resource.PullRequestId;
                var repositoryName = resource.Repository.Name;
                var projectName = resource.Repository.Project.Name;

                //Need to see if there is a better way fetching the org.
                var orgName = pullRequest.Resource.Repository.RemoteUrl.PathAndQuery.Split("/")[1];

                var gitHelper = new GitHelper(GenericHelper.GetConfig()["PAT"], orgName, projectName);

                var prCommits = await gitHelper.FetchCommitsFromPullRequestAsync(repositoryName, prId);

                var pendingStatusUpdate = GenerateEventContent(PullRequestVerificationStatus.Pending,
                                                              "Waiting for commit message verification.");

                await gitHelper.PostStatusOnPullRequestAsync(resource.SupportsIterations, repositoryName, prId, pendingStatusUpdate);
                
                //Delayed for demoing purposes :)
                await Task.Delay(5000);
                
                var evalContentResult = await EvaluateCommitMessagesAsync(gitHelper, resource, prCommits);

                await gitHelper.PostStatusOnPullRequestAsync(resource.SupportsIterations, repositoryName, prId, evalContentResult);

            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                throw;
            }

        }

        private static async Task<string> EvaluateCommitMessagesAsync(GitHelper helper, Resource resource, List<Commit> prCommits)
        {
            bool evalResult = false;

            var commits = await helper.GetCommitsWithWorkItemReferencesByIdsAsync(resource.Repository.Name, 
                                                                               resource.SourceRefName, 
                                                                               resource.LastMergeSourceCommit.commitId, 
                                                                               prCommits.Count);

            if (commits.Count > 0) {
                foreach (var commit in commits)
                {
                    evalResult = commit.WorkItems.Count > 0;
                    if (!evalResult) { break; }
                }
            }

            var state = evalResult ? PullRequestVerificationStatus.Success : PullRequestVerificationStatus.Failed;
            var message = evalResult ? "Proved that all commits contained in PR contain at least one work item link." :
                                       "Commit verification failed. Not all commits contain linked work items.";

            return GenerateEventContent(state, message);

        }

        private static string GenerateEventContent(string status, string message)
        {
            return JsonConvert.SerializeObject(
                new
                {
                    State = status,
                    Description = message,

                    Context = new
                    {
                        Name = "commit-verification",
                        Genre = "verifiers"
                    }
                }
            );
        }
    }

}
