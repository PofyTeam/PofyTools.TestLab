using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools;

namespace Guvernal.CardGame
{

    public class RequestMoveButton : ButtonView
    {

        public Direction requestDirection;

        protected override void OnClick ()
        {
            Board.requestMove (this.requestDirection);
        }
    }

}