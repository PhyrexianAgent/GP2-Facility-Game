using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Transition : ITransition
{
    public IState To {get;}
    public IPredicate Condition {get;}
    private Func<IState, bool> transitionFunc; // Stores a method with an IState perameter to call before transition's To state's OnEnter is called.

    public Transition(IState to, IPredicate condition){
        To = to;
        Condition = condition;
    }
    public Transition(IState to, IPredicate condition, Func<IState, bool> transitionFunc) : this(to, condition){
        this.transitionFunc = transitionFunc;
    }
    public void CallTransition(){ // Invokes the given method if any was assigned
        if (transitionFunc != null)
            transitionFunc(To);
    }
}
