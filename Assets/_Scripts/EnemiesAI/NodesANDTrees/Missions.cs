using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTrees
{
    public interface IStrategy
    {

        Node.Status Process();
        void Reset()
        {
            //Default implementation
        }
    }

    //Run the method attached to the action
    public class ActionStrategy : IStrategy
    {
        readonly Action doSomething;
        public ActionStrategy(Action doSomething)
        {
            this.doSomething = doSomething;
        }

        public Node.Status Process()
        {
            doSomething();
            return Node.Status.Success;
        }
    }

    //If the condition is true, success node so it can move to the next one
    public class Condition : IStrategy
    {
        readonly Func<bool> predicate;

        public Condition(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public Node.Status Process()
        {
            var result = predicate();

            Debug.Log($"Condition evaluated: {result}");

            return result ? Node.Status.Success : Node.Status.Failure;
        }


    }

    //Do this action
    public class PatrolStrategy : IStrategy
    {
        readonly Transform entity;
        readonly NavMeshAgent agent;
        readonly List<Transform> patrolPoints;
        readonly float patrolSpeed;
        int currentIndex;
        bool isPathCalculed;

        public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
        {
            this.entity = entity;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
        }

        public Node.Status Process()
        {
            if (currentIndex == patrolPoints.Count) return Node.Status.Success;

            var target = patrolPoints[currentIndex];
            agent.SetDestination(target.position);
            entity.LookAt(target.position);

            if (isPathCalculed && agent.remainingDistance < 0.1f)
            {
                currentIndex++;
                isPathCalculed = false;
            }

            if (agent.pathPending)
            {
                isPathCalculed = true;
            }

            return Node.Status.Running;
        }

        public void Reset() => currentIndex = 0;
    }

    public class MoveToTarget : IStrategy
    {
        readonly Transform entity;
        readonly NavMeshAgent agent;
        readonly Transform target;
        bool isPathCalculated;

        public MoveToTarget(Transform entity, NavMeshAgent agent, Transform target)
        {
            this.entity = entity;
            this.agent = agent;
            this.target = target;
        }

        public Node.Status Process()
        {
            if (Vector3.Distance(entity.position, target.position) < 1f)
            {
                return Node.Status.Success;
            }

            agent.SetDestination(target.position);
            entity.LookAt(target.position);

            if (agent.pathPending)
            {
                isPathCalculated = true;
            }
            return Node.Status.Running;
        }

        public void Reset() => isPathCalculated = false;
    }

    public class MoveToTargetSafe : IStrategy
    {
        readonly Transform entity;
        readonly NavMeshAgent agent;
        readonly Transform target;
        bool condition;
        bool isPathCalculated;

        public MoveToTargetSafe(Transform entity, NavMeshAgent agent, Transform target, bool condition)
        {
            this.entity = entity;
            this.agent = agent;
            this.target = target;
            this.condition = condition;
        }

        public Node.Status Process()
        {
          
            if (Vector3.Distance(entity.position, target.position) < 1f)
            {
                return Node.Status.Success;
            }

            agent.SetDestination(target.position);
            entity.LookAt(target.position);

            if (agent.pathPending)
            {
                isPathCalculated = true;
            }
            return Node.Status.Running;
        }

        public void Reset() => isPathCalculated = false;
    }
}

//using System;
//using System.Collections.Generic;
//using UnityEditor.Rendering;
//using UnityEngine;
//using UnityEngine.AI;

//namespace BehaviourTrees
//{
//    public interface IStrategy
//    {
//        Node.Status Process();

//        void Reset()
//        {
//            // Noop
//        }
//    }

//    public class ActionStrategy : IStrategy
//    {
//        readonly Action doSomething;

//        public ActionStrategy(Action doSomething)
//        {
//            this.doSomething = doSomething;
//        }

//        public Node.Status Process()
//        {
//            doSomething();
//            return Node.Status.Success;
//        }
//    }

//    public class Condition : IStrategy
//    {
//        readonly Func<bool> predicate;

//        public Condition(Func<bool> predicate)
//        {
//            this.predicate = predicate;
//        }

//        public Node.Status Process() => predicate() ? Node.Status.Success : Node.Status.Failure;
//    }

//    public class PatrolStrategy : IStrategy
//    {
//        readonly Transform entity;
//        readonly NavMeshAgent agent;
//        readonly List<Transform> patrolPoints;
//        readonly float patrolSpeed;
//        int currentIndex;
//        bool isPathCalculated;

//        public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
//        {
//            this.entity = entity;
//            this.agent = agent;
//            this.patrolPoints = patrolPoints;
//            this.patrolSpeed = patrolSpeed;
//        }

//        public Node.Status Process()
//        {
//            if (currentIndex == patrolPoints.Count) return Node.Status.Success;

//            var target = patrolPoints[currentIndex];
//            agent.SetDestination(target.position);
//            entity.LookAt(target.position);

//            if (isPathCalculated && agent.remainingDistance < 0.1f)
//            {
//                currentIndex++;
//                isPathCalculated = false;
//            }

//            if (agent.pathPending)
//            {
//                isPathCalculated = true;
//            }

//            return Node.Status.Running;
//        }

//        public void Reset() => currentIndex = 0;
//    }

//    public class MoveToTarget : IStrategy
//    {
//        readonly Transform entity;
//        readonly NavMeshAgent agent;
//        readonly Transform target;
//        bool isPathCalculated;

//        public MoveToTarget(Transform entity, NavMeshAgent agent, Transform target)
//        {
//            this.entity = entity;
//            this.agent = agent;
//            this.target = target;
//        }

//        public Node.Status Process()
//        {
//            if (Vector3.Distance(entity.position, target.position) < 1f)
//            {
//                return Node.Status.Success;
//            }

//            agent.SetDestination(target.position);
//            entity.LookAt(target.position);

//            if (agent.pathPending)
//            {
//                isPathCalculated = true;
//            }
//            return Node.Status.Running;
//        }

//        public void Reset() => isPathCalculated = false;
//    }
//}

