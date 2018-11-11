using Entitas;
using UnityEngine;

public class InputSystem : IExecuteSystem
{
    readonly Contexts m_contexts;

    public InputSystem(Contexts contexts)
    {
        m_contexts = contexts;
    }

    public  void Execute()
    {
        // 需编辑器中设置 inputmanger
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        m_contexts.game.ReplaceInput(new Vector3(horizontal, vertical, 0f));
    }
}
