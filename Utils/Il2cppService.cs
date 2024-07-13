using Bloodstone.API;
using Il2CppInterop.Runtime;
using Unity.Collections;
using Unity.Entities;

namespace CrimsonHunt.Utils;

public static class Il2cppService
{
    public static NativeArray<Entity> GetEntitiesByComponentTypes<T1>(bool includeAll = false)
    {
        EntityQueryOptions options = includeAll ? EntityQueryOptions.IncludeAll : EntityQueryOptions.Default;

        EntityQueryDesc queryDesc = new EntityQueryDesc
        {
            All = new ComponentType[] {
                new ComponentType(Il2CppType.Of<T1>(), ComponentType.AccessMode.ReadWrite)
            },
            Options = options
        };

        var query = VWorld.Server.EntityManager.CreateEntityQuery(queryDesc);
        var entities = query.ToEntityArray(Allocator.Temp);

        return entities;
    }

    public static NativeArray<Entity> GetEntitiesByComponentTypes<T1, T2>(bool includeAll = false)
    {
        EntityQueryOptions options = includeAll ? EntityQueryOptions.IncludeAll : EntityQueryOptions.Default;

        EntityQueryDesc queryDesc = new EntityQueryDesc
        {
            All = new ComponentType[] {
                new ComponentType(Il2CppType.Of<T1>(), ComponentType.AccessMode.ReadWrite),
                new ComponentType(Il2CppType.Of<T2>(), ComponentType.AccessMode.ReadWrite)
            },
            Options = options
        };

        var query = VWorld.Server.EntityManager.CreateEntityQuery(queryDesc);
        var entities = query.ToEntityArray(Allocator.Temp);

        return entities;
    }

    public static NativeArray<Entity> GetEntitiesByComponentTypes<T1, T2, T3>(bool includeAll = false)
    {
        EntityQueryOptions options = includeAll ? EntityQueryOptions.IncludeAll : EntityQueryOptions.Default;

        EntityQueryDesc queryDesc = new EntityQueryDesc
        {
            All = new ComponentType[] {
                new ComponentType(Il2CppType.Of<T1>(), ComponentType.AccessMode.ReadWrite),
                new ComponentType(Il2CppType.Of<T2>(), ComponentType.AccessMode.ReadWrite),
                new ComponentType(Il2CppType.Of<T3>(), ComponentType.AccessMode.ReadWrite)
            },
            Options = options
        };

        var query = VWorld.Server.EntityManager.CreateEntityQuery(queryDesc);
        var entities = query.ToEntityArray(Allocator.Temp);

        return entities;
    }
}
