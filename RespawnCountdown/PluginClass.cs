using Synapse.Api.Plugin;
using Synapse.Translation;
using System;

namespace cRespawnCountdown
{
    public class PluginTranslation : IPluginTranslation
    {
        public String RespawnIn { get; set; } = "Next respawn in";
        public String Seconds { get; set; } = " seconds";
        public String GetReady { get; set; } = "Get ready!";
    }

    [PluginInformation(
        Author = "Cubuzz",
        Description = "Respawn Countdown for spectators",
        LoadPriority = 0,
        Name = "Respawn Countdown",
        SynapseMajor = 2,
        SynapseMinor = 7,
        SynapsePatch = 1,
        Version = "2.0.1"
        )]

    public class PluginClass : AbstractPlugin
    {
        [Config(section = "RespawnCountdown")]
        public static PluginConfig Config;

        [SynapseTranslation]
        public static new SynapseTranslation<PluginTranslation> Translation { get; set; }

        public override void Load()
        {
            Translation.AddTranslation(new PluginTranslation());
            SynapseController.Server.Logger.Info("Geared up and ready to go! Respawn Countdown is now enabled.");

            new EventHandlers(Translation);
        }

        //This Method is only needed if you want to reload anything(Translation and Config will be reloaded by Synapse!)
        public override void ReloadConfigs()
        {
            // Disabled.
            // For now.
        }
    }
}
