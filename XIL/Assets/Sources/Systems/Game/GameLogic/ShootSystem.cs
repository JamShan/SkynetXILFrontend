using UnityEngine;
using Entitas;

public class ShootSystem : IExecuteSystem
{
    readonly Contexts m_contexts;

    public ShootSystem(Contexts contexts)
    {
        m_contexts = contexts;
    }

    public void Execute()
    {
        // InputManger 有不同设备的需要把默认 Jump的 都改为 Fire(空格)
        if (Input.GetButtonDown("Fire"))
        {
            InputLoginService.singleton.CreateLoginCMDEntity();
            GameEntityService.singleton.CreateLaser();
        }
    }
}

