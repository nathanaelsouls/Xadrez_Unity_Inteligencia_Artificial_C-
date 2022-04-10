using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBeginState : State
{
    public override async void Enter(){
        Debug.Log("Turn begin:");
        if(machine.currentlyPlaying == machine.player1){
            machine.currentlyPlaying = machine.player2;
        } else {
            machine.currentlyPlaying = machine.player1;
        }

        Debug.Log(machine.currentlyPlaying+" now playing");
        await Task.Delay(100);
        if(machine.currentlyPlaying.AIControlled)
            machine.ChangeTo<AIPlayingState>();
        else
            machine.ChangeTo<PieceSelectionState>();
    }
}
