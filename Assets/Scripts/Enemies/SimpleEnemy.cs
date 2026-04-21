using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    PlayerController playerController;
    GameObject player;
    [SerializeField] private bool AIEnabled;
    public StateMachineNode currentState { get; private set; }
    void Start()
    {
        playerController = PlayerController.instance;
        player = playerController.gameObject;
        currentState = new EnemyNullState();
        currentState.EnterState(this, currentState);
    }

    void Update()
    {
        if (!AIEnabled) return;
        currentState.FrameTick(this);
    }
    private void FixedUpdate()
    {
        if (!AIEnabled) return;
        currentState.PhysicsTick(this);
        currentState.ConditionUpdate(this);
        //ResetVariables();
    }

    public void ChangeState(StateMachineNode toState)
    {
        if (toState == null) return;
        currentState.ExitState(this, toState);
        toState.EnterState(this, currentState);
        currentState = toState;
    }
}

public class EnemyNullState : StateMachineNode
{
    public string Name => "EnemyNullState";

    public StateMachineNode Clone()
    {
        return new EnemyNullState();
    }

    public void ConditionUpdate(object invoker)
    {
        SimpleEnemy fms = (SimpleEnemy)invoker;
        fms.ChangeState(new EnemyWatchingState());
    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        
    }

    public void FrameTick(object invoker)
    {
        
    }

    public void PhysicsTick(object invoker)
    {
        
    }
}
public class EnemyWatchingState : StateMachineNode
{
    SimpleEnemy fms;
    GameObject player;
    public string Name => "EnemyWatchingState";

    public StateMachineNode Clone()
    {
        return new EnemyWatchingState();
    }

    public void ConditionUpdate(object invoker)
    {
        fms = (SimpleEnemy)invoker;
        if (Vector3.Distance(fms.gameObject.transform.position, player.transform.position) < 10.0f)
        {
            fms.ChangeState(new EnemyChasingState());
        }
    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        player = PlayerController.instance.gameObject;
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {

    }

    public void FrameTick(object invoker)
    {

    }

    public void PhysicsTick(object invoker)
    {

    }
}
public class EnemyChasingState : StateMachineNode
{
    SimpleEnemy fms;
    GameObject player;
    public string Name => "EnemyWatchingState";

    public StateMachineNode Clone()
    {
        return new EnemyWatchingState();
    }

    public void ConditionUpdate(object invoker)
    {
        fms = (SimpleEnemy)invoker;
        if (Vector3.Distance(fms.gameObject.transform.position, player.transform.position) > 15.0f)
        {
            fms.ChangeState(new EnemyWatchingState());
        }
    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        player = PlayerController.instance.gameObject;
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {

    }

    public void FrameTick(object invoker)
    {

    }

    public void PhysicsTick(object invoker)
    {
        Vector3 playerPos = player.transform.position;
        //Vector3 myPos = fms.transform.position;
        //Vector3 dir = playerPos - myPos;
        fms.transform.LookAt(playerPos);

    }
}


