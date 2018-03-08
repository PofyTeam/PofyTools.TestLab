using UnityEngine;

public class CardFlip : MonoBehaviour
{

    private void OnMouseDown ()
    {
        this.transform.Rotate (Vector3.up * 180f);
    }

}
