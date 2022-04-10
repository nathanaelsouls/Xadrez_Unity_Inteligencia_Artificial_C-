using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class StateMachineController : MonoBehaviour
{
    public static StateMachineController instance;
    public Player player1;
    public Player player2;
    public Player currentlyPlaying;
    public TaskCompletionSource<object> taskHold;
    public GameObject promotionPanel;
    State _current;
    bool busy;

    void Awake(){
        instance = this;
    }

    void Start(){
        ChangeTo<LoadState>();
    }

    public void ChangeTo<T>() where T:State{
        State state = GetState<T>();
        if (_current != state){
            ChangeState(state);
        }
    }

    public T GetState<T>() where T:State{
        T target = GetComponent<T>();
        if(target == null)
            target = gameObject.AddComponent<T>(); 
        return target;
    }

    void ChangeState(State value){
        if(busy)
            return;                   
        busy = true;

        if(_current != null)
            _current.Exit();                   

        _current = value;
        if(_current != null)
            _current.Enter();        

        busy = false;
        
    }
}
