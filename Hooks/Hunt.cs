using Bloodstone.API;
using CrimsonHunt.Structs;
using HarmonyLib;
using ProjectM;
using Stunlock.Core;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace CrimsonHunt.Hooks;

[HarmonyPatch]
internal class Hunt
{
    [HarmonyPatch(typeof(VBloodSystem), nameof(VBloodSystem.OnUpdate))]
    [HarmonyPrefix]
    public static void OnUpdate_Prefix(VBloodSystem __instance)
    {
        if (!Plugin.Settings.GetActiveSystem(Systems.ENABLE) || __instance.EventList.IsEmpty) return;

        foreach (var _event in __instance.EventList)
        {
            var _bossId = __instance._PrefabCollectionSystem._PrefabDataLookup[_event.Source].AssetName;
            Entity _bossEntity = GetVBlood(__instance, _event);
            Plugin.LogInstance.LogMessage("VBloodSystem.OnUpdate");

            if (!VWorld.Server.EntityManager.TryGetComponentData<PlayerCharacter>(_event.Target, out var _data) ||
                _bossId.ToString() == "CHAR_Vermin_DireRat_VBlood" || _bossEntity == Entity.Null) continue;

            Player _player = new(_data.UserEntity);

            int _exists = ActionScheduler.KillEvents.Where(x => x.BossName == _bossId.ToString()).ToList().Count;
            if (_exists == 0)
            {
                Structs.KillEvent _killEvent = new Structs.KillEvent(_bossEntity, _bossId.ToString());
                ActionScheduler.KillEvents.Add(_killEvent);
            }

            foreach (var _killevent in ActionScheduler.KillEvents)
            {
                if (_killevent.BossName == _bossId.ToString())
                {
                    _killevent.AddPlayer(_player);
                }
            }
        }
    }

    private static Entity GetVBlood(VBloodSystem __instance, VBloodConsumed _event)
    {
        var _entities = __instance.EntityManager.CreateEntityQuery(new EntityQueryDesc()
        {
            All = new ComponentType[]
                {
                    ComponentType.ReadOnly<VBloodUnit>()
                },
            Options = EntityQueryOptions.IncludeAll
        }).ToEntityArray(Allocator.Temp);

        Entity _bossEntity = Entity.Null;
        foreach (var _entity in _entities)
        {
            if (!VWorld.Server.EntityManager.TryGetComponentData<PrefabGUID>(_entity, out var _prefabGUID)) continue;
            if (_prefabGUID.GuidHash == _event.Source.GuidHash)
            {
                _bossEntity = _entity;
                break;
            }
        }
        return _bossEntity;
    }
}
