using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected StateMachineController machine
    {
        get { return StateMachineController.instance;}
    }

    public virtual void Enter(){

    }

    public virtual void Exit(){

    }
}
