using System;
using UnityEngine;

public class Transition
{
    MoveState originalState;
    MoveState endState;
    Func<bool> condition;

    public Transition(MoveState origin, MoveState destination, Func<bool> condition)
    {
        originalState = origin;
        endState = destination;
        this.condition = condition;
    }

    public MoveState CheckState()
    {
        if (condition())
        {
            return endState;
        }
        return originalState;
    }
}
