using BehaviorTreeMila;
using UnityEngine;
using UnityEngine.AI;
using static EnemiesEnums;

namespace BasicEnemyMeleMissions
{
    public class MeleBasicEnemyMissions : Node
    {
    } 

    public class CheckEnemyInChaseRange : Node
    {
        BasicEnemyMele _enemy;

        public CheckEnemyInChaseRange(BasicEnemyMele enemy)
        {
            _enemy = enemy; 
        }

        public override NodeState Evaluate()
        {
            if(_enemy.playerInRange && _enemy.agent.enabled)
            {
                state = NodeState.SUCCESS;
                _enemy.agent.stoppingDistance = 2.5f;
                return state;
            }
            else
            {
                state = NodeState.FAILURE;
                _enemy.agent.stoppingDistance = 1;
                return state;
            }
                
        }
    }

    public class IsInAttackRange : Node
    {
        BasicEnemyMele _enemy;

        public IsInAttackRange(BasicEnemyMele enemy)
        {
            _enemy = enemy;
        }

        public override NodeState Evaluate()
        {
            float distanceToPlayer = Vector3.Distance(_enemy.transform.position, gameManager.instance.player.transform.position);
            //Debug.Log(distanceToPlayer);
            if (_enemy.playerInRange && distanceToPlayer <= _enemy.agent.stoppingDistance)
            {
                state = NodeState.SUCCESS;
                _enemy.agent.stoppingDistance = 3;
                return state;
            }
            else
            {
                state = NodeState.FAILURE;
                
                return state;
            }

        }
    }

    public class ChasePlayer : Node
    {
        BasicEnemyMele _enemy;
        public ChasePlayer(BasicEnemyMele enemy)
        {
            _enemy = enemy;
        }

        public override NodeState Evaluate()
        {
            _enemy.agent.SetDestination(gameManager.instance.player.transform.position);
            state =  NodeState.RUNNING;
            return state;
        }
    }

    public class EnemyDead : Node
    {
        BasicEnemyMele _enemy;

        public EnemyDead(BasicEnemyMele enemy)
        {
            _enemy = enemy;
        }
        public override NodeState Evaluate()
        {
            if (_enemy.characterStats.IsDead())
            {
                state = NodeState.SUCCESS;
                _enemy.agent.enabled = false;               
                return state;
            }
            else
            {
                state = NodeState.FAILURE;
                return state;
            }
        }
    }

    public class DestroyEnemy : Node
    { 
        BasicEnemyMele enemy;
        bool _deadTriggered = false;

        public DestroyEnemy(BasicEnemyMele enemy)
        {
            this.enemy = enemy;
        }
        public override NodeState Evaluate()
        {
            if (!_deadTriggered)
            {
                enemy.anim.SetTrigger("isDead");
                _deadTriggered = true;
            }
            
            state = NodeState.RUNNING;
            return state;
        }
    }

    public class AttackPlayer : Node
    {
        BasicEnemyMele enemy;        
        bool isWaiting = true;
        float wait;
        public AttackPlayer(BasicEnemyMele enemy)
        {
            this.enemy = enemy;
        }
        public override NodeState Evaluate()
        {
            
            if (enemy.isAttacking)
            {
                //OK
            }
            if(isWaiting)
            {
                wait += Time.deltaTime;
                if(wait > 2)
                {
                    isWaiting = false;
                }
            }
            else
            {
                if(enemy.onAttackCoolDown == false)
                {
                    enemy.anim.SetFloat("attackSpeed", enemy.characterStats.attackSpeed);
                    enemy.AttackMelle();
                    enemy.onAttackCoolDown = true;
                    wait = 0;
                    isWaiting = true;
                    
                    
                }
                
            }
            state = NodeState.RUNNING;
            return state;
        }
    }

}
namespace BasicEnemySkeletonMissions
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
                    if (_agent.enabled)
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
    public class MeleBasicEnemyMissions : Node
    {
    }
    public class CheckEnemyInChaseRange : Node
    {
        SkeletonEnemyDefault _enemy;

        public CheckEnemyInChaseRange(SkeletonEnemyDefault enemy)
        {
            _enemy = enemy;
        }

        public override NodeState Evaluate()
        {
            if (_enemy.characterStats.playerInRange && _enemy.characterStats.agent.enabled)
            {
                Debug.Log("player in range");
                state = NodeState.SUCCESS;
                _enemy.characterStats.agent.stoppingDistance = 3;
                return state;
            }
            else
            {
                state = NodeState.FAILURE;
                _enemy.characterStats.agent.stoppingDistance = 1;
                return state;
            }

        }
    }

    public class IsInAttackRange : Node
    {
        SkeletonEnemyDefault _enemy;

        public IsInAttackRange(SkeletonEnemyDefault enemy)
        {
            _enemy = enemy;
        }

        public override NodeState Evaluate()
        {
            float distanceToPlayer = Vector3.Distance(_enemy.transform.position, gameManager.instance.thirdPersonPlayerController.transform.position);
            //Debug.Log(distanceToPlayer);
            if (_enemy.characterStats.playerInRange && distanceToPlayer <= _enemy.characterStats.agent.stoppingDistance)
            {
                state = NodeState.SUCCESS;
                _enemy.characterStats.agent.stoppingDistance = 2;
                _enemy.facePlayer();
                return state;
            }
            else
            {
                state = NodeState.FAILURE;

                return state;
            }

        }
    }

    public class ChasePlayer : Node
    {
        SkeletonEnemyDefault _enemy;
        public ChasePlayer(SkeletonEnemyDefault enemy)
        {
            _enemy = enemy;
        }

        public override NodeState Evaluate()
        {
            Debug.Log("Chacing player");
            _enemy.characterStats.agent.SetDestination(gameManager.instance.thirdPersonPlayerController.transform.position);
            state = NodeState.RUNNING;
            return state;
        }
    }

    public class EnemyDead : Node
    {
        SkeletonEnemyDefault _enemy;

        public EnemyDead(SkeletonEnemyDefault enemy)
        {
            _enemy = enemy;
        }
        public override NodeState Evaluate()
        {
            if (_enemy.characterStats.IsDead())
            {
                state = NodeState.SUCCESS;
                _enemy.characterStats.agent.enabled = false;
                return state;
            }
            else
            {
                state = NodeState.FAILURE;
                return state;
            }
        }
    }

    public class DestroyEnemy : Node
    {
        SkeletonEnemyDefault enemy;
        bool _deadTriggered = false;

        public DestroyEnemy(SkeletonEnemyDefault enemy)
        {
            this.enemy = enemy;
        }
        public override NodeState Evaluate()
        {
            if (!_deadTriggered)
            {
                enemy.characterStats.animator.SetTrigger("isDead");
                _deadTriggered = true;
            }

            state = NodeState.RUNNING;
            return state;
        }
    }

    public class AttackPlayer : Node
    {
        SkeletonEnemyDefault enemy;
        bool isWaiting = true;
        float wait;
        public AttackPlayer(SkeletonEnemyDefault enemy)
        {
            this.enemy = enemy;
        }
        public override NodeState Evaluate()
        {

            if (enemy.characterStats.isAttacking)
            {
                //OK
            }
            if (isWaiting)
            {
                wait += Time.deltaTime;
                if (wait > 2)
                {
                    isWaiting = false;
                }
            }
            else
            {
                if (enemy.characterStats.onAttackCoolDown == false)
                {
                    enemy.AttackMelle();
                    enemy.characterStats.onAttackCoolDown = true;
                    wait = 0;
                    isWaiting = true;


                }

            }
            state = NodeState.RUNNING;
            return state;
        }
    }
}
namespace BasicEnemyGolemMissions
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
            _currentWaypointIndex = Random.Range(0, _waypoints.Length);
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
                if (_agent.enabled)
                {
                    Transform wp = _waypoints[_currentWaypointIndex];
                    //Debug.Log(Vector3.Distance(_transform.position, wp.position));
                    if (Vector3.Distance(_transform.position, wp.position) < 1.6)
                    {
                        //_transform.position = wp.position;
                        _waitCounter = 0f;
                        _waiting = true;

                        _currentWaypointIndex = Random.Range(0, _waypoints.Length);
                          Debug.Log("Current index is" + _currentWaypointIndex);
                        //_animator.SetBool("Walking", false);
                    }
                    else
                    {
                        if (_agent.enabled)
                        {
                            _agent.SetDestination(wp.position);
                            LookAtTarget(wp, _transform);
                        }

                        // _transform.LookAt(wp.position);
                    }
                }
                else
                {
                    state = NodeState.FAILURE;
                    return state;
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
    public class MeleBasicEnemyMissions : Node
    {
    }
    public class CheckEnemyInChaseRange : Node
    {
        GolemEnemyBasic _enemy;

        public CheckEnemyInChaseRange(GolemEnemyBasic enemy)
        {
            _enemy = enemy;
        }

        public override NodeState Evaluate()
        {
            if (_enemy.characterStats.playerInRange && _enemy.characterStats.agent.enabled)
            {
                Debug.Log("player in range");
                state = NodeState.SUCCESS;
                _enemy.characterStats.agent.stoppingDistance = 3;
                return state;
            }
            else
            {
                state = NodeState.FAILURE;
                _enemy.characterStats.agent.stoppingDistance = 1;
                return state;
            }

        }
    }

    public class IsInAttackRange : Node
    {
        GolemEnemyBasic _enemy;

        public IsInAttackRange(GolemEnemyBasic enemy)
        {
            _enemy = enemy;
        }

        public override NodeState Evaluate()
        {
            float distanceToPlayer = Vector3.Distance(_enemy.transform.position, gameManager.instance.thirdPersonPlayerController.transform.position);
            //Debug.Log(distanceToPlayer);
            if (_enemy.characterStats.playerInRange && distanceToPlayer <= _enemy.characterStats.agent.stoppingDistance)
            {
                state = NodeState.SUCCESS;
                _enemy.characterStats.agent.stoppingDistance = 2;
                _enemy.facePlayer();
                return state;
            }
            else
            {
                state = NodeState.FAILURE;

                return state;
            }

        }
    }

    public class ChasePlayer : Node
    {
        GolemEnemyBasic _enemy;
        public ChasePlayer(GolemEnemyBasic enemy)
        {
            _enemy = enemy;
        }

        public override NodeState Evaluate()
        {
            Debug.Log("Chacing player");
            _enemy.characterStats.agent.SetDestination(gameManager.instance.thirdPersonPlayerController.transform.position);
            state = NodeState.RUNNING;
            return state;
        }
    }

    public class EnemyDead : Node
    {
        GolemEnemyBasic _enemy;

        public EnemyDead(GolemEnemyBasic enemy)
        {
            _enemy = enemy;
        }
        public override NodeState Evaluate()
        {
            if (_enemy.characterStats.IsDead())
            {
                state = NodeState.SUCCESS;
                _enemy.characterStats.agent.enabled = false;
                return state;
            }
            else
            {
                state = NodeState.FAILURE;
                return state;
            }
        }
    }

    public class DestroyEnemy : Node
    {
        GolemEnemyBasic enemy;
        bool _deadTriggered = false;

        public DestroyEnemy(GolemEnemyBasic enemy)
        {
            this.enemy = enemy;
        }
        public override NodeState Evaluate()
        {
            if (!_deadTriggered)
            {
                enemy.characterStats.animator.SetTrigger("Dead");
                _deadTriggered = true;
            }

            state = NodeState.RUNNING;
            return state;
        }
    }

    public class AttackPlayer : Node
    {
        GolemEnemyBasic enemy;
        bool isWaiting = true;
        float wait;
        public AttackPlayer(GolemEnemyBasic enemy)
        {
            this.enemy = enemy;
        }
        public override NodeState Evaluate()
        {

            if (enemy.characterStats.isAttacking)
            {
                //OK
            }
            if (isWaiting)
            {
                wait += Time.deltaTime;
                if (wait > 2)
                {
                    isWaiting = false;
                }
            }
            else
            {
                if (enemy.characterStats.onAttackCoolDown == false)
                {
                   
                    enemy.AttackMelle();
                    enemy.characterStats.onAttackCoolDown = true;
                    wait = 0;
                    isWaiting = true;
                }

            }
            state = NodeState.RUNNING;
            return state;
        }
    }
}


