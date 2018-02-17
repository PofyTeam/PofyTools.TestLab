using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools;

namespace Guvernal.CardGame
{

    public class StartGameButton : ButtonView
    {
        protected override void OnClick ()
        {
            Board.requestGameStart ();
            this.gameObject.SetActive (false);
        }

        
    }


}