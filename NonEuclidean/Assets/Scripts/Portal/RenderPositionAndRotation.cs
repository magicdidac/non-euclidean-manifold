using System.Collections.Generic;
using UnityEngine;

public struct PositionAndRotation
{
    public Vector3 renderPosition;
    public Quaternion renderRotation;
    public MinePortal alsoPortal;

    public PositionAndRotation(Vector3 position, Quaternion rotation)
    {
        this.renderPosition = position;
        this.renderRotation = rotation;
        this.alsoPortal = null;
    }

    public PositionAndRotation(Vector3 position, Quaternion rotation, MinePortal portal)
    {
        this.renderPosition = position;
        this.renderRotation = rotation;
        this.alsoPortal = portal;
    }
}

public class RenderPositionAndRotation
{

    public PositionAndRotation linked;
    public List<PositionAndRotation> alsoVisible = new List<PositionAndRotation>();

    public RenderPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        this.linked = new PositionAndRotation(position, rotation);
    }

    public void Add(PositionAndRotation par)
    {
        alsoVisible.Add(par);
    }

}
