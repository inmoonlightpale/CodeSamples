using UnityEngine;

public class VUIAnim_Receiver_GameSTate : VUIAnim_Receiver
{

    [SerializeField]
    private GameStates EnableState;

    [SerializeField]
    private bool CurrentlyEnabled;

    void OnEnable()
    {
        events.OnGameStateChanging += EventsOnOnGameStateChanging;
    }

    private void EventsOnOnGameStateChanging(GameStates newstate)
    {
        if (CurrentlyEnabled)
        {
            if (newstate != EnableState)
            {
                TriggerElements();
                CurrentlyEnabled = false;
            }
        }
        else
        {
            if (newstate == EnableState)
            {
                TriggerElements();
                CurrentlyEnabled = true;
            }
        }
    }

    void OnDisable()
    {
        events.OnGameStateChanging -= EventsOnOnGameStateChanging;
    }

}
