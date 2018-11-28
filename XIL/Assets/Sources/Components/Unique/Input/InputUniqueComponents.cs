using Entitas;
using Entitas.CodeGeneration.Attributes;
using SkynetClient;

[Input, Unique]
public class LoginCMD : IComponent
{
   public  AuthPackage cmd;
}
