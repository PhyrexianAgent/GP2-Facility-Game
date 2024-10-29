using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine : MonoBehaviour //not adding GetOrAddNode because I dont see the point
{
    protected class StateNode{
        public IState State;
        public HashSet<ITransition> Transitions;

        public StateNode(IState state){
            State = state;
            Transitions = new HashSet<ITransition>();
        }
        public void AddTransition(IState to, IPredicate condition){
            Transitions.Add(new Transition(to, condition));
        }
        public void AddTransition(IState to, IPredicate condition, Func<IState, bool> func){ // For setting a transition with a method to call before next state's enter method
            Transitions.Add(new Transition(to, condition, func));
        }
    }
    
    private StateNode currentState;
    private Dictionary<Type, StateNode> nodes = new Dictionary<Type, StateNode>();

    protected void AddNode(IState state, bool makeFirstState = false){
        nodes.Add(state.GetType(), new StateNode(state));
        if (makeFirstState) {
            SetCurrentState(state);
        }
    }
    protected StateNode GetNode(IState state) => nodes[state.GetType()];
    protected ITransition GetTransition(){
        if (currentState == null)
            return null;
        foreach(ITransition transition in currentState.Transitions){
            if (transition.Condition.Evaluate())
                return transition;
        }
        return null;
    }
    public void ChangeCurrentState(IState state){
        if (currentState.State == state)
            return;

        StateNode nextState = nodes[state.GetType()];

        currentState.State?.OnExit();
        nextState.State?.OnEnter();
        currentState = nextState;
    }
    public void AddTransition(IState from, IState to, IPredicate condition){
        nodes[from.GetType()].AddTransition(to, condition);
    }
    public void AddTransition(IState from, IState to, IPredicate condition, Func<IState, bool> func){
        nodes[from.GetType()].AddTransition(to, condition, func);
    }
    public void SetCurrentState(IState state){
        currentState = nodes[state.GetType()];
        currentState.State?.OnEnter();
    }
    public void Update(){
        ITransition transition = GetTransition();
        if (transition != null){
            transition?.CallTransition();
            ChangeCurrentState(transition.To);
        }
        if (currentState != null)
            currentState.State?.Update();
    }
    public void FixedUpdate(){
        if (currentState != null)
            currentState.State?.FixedUpdate();
    }
}
