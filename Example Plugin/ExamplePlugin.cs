using Synapse;
using Synapse.Api.Plugin;
using System.Collections.Generic;

namespace Example_Plugin
{
    [PluginDetails(
        Author = "Dimenzio",
        Description = "An Example Plugin",
        Name = "ExamplePlugin",
        Version = "1.0",
        SynapseMajor = 1,
        SynapseMinor = 0
        )]
    public class ExamplePlugin : Plugin
    {
        public string JoinMessage;
        public string EmptyException;

        public bool Enabled;
        public string Tag;
        public ushort Duration;

        public EventHandlers EventHandlers;

        public override void OnEnable()
        {
            //CreateConfigs
            ReloadConfigs();
            this.ConfigReloadEvent += ReloadConfigs;

            //Hook Events
            EventHandlers = new EventHandlers(this);
        }

        public void ReloadConfigs()
        {
            Log.Info("Reloading Configs...");

            Enabled = Plugin.Config.GetBool("example_enabled",true);
            Tag = Plugin.Config.GetString("example_nametag", "[Example]");
            Duration = Plugin.Config.GetUShort("example_duration", 5);

            //CreateTranslation
            var translations = new Dictionary<string, string>()
            {
                {"join", "Your name was changed!"},
                { "empty", "The Config was empty so youre name wasnt changed"}
            };

            this.Translation.CreateTranslations(translations);

            JoinMessage = this.Translation.GetTranslation("join");
            EmptyException = this.Translation.GetTranslation("empty");
        }
    }
}
