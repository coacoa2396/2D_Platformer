using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Eagle : Monster
{
    public Transform playerTransform;
    public Vector3 startPos;
    public StateMachine fsm;


    private void Awake()
    {
        
    }

    private void Start()
    {
        fsm = new StateMachine(this);
        startPos = transform.position;
    }

    private void Update()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }


    public void ChangeState(string name)
    {
        switch (name)
        {
            case "Idle":
                fsm.ChangeState(idleState)
        }
    }

    public class StateMachine
    {
        public IState curState;

        public IdleState idleState;
        public TraceState traceState;
        public ReturnState returnState;

        public StateMachine(Eagle eagle)
        {
            idleState = new IdleState(eagle);
            traceState = new TraceState(eagle);
            returnState = new ReturnState(eagle);

            curState = idleState;
        }

        public void Update()
        {
            curState.Update();
        }

        public void ChangeState(IState nextState)
        {
            curState.Exit();
            curState = nextState;
            curState.Enter();
        }

        public void SetInitState(IState initState)
        {
            curState = initState;
            curState.Enter();
        }
    }

    
    
    public class IdleState : IState
    {
        Eagle owner;
        Transform playerTransform;
        float findRange = 5f;

        public IdleState(Eagle owner)
        {
            this.owner = owner;
        }

        public void Enter()
        {
            Debug.Log("아이들 됨?");
            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        public void Update()
        {
            if (Vector3.Distance(playerTransform.position, owner.transform.position) < findRange)
            {
                // 상태변화
                owner.ChangeState("Trace");
            }
        }

        public void Exit()
        {

        }
    }

    
    public class TraceState : IState
    {
        Eagle owner;
        Transform playerTransform;
        float findRange = 5f;
        float moveSpeed = 2f;

        public TraceState(Eagle owner)
        {
            this.owner = owner;

        }

        public void Enter()
        {
            Debug.Log("추적 됨?");
            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        public void Update()
        {
            Vector3 dir = (playerTransform.position - owner.transform.position).normalized;
            owner.transform.Translate(dir * moveSpeed * Time.deltaTime);

            if (Vector3.Distance(playerTransform.position, owner.transform.position) < findRange)
            {
                owner.ChangeState("Return");
            }
        }

        public void Exit()
        {

        }
    }

    public class ReturnState : IState
    {
        Eagle owner;
        Transform playerTransform;
        Vector3 startPos;
        float findRange = 5f;
        float returnSpeed = 10f;

        public ReturnState(Eagle owner)
        {
            this.owner = owner;
            startPos = owner.transform.position;
        }

        public void Enter()
        {
            Debug.Log("리턴 됨?");
        }

        public void Update()
        {
            Vector3 dir = (startPos - owner.transform.position).normalized;
            owner.transform.Translate(dir * returnSpeed * Time.deltaTime);

            if (Vector3.Distance(owner.transform.position, startPos) < 0.01f)
            {
                owner.ChangeState("Idle");
            }

            if (Vector3.Distance(playerTransform.position, owner.transform.position) < findRange)
            {
                owner.ChangeState("Trace");
            }
        }

        public void Exit()
        {

        }


    }
    

    /*
    void IdleUpdate()
    {
        // 아무것도 안하는 상태

        if (Vector2.Distance(playerTransform.position, transform.position) < findRange)
        {
            curState = State.Trace;
        }
    }

    void TraceUpdate()
    {
        Vector3 dir = (playerTransform.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (Vector2.Distance(playerTransform.position, transform.position) < findRange)
        {
            curState = State.Return;
        }
    }

    void ReturnUpdate()
    {
        Vector3 dir = (startPos - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, startPos) < 0.01f)
        {
            curState = State.Idle;
        }

        if (Vector2.Distance(playerTransform.position, transform.position) < findRange)
        {
            curState = State.Trace;
        }


    }

    void HitUpdate()
    {

    }

    void DiedUpdate()
    {

    }
    */
}
