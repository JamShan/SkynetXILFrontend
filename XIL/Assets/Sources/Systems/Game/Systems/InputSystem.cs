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
        // 需编辑器中设置 inputmanger w a s d
        // https://blog.csdn.net/yuxikuo_1/article/details/45111851
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        m_contexts.game.ReplaceInput(new Vector3(horizontal, vertical, 0f));
        //Debug.Log(horizontal + "  " + vertical);
    }
}
