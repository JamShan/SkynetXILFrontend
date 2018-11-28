using UnityEngine;
using Entitas;
using System.Collections.Generic;

public class LoginCMDSystem : ReactiveSystem<InputEntity>
{
    readonly Contexts m_contexts;

    public LoginCMDSystem(Contexts contexts) : base(contexts.input)
    {
        m_contexts = contexts;
    }

    protected override bool Filter(InputEntity entity)
    {
        return entity.hasLoginCMD;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector(InputMatcher.LoginCMD);
    }

    protected override void Execute(List<InputEntity> entities)
    {
        foreach (var entity in entities)
        {
            InputLoginService.singleton.DoLoginLogic(entity);
        }

    }


}
