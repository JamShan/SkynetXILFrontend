using System;

public class InputServices
{
    public static InputServices singleton = new InputServices();

    public void Initialize(Contexts contexts, InputController iputController)
    {
        InputLoginService.singleton.Initialize(contexts);
    }
}
