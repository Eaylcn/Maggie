using System;
using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string stateName;

    // |EN| Constructor to initialize the state with player, state machine and state name |TR| State'i player, state machine ve state adı ile başlatmak için yapıcı
    public EntityState(Player player, StateMachine stateMachine, string stateName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
        // |EN| Eveytime state will be changed this method will be called |TR| Her state değiştiğinde bu method çağrılacak
        Debug.Log("I entered to " + stateName + " state ");
    }

    public virtual void Update()
    {
        // |EN| We are going to run logic of the state here |TR| State'in mantığını burada çalıştıracağız
        Debug.Log("I am updating " + stateName + " state ");
    }
    
    public virtual void Exit()
    {
        // |EN| This method will be called when exiting the state |TR| Bu method state'den çıkarken çağrılacak
        Debug.Log("I exited from " + stateName + " state ");
    }
}
