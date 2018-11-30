using UnityEngine;
using Entitas;

public class ReplaceAccelerationSystem : IExecuteSystem
{
    readonly Contexts m_contexts;

    public ReplaceAccelerationSystem(Contexts contexts)
    {
        m_contexts = contexts;
    }

    public void Execute()
    {
        var input = m_contexts.game.input.value.y;
        var player = m_contexts.game.playerEntity;
        var player_transform = player.view.value.transform;
        var forward = player_transform.up;
        var move_speed = m_contexts.game.gameDataSetup.value.playerMovementSpeed;
        var acceleration = player.acceleration.value;
        player.ReplaceAcceleration(acceleration + input * forward * move_speed * Time.deltaTime);
    }
}

