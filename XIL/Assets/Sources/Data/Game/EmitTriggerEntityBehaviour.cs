using UnityEngine;

public class EmitTriggerEntityBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(name + "====" + other.name);
        var entity = Contexts.sharedInstance.game.CreateEntity();
        entity.AddCollision(gameObject, other.gameObject);
    }
}

