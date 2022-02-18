using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void D_Void();
    public delegate void D_Int(int value);
    public delegate void D_GameObjectInt(GameObject value, int value2);

    public static event D_GameObjectInt onActorDamaged;

    public static void ActorDamaged(GameObject obj, int damage) { onActorDamaged?.Invoke(obj, damage); }
}