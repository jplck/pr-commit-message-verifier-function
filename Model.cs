using AzureDevOpsHelperLib.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PRCommitStatusCheck
{
    public class PullRequestEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("resource")]
        public Resource Resource { get; set; }
    }

    public class Resource
    {
        [JsonProperty("pullRequestId")]
        public int PullRequestId { get; set; }

        [JsonProperty("commits")]
        public List<Commit> Commits { get; set;  }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("lastMergeSourceCommit")]
        public Commit LastMergeSourceCommit { get; set; }

        [JsonProperty("lastMergeTargetCommit")]
        public Commit LastMergeTargetCommit { get; set; }

        [JsonProperty("lastMergeCommit")]
        public Commit LastMergeCommit { get; set; }

        [JsonProperty("sourceRefName")]
        public string SourceRefName { get; set; }

        [JsonProperty("targetRefName")]
        public string TargetRefName { get; set; }

        [JsonProperty("supportsIterations")]
        public bool SupportsIterations { get; set; }
    }
}
