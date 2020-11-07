using Synapse;

namespace Example_Plugin
{
    public class EventHandlers
    {
        public EventHandlers()
        {
            //You can also use SynapseController.Server to get the Server!
            Server.Get.Events.Player.PlayerJoinEvent += OnJoin;
            Server.Get.Events.Round.RoundStartEvent += OnStart;
        }

        private void OnStart()
        {
            foreach(var player in Server.Get.Players)
            {
                player.GiveTextHint("The Round started!");
            }
        }

        private void OnJoin(Synapse.Api.Events.SynapseEventArguments.PlayerJoinEventArgs ev)
        {
            ev.Player.SendConsoleMessage(PluginClass.Config.consoleMessage);
        }
    }
}
