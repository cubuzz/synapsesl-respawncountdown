using Synapse.Config;

namespace cRespawnCountdown
{
    public class PluginConfig : IConfigSection
    {
        public bool Enabled = true;
        public float PollInterval = 0.5f;
        public float FastPollInterval = 0.1f;
        public int SyncTickInterval = 30;
    }
}
