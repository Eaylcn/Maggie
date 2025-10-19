using UnityEngine;

/*
 * A simple state machine class that manages transitions between entity states.
 * It initializes with a starting state and handles switching between states
 * by calling the Enter, Update, and Exit methods of each state as appropriate.
 * Designed to be used in player or enemy controllers to organize complex behavior logic.
 */

/*
 * Entity state'leri arasındaki geçişleri yöneten basit bir state machine sınıfı.
 * Başlangıç durumuyla başlatılır ve uygun olduğunda her state’in Enter, Update ve Exit
 * metodlarını çağırarak durum değişimlerini yönetir.
 * Karmaşık davranış mantığını düzenlemek için player veya düşman kontrolörlerinde kullanılmak üzere tasarlanmıştır.
 */

public class StateMachine
{
    public EntityState currentState { get; private set; }

    // |EN| Initialize the state machine with a starting state |TR| State machine'i başlangıç state'i ile başlat
    public void Initialize(EntityState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(EntityState newState)
    {
        // |EN| Call the Exit method of the current state |TR| Mevcut state'in Exit methodunu çağır
        currentState.Exit();

        // |EN| Change to the new state |TR| Yeni duruma geç
        currentState = newState;

        // |EN| Call the Enter method of the new state |TR| Yeni state'in Enter methodunu çağır
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        // |EN| Update the current state |TR| Mevcut state'i güncelle
        currentState.Update();
    }
}
