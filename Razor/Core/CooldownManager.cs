#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Assistant.Gumps.Internal;

namespace Assistant.Core
{
    public static class CooldownManager
    {
        private static Timer CooldownTimer { get; set; }
        public static Dictionary<string, Cooldown> Cooldowns { get; private set; }

        private static CooldownGump _gump;

        public static void Initialize()
        {
            CooldownTimer = new InternalTimer();
            Cooldowns = new Dictionary<string, Cooldown>();
        }

        public static bool Running => CooldownTimer.Running;

        public static void Start()
        {
            if (CooldownTimer.Running)
            {
                CooldownTimer.Stop();
            }

            CooldownTimer.Start();
        }

        public static void Stop()
        {
            if (!CooldownTimer.Running)
                return;

            _gump.CloseGump();
            Cooldowns.Clear();

            CooldownTimer.Stop();
        }

        private class InternalTimer : Timer
        {
            public InternalTimer() : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            {
            }

            protected override void OnTick()
            {
                if (World.Player == null)
                {
                    CooldownTimer.Stop();
                    return;
                }

                _gump?.CloseGump();
                
                RemoveExpired();
                
                if (Cooldowns.Count > 0)
                {
                    _gump = new CooldownGump();
                    _gump.SendGump();    
                }
            }
        }

        public static void AddCooldown(Cooldown cooldown)
        {
            Cooldowns[cooldown.Name] = cooldown;

            if (!CooldownTimer.Running)
            {
                Start();
            }
        }

        public static void AddCooldown(string name, int seconds, int hue = 0, int icon = 0, int sound = 0, bool stayVisible = false)
        {
            Cooldowns[name] = new Cooldown
            {
                Name = name,
                Seconds = seconds,
                EndTime = DateTime.UtcNow.AddSeconds(seconds),
                Hue = hue,
                Icon = icon,
                SoundId = sound,
                StayVisible = stayVisible
            };

            if (!CooldownTimer.Running)
            {
                Start();
            }
        }

        public static void RemoveExpired()
        {
            foreach (KeyValuePair<string, Cooldown> cooldown in Cooldowns.ToList())
            {
                if (cooldown.Value.StayVisible)
                {
                    continue;
                }
                
                TimeSpan diff = cooldown.Value.EndTime - DateTime.UtcNow;
                int timeLeft = (int)diff.TotalSeconds;

                if (timeLeft <= 0)
                {
                    if (cooldown.Value.SoundId > 0)
                    {
                        Client.Instance.SendToClient(new PlaySound(cooldown.Value.SoundId));
                    }
                    
                    Cooldowns.Remove(cooldown.Key);
                }
            }
        }
        
    }

    public class Cooldown
    {
        public string Name { get; set; }
        public int Seconds { get; set; }
        public DateTime EndTime { get; set; }
        public int Hue { get; set; }
        public int Icon { get; set; } //30010 - 30057
        public int SoundId { get; set; }
        public bool StayVisible { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }

    }
}