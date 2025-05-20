using UnityEngine;
using BehaviorTreeMila;
using UnityEngine.AI;


namespace BasicEnemyMeleMissions
{
    public class TaskPatrol : Node
{
    private Transform _transform;
    //private Animator _animator;
    private Transform[] _waypoints;

    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;
    NavMeshAgent _agent;

    public TaskPatrol(Transform transform, Transform[] waypoints, NavMeshAgent agent)
    {
        _transform = transform;
        //_animator = transform.GetComponent<Animator>();
        _waypoints = waypoints;
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                    _agent.stoppingDistance = 1;
                    _waiting = false;
                //_animator.SetBool("Walking", true);
            }
        }
        else
        {
            Transform wp = _waypoints[_currentWaypointIndex];
            //Debug.Log(Vector3.Distance(_transform.position, wp.position));
            if (Vector3.Distance(_transform.position, wp.position) < 1.6)
            {
                //_transform.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                //_animator.SetBool("Walking", false);
            }
            else
            {
                if(_agent.enabled)
                {
                    _agent.SetDestination(wp.position);
                    LookAtTarget(wp, _transform);
                }
                
               // _transform.LookAt(wp.position);
            }
        }


        state = NodeState.RUNNING;
        return state;
    }
        private void LookAtTarget(Transform target, Transform transform)
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        }

    }


}
