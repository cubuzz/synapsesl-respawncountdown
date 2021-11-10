using MEC;
using Synapse;
using Synapse.Command;
using Synapse.Translation;
using System;
using System.Collections.Generic;

namespace cRespawnCountdown
{

    [CommandInformation(
        Name = "rc.t",
        Aliases = new string[] {""},
        Description = "Cubuzz's respawn countdown.",
        Permission = "rc.t",
        Platforms = new[] {Platform.RemoteAdmin, Platform.ServerConsole},
        Usage = "Toggles RespawnCountdown on or off."
        )]
    
    class ToggleCommand : ISynapseCommand
    {
        public CommandResult Execute(CommandContext ctx)
        {
            var result = new CommandResult();

            // Toggle or Set?
            if (ctx.Arguments.Count > 0)
            {
                if (ctx.Arguments.FirstElement() == "true" || ctx.Arguments.FirstElement() == "on")
                {
                    PluginClass.Config.Enabled = true;
                    result.Message = "Countdown toggled ON.";
                    result.State = CommandResultState.Ok;
                }
                else if (ctx.Arguments.FirstElement() == "false" || ctx.Arguments.FirstElement() == "off")
                {
                    PluginClass.Config.Enabled = false;
                    result.Message = "Countdown toggled OFF.";
                    result.State = CommandResultState.Ok;
                }
                else
                {
                    result.Message = "I don't know this argument!";
                    result.State = CommandResultState.Error;
                }
            }
            else
            {
                PluginClass.Config.Enabled = !PluginClass.Config.Enabled;
                result.Message = $"Countdown toggled {PluginClass.Config.Enabled}";
                result.State = CommandResultState.Ok;
            }
            return result;
        }
    }

    public class EventHandlers
    {
        float nextRespawnDue = 0;
        List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        List<Synapse.Api.Player> DeadPlayers = new List<Synapse.Api.Player>();
        bool isRespawning = false;
        
        private SynapseTranslation<PluginTranslation> Translation;

        public EventHandlers(SynapseTranslation<PluginTranslation> ActiveTranslation)
        {
            Translation = ActiveTranslation;
            //You can also use SynapseController.Server to get the Server!
            Server.Get.Events.Round.WaitingForPlayersEvent += CleanUp;
            Server.Get.Events.Round.RoundStartEvent += OnStart;
            Server.Get.Events.Round.RoundRestartEvent += OnEnd;
            Server.Get.Events.Player.PlayerDeathEvent += YouDied;
            Server.Get.Events.Player.PlayerJoinEvent += DeadButJoined;
            Server.Get.Events.Round.TeamRespawnEvent += Revived;
            Server.Get.Events.Player.PlayerLeaveEvent += Disconnect;
        }

        private void Disconnect(Synapse.Api.Events.SynapseEventArguments.PlayerLeaveEventArgs args)
        {
            DeadPlayers.Remove(args.Player);
        }

        private void CleanUp()
        {
            Timing.KillCoroutines(Coroutines.ToArray());
            DeadPlayers.Clear();
        }
        private void OnStart() {
            Coroutines.Add(Timing.RunCoroutine(RespawnCountdown()));
            Coroutines.Add(Timing.RunCoroutine(Reindex()));
        }

        private void OnEnd() {
            Server.Get.Logger.Warn("[RC] Requested killing all coroutines, and it's likely not going to work. Trying anyway :)");
            Timing.KillCoroutines(Coroutines.ToArray());
        }

        private void YouDied(Synapse.Api.Events.SynapseEventArguments.PlayerDeathEventArgs args) => DeadPlayers.Add(args.Victim);
        private void DeadButJoined(Synapse.Api.Events.SynapseEventArguments.PlayerJoinEventArgs args) => DeadPlayers.Add(args.Player);

        private void Revived(Synapse.Api.Events.SynapseEventArguments.TeamRespawnEventArgs args)
        {
            foreach(Synapse.Api.Player player in args.Players)
            {
                DeadPlayers.Remove(player);
            }
            // Server.Get.Logger.Info($"Now dead: {DeadPlayers.Count}");
        }

        private void reindex()
        {
            DeadPlayers.Clear();
            foreach (Synapse.Api.Player player in Server.Get.Players)
            {
                if(player.RoleType == RoleType.Spectator)
                {
                    DeadPlayers.Add(player);
                }
            }
        }

        private IEnumerator<float> Reindex()
        {
            while (PluginClass.Config.Enabled)
            {
                reindex();
                yield return Timing.WaitForSeconds(15f);
            }
        }
        
        private IEnumerator<float> RespawnCountdown()
        {
            Server.Get.Logger.Info("[RC] Registered coroutine. We're ready to roll.");
            while (PluginClass.Config.Enabled)
            {
                if (nextRespawnDue < 0)
                {
                    nextRespawnDue = Server.Get.Map.Round.NextRespawn;
                    // Server.Get.Logger.Warn(nextRespawnDue);
                    // Is the next respawn in less than 20 seconds?
                    isRespawning = (20 > nextRespawnDue);
                }

                // Deduct Delta from nextRespawnDue
                if (nextRespawnDue < 20){nextRespawnDue -= PluginClass.Config.FastPollInterval;} 
                else{nextRespawnDue = Server.Get.Map.Round.NextRespawn; }

                // Iterate over players, give dead players the broadcast
                // OUTSOURCED: See YouDied and Revived

                float respawnIn = (float) Math.Round(nextRespawnDue, 1);
                try
                {
                    foreach (Synapse.Api.Player player in DeadPlayers)
                    {

                        if (isRespawning)
                        {
                            player.SendBroadcast(1, $"<color=yellow><size=28><i>{Translation.ActiveTranslation.GetReady}\n{(respawnIn).ToString("##0.0")}</i></size></color>", true);
                        }
                        else
                        {
                            player.SendBroadcast(1, $"<size=30><i>{Translation.ActiveTranslation.RespawnIn}: <b><color=#df2222>{(respawnIn + 12).ToString("##0")} {Translation.ActiveTranslation.Seconds}</color></b>.</i></size>\n<size=30><i><b><color=blue>NTF:</color> {Server.Get.Map.Round.MtfTickets}</b></i><color=#666666> | </color><i><b><color=green>CI:</color> {Server.Get.Map.Round.ChaosTickets}</b></i></size>", true);
                        }
                    
                    }
                }
                catch (Exception e)
                {
                    Server.Get.Logger.Error(e.ToString());
                    reindex();
                }
                // player.GiveTextHint($"Nächster Respawn in {respawnIn} Sekunden...", 1f);

                // Server.Get.Logger.Info($"{nextSyncTick}; {nextRespawnDue}");
                if (nextRespawnDue < 20)
                { yield return Timing.WaitForSeconds(PluginClass.Config.FastPollInterval); } 
                else { yield return Timing.WaitForSeconds(PluginClass.Config.PollInterval); }
            }
        }
    }
}
