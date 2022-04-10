using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightClick : MonoBehaviour
{
    public AvailableMove move;
    void OnMouseDown(){
        InputController.instance.tileClicked(this, null);
    }
}
