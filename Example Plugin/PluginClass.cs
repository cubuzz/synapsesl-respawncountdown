using Synapse.Api.Plugin;
using System.Collections.Generic;
using Synapse.Api;

namespace Example_Plugin
{
    [PluginInformation(
        Author = "Dimenzio",
        Description = "Example",
        LoadPriority = int.MinValue,
        Name = "ExamplePlugin",
        SynapseMajor = 2,
        SynapseMinor = 0,
        SynapsePatch = 0,
        Version = "2.0.0"
        )]
    public class PluginClass : AbstractPlugin
    {
        [Synapse.Api.Plugin.Config(section = "Example Plugin2")]
        public static Config Config;

        public override void Load()
        {
            SynapseController.Server.Logger.Info("Example Plugin Load");
            var dict = new Dictionary<string, string>()
            {
                {"translation1","Some Translation" }
            };
            Translation.CreateTranslations(dict);
            Logger.Get.Info(this.Translation.GetTranslation("translation1"));

            new EventHandlers();
        }

        //This Method is only needed if you want to reload anything(Translation and Config will be reloaded by Synapse!)
        public override void ReloadConfigs()
        {
            
        }
    }
}
