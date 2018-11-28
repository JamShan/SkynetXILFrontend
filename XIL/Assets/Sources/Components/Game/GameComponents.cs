using Entitas;
using UnityEngine;
using Entitas.CodeGeneration.Attributes;


[Game]
public class AccelerationComponent : IComponent
{
    public Vector3 value;
}


[Game]
public class AsteroidComponent : IComponent
{
    public int level;
}


[Game]
public class InitialPositionComponent : IComponent
{
    public Vector3 value;
}

[Game]
public class LaserComponent : IComponent
{
}

[Game]
public class ResourceComponent : IComponent
{
    public GameObject prefab;
}

[Game]
public class ViewComponent : IComponent
{
    [EntityIndex]
    public GameObject  value;
}

[Game]
public class CollisionComponent : IComponent
{
    public GameObject first;
    public GameObject second;
}


[Game]
public class DestroyedComponent : IComponent
{
}
