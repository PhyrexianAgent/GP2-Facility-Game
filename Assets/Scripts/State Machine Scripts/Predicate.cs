using System;

public class Predicate : IPredicate
{
    private readonly Func<bool> func;
    public Predicate(Func<bool> func){
        this.func = func;
    }
    public bool Evaluate() => func.Invoke();
}
