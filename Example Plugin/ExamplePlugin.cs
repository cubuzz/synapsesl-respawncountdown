using MEC;
using Synapse.Api;
using Synapse.Api.Plugin;
using System.Collections.Generic;

namespace Example_Plugin
{
    [PluginInformations(
        Author = "Dimenzio",
        Description = "Example",
        LoadPriority = int.MaxValue,
        Name = "ExamplePlugin",
        SynapseMajor = 2,
        SynapseMinor = 0,
        SynapsePatch = 0,
        Version = "2.0.0"
        )]
    public class ExamplePlugin
    {
        public ExamplePlugin(PluginExtension pe)
        {
            SynapseController.Server.Logger.Info("ExamplePlugin!");

            SynapseController.Server.Events.Player.LoadComponentsEvent += LoadComp;
        }

        private void LoadComp(Synapse.Api.Events.SynapseEventArguments.LoadComponentEventArgs ev)
        {
            if (ev.Player == null)

            SynapseController.Server.Logger.Info("load");

            foreach (var tesla in SynapseController.Server.Map.Elevators)
                SynapseController.Server.Logger.Info(tesla.GameObject.name);

            foreach (var tesla in SynapseController.Server.Map.Doors)
                SynapseController.Server.Logger.Info(tesla.GameObject.name);

            foreach (var tesla in SynapseController.Server.Map.Teslas)
                SynapseController.Server.Logger.Info(tesla.GameObject.name);

            foreach (var room in SynapseController.Server.Map.Rooms)
                SynapseController.Server.Logger.Info(room.GameObject.name + ": " + room.Position + ":   "+ room.Zone + ": " + room.RoomType);

            //Timing.RunCoroutine(Show(ev.Player.GetPlayer()));
        }

        private IEnumerator<float> Show(Player player)
        {
            yield return Timing.WaitForSeconds(30f);
            for(; ; )
            {
                yield return Timing.WaitForSeconds(5f);
                if (player.LookingAt != null)
                    SynapseController.Server.Logger.Info(player.LookingAt.name);
            }
        }
    }
}
