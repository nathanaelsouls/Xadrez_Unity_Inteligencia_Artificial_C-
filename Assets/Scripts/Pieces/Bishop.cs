using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    protected override void Start(){
        base.Start();
        movement = new BishopMovement(maxTeam);
    }
}
