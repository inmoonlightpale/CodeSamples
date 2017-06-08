using UnityEngine;

public class VUIAnim_Element : MonoBehaviour
{
    [SerializeField]
    private GameStates InitializationState = GameStates.initializing;

    [SerializeField]
    private VuiAnimTriggerStates TriggerState;

    [SerializeField]
    private VuiAnimTriggerStates StartingState = VuiAnimTriggerStates.Triggeroff;

    void Awake()
    {
        TriggerState = VuiAnimTriggerStates.Uninitialized;
    }

    void OnEnable()
    {
        events.OnGameStateChanging += EventsOnOnGameStateChanging;
    }

    private void EventsOnOnGameStateChanging(GameStates newstate)
    {
        if (newstate == InitializationState)
        {
            InitializeTrigger();
        }
    }

    void OnDisable()
    {
        events.OnGameStateChanging -= EventsOnOnGameStateChanging;
    }

    public virtual void InitializeTrigger()
    {
        SetTriggerState(StartingState);
        SnapStates();
    }

    public virtual void TriggerElement()
    {
        SwitchStates();
        ProcessState();
    }

    public virtual void TriggerOn()
    {

    }

    public virtual void TriggerOff()
    {

    }

    public virtual void SnapOn()
    {

    }

    public virtual void SnapOff()
    {

    }

    internal void SetTriggerState(VuiAnimTriggerStates newState)
    {
        TriggerState = newState;
    }

    internal void SnapStates()
    {
        if (TriggerState == VuiAnimTriggerStates.Triggeron) SnapOn();
        else if (TriggerState == VuiAnimTriggerStates.Triggeroff) SnapOff();
    }

    internal void ProcessState()
    {
        if (TriggerState == VuiAnimTriggerStates.Triggeron) TriggerOn();
        else if (TriggerState == VuiAnimTriggerStates.Triggeroff) TriggerOff();
    }

    internal void SwitchStates()
    {
        if (TriggerState == VuiAnimTriggerStates.Triggeroff) SetTriggerState(VuiAnimTriggerStates.Triggeron);
        else if (TriggerState == VuiAnimTriggerStates.Triggeron) SetTriggerState(VuiAnimTriggerStates.Triggeroff);
    }
}

public enum VuiAnimTriggerStates
{
    Uninitialized,
    Triggeron,
    Triggeroff
}