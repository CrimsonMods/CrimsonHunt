using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace CrimsonHunt.Structs;

public struct KillEvent
{ 
    public DateTime Killed { get; set; }
    public Entity Boss { get; set; }
    public string BossName { get; set; }
    public List<Player> Players { get; set; }

    public KillEvent(Entity _boss, string _bossName)
    {
        Killed = DateTime.Now;
        Boss = _boss;
        BossName = _bossName;
        Players = new List<Player>();
    }

    public readonly void AddPlayer(Player player)
    {
        int exists = Players.Where(x => x.Name == player.Name).ToList().Count;
        if (exists == 0) Players.Add(player);
    }
}
