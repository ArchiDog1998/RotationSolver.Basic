using ECommons.ExcelServices;
using RotationSolver.Basic.Configuration.Timeline;

namespace RotationSolver.Basic.Configuration;

internal class TerritoryConfig
{
    [JsonProperty]
    private Dictionary<Job, TerritoryConfigItem> _jobConfigs { get; set; } = [];

    [JsonIgnore]
    public TerritoryConfigItem JobConfig
    {
        get
        {
            var job = DataCenter.Job;
            if (_jobConfigs.TryGetValue(job, out var config)) return config;

            return _jobConfigs[job] = new ();
        }
    }

    public TerritoryConfigItem Config { get; set; } = new();

    [JsonIgnore]
    public Dictionary<float, List<BaseTimelineItem>> Timeline
    {
        get
        {
            var timeline1 = Config.Timeline;
            var timeline2 = JobConfig.Timeline;

            Dictionary<float, List<BaseTimelineItem>> result = new(Math.Max(timeline1.Count, timeline2.Count));

            foreach(var pair in timeline1)
            {
                result[pair.Key] = new(pair.Value);
            }

            foreach (var pair in timeline2)
            {
                if(result.TryGetValue(pair.Key, out var list))
                {
                    list.AddRange(pair.Value);
                }
                else
                {
                    result[pair.Key] = new(pair.Value);
                }
            }

            return result;
        }
    }
}

internal class TerritoryConfigItem
{
    public Dictionary<float, List<BaseTimelineItem>> Timeline { get; set; } = [];

    //TODO: map effects.
}
