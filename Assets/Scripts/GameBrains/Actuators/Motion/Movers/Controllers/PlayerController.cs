using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.Controllers
{
    public abstract class PlayerController : Controller
    {
        //TODO: Encapsulate these fields
        [SerializeField] protected string sideAxis = "Horizontal";
        [SerializeField] protected string forwardAxis = "Vertical";
    }
}