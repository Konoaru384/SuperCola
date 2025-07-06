using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Player = Exiled.API.Features.Player;
using PlayerHandler = Exiled.Events.Handlers.Player;

namespace SuperCola
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "SuperCola";
        public override string Prefix => "supercola";
        public override string Author => "Konoara";
        public override Version Version => new Version(1, 0, 0);

        private readonly Dictionary<Player, int> _counts
            = new Dictionary<Player, int>();

        public override void OnEnabled()
        {
            base.OnEnabled();
            PlayerHandler.UsedItem += OnUsedItem;
            PlayerHandler.Hurting += OnHurting;
            PlayerHandler.Died += OnDied;
        }

        public override void OnDisabled()
        {
            PlayerHandler.UsedItem -= OnUsedItem;
            PlayerHandler.Hurting -= OnHurting;
            PlayerHandler.Died -= OnDied;
            _counts.Clear();
            base.OnDisabled();
        }

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (ev.Item.Type != ItemType.SCP207)
                return;

            var ply = ev.Player;
            if (!_counts.ContainsKey(ply))
                _counts[ply] = 0;

            _counts[ply]++;

            if (_counts[ply] >= 4)
            {
                ply.DisableEffect(EffectType.Scp207);
                byte intensity = Config.MaxSpeedLevel;
                ply.EnableEffect(EffectType.Scp207, intensity);
            }
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.DamageHandler.Type == DamageType.Scp207
                && _counts.TryGetValue(ev.Player, out int c)
                && c >= 4)
            {
                ev.IsAllowed = false;
            }
        }

        private void OnDied(DiedEventArgs ev)
        {
            _counts.Remove(ev.Player);
        }
    }
}
