using Synapse.Events;

namespace Example_Plugin
{
    public class EventHandlers
    {
        private ExamplePlugin plugin;

        public EventHandlers(ExamplePlugin examplePlugin)
        {
            plugin = examplePlugin;

            Events.PlayerJoinEvent += OnPlayerJoin;
        }

        private void OnPlayerJoin(Synapse.Events.Classes.PlayerJoinEvent ev)
        {
            //Checks if the Plugin is enabled
            if (plugin.Enabled)
            {
                if (ev.Nick != string.Empty)
                {
                    //Add to the begining of the Name the Tag
                    ev.Nick = plugin.Tag + " " + ev.Nick;
                    //Broadcasts the Message to the Player
                    ev.Player.Broadcast(plugin.Duration,plugin.JoinMessage);
                }
                else
                {
                    ev.Player.Broadcast(plugin.Duration,plugin.EmptyException);
                }
            }
        }
    }
}
