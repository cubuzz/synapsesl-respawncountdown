using Synapse.Api.Plugin;

namespace cRespawnCountdown
{
    [PluginInformation(
        Author = "Cubuzz",
        Description = "Respawn Countdown for spectators",
        LoadPriority = 0,
        Name = "Respawn Countdown",
        SynapseMajor = 2,
        SynapseMinor = 5,
        SynapsePatch = 3,
        Version = "2.0.0"
        )]
    public class PluginClass : AbstractPlugin
    {
        [Config(section = "RespawnCountdown")]
        public static PluginConfig Config;

        public override void Load()
        {
            SynapseController.Server.Logger.Info("Geared up and ready to go! Respawn Countdown is now enabled.");

            new EventHandlers();
        }

        //This Method is only needed if you want to reload anything(Translation and Config will be reloaded by Synapse!)
        public override void ReloadConfigs()
        {

        }
    }
}
