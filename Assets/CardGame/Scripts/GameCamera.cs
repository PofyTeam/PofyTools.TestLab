namespace Guvernal.CardGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using PofyTools;

    [RequireComponent (typeof (Camera))]
    public class GameCamera : StateableActor
    {
        [SerializeField]
        private Camera _camera;

        public override bool Initialize ()
        {
            if(base.Initialize ())
            {
                //Do Initialize
                return true;
            }
            return false;
        }

    }

}