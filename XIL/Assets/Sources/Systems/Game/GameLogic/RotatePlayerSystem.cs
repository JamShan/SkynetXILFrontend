using UnityEngine;
using Entitas;

public class RotatePlayerSystem : IExecuteSystem
{
    readonly Contexts  m_contexts;

    public RotatePlayerSystem(Contexts contexts)
    {
        m_contexts = contexts;
    }

    public void Execute()
    {
        var input = m_contexts.game.input.value.x;
        var player_transform = m_contexts.game.playerEntity.view.value.transform;
        var player_rotation = player_transform.rotation.eulerAngles;
        player_rotation.z -= input * m_contexts.game.gameDataSetup.value.rotationSpeed * Time.deltaTime;
        player_transform.rotation = Quaternion.Euler(player_rotation);
    }
}
