using Bloody.Core;
using CrimsonHunt.Utils;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;

namespace CrimsonHunt.Structs;

public struct PlayerData
{
    public FixedString64Bytes CharacterName { get; set; }
    public ulong SteamID { get; set; }
    public bool IsOnline { get; set; }
    public Entity UserEntity { get; set; }
    public Entity CharEntity { get; set; }

    public PlayerData(FixedString64Bytes _characterName = default, ulong _steamID = 0, bool _isOnline = false, Entity _userEntity = default, Entity _charEntity = default)
    {
        CharacterName = _characterName;
        SteamID = _steamID;
        IsOnline = _isOnline;
        UserEntity = _userEntity;
        CharEntity = _charEntity;
    }
}

public class Player
{
    public string Name { get; set; }
    public ulong SteamID { get; set; }
    public bool IsOnline { get; set; }
    public bool IsAdmin { get; set; }
    public Entity User { get; set; }
    public Entity Character { get; set; }

    public Player(Entity userEntity = default, Entity charEntity = default)
    {
        User = userEntity;
        var user = User.Read<User>();
        Character = user.LocalCharacter._Entity;
        Name = user.CharacterName.ToString();
        IsOnline = user.IsConnected;
        IsAdmin = user.IsAdmin;
        SteamID = user.PlatformId;
    }
}
