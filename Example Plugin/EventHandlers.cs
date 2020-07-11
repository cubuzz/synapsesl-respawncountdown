using Synapse;
using Synapse.Api;
using Synapse.Events;
using Synapse.Events.Classes;
using UnityEngine;

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

    public class Example : Synapse.Plugin
    {
        public override void OnEnable()
        {
            //As Soon as the Player join and his Components can be added start the method LoadComponents
            Events.LoadComponentsEvent += LoadComponents;
        }

        private void LoadComponents(Synapse.Events.Classes.LoadComponentsEvent ev)
        {
            //Check if the Component somehow already exist
            if (ev.Player.GetComponent<ComponentExample>() == null)
                //If not add the Component to the player
                ev.Player.AddComponent<ComponentExample>();

            //Change The value for this One specific Player
            ev.Player.GetComponent<ComponentExample>().my_Variable_i_want_to_store = "Yea i can change it!";
            //Start this method for this One specific Player
            ev.Player.GetComponent<ComponentExample>().SayMyVariable();
        }

        public override string GetName => "YourPluginName";
    }

    public class ComponentExample : MonoBehaviour
    {
        //A Variable every Player gets when he joins
        public string my_Variable_i_want_to_store;

        // A Method which is called when the Components gets added
        public void Awake()
        {
            my_Variable_i_want_to_store = "Hello World";
            Log.Info($"My Awesome Component from Player : {this.gameObject.GetPlayer().NickName} was added :D");

            Events.ConsoleCommandEvent += OnCommand;
        }

        private void OnCommand(ConsoleCommandEvent ev)
        {
            throw new System.NotImplementedException();
        }

        //A Method which will activate when the Components get Removed (player leave)
        public void OnDestroy()
        {
            Log.Info($"My Awesome Component from Player : {this.gameObject.GetPlayer().NickName} was removed D:");
        }

        //A Method which gets the gameobject from this Component to find out the name and say something in the Console
        public void SayMyVariable()
        {
            Log.Info($"{this.gameObject.GetPlayer().NickName} want to say: {my_Variable_i_want_to_store}");
        }
    }
}
