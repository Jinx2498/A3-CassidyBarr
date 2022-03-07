using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.Controllers
{
    public abstract class Controller : ExtendedMonoBehaviour
    {
        [SerializeField] protected bool isPlayerControlled;
        //TODO: There should be a mover maximum speed and a player maximum speed
        [SerializeField] protected float maximumSpeed = 5;
    }
}