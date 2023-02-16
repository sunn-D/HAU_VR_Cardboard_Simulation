using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HAU_VR_Cardboard.Scripts.NPC_Controller
{
    public class NPCMoving : NPCBase
    {
        //
        [field: FoldoutGroup("Path"), SerializeField] public float TimeIdle { get; set; }
        [field: FoldoutGroup("Path"), SerializeField] public float Speed { get; set; }
        [field: FoldoutGroup("Path"), SerializeField] public List<Transform> Points { get; set; }
        
        //
        private readonly int SpeedHash = Animator.StringToHash("Speed_f");
        
        //
        private List<Vector3> _positionPoints = new List<Vector3>(); 
        private Vector3 _nextPoint;
        private float _currentIdleTime;
        private int _nextPointIndex;
        private bool _setState;
        
        //
        private void Start()
        {
            foreach (var point in Points)
            {
                _positionPoints.Add(new Vector3(point.position.x, 0f, point.position.z));
            }
            _currentIdleTime = TimeIdle;
            _nextPoint = _positionPoints[1];
            _nextPointIndex = 1;
            transform.position = _positionPoints[0];
            NPCAnimator.SetFloat(SpeedHash, 0f);
        }
        
        //
        private void Update()
        {
            if (_currentIdleTime > 0)
            {
                _currentIdleTime -= Time.deltaTime;
            }
            else
            {
                if (transform.position == _nextPoint)
                {
                    _nextPointIndex++;
                    if (_nextPointIndex >= _positionPoints.Count) _nextPointIndex = 0;
                    _nextPoint = _positionPoints[_nextPointIndex];
                    _currentIdleTime = TimeIdle;
                    NPCAnimator.SetFloat(SpeedHash, 0f);
                    State = NPCState.Idle;
                    _setState = false;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _nextPoint, Speed * Time.deltaTime);
                    if (!_setState)
                    {
                        _setState = true;
                        State = NPCState.Moving;
                        NPCAnimator.SetFloat(SpeedHash, .5f);
                    }
                }
            }

            if (State == NPCState.Idle)
            {
                var degreesPerSecond = 90 * Time.deltaTime;
                var direction = _nextPoint - transform.position;
                var targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, degreesPerSecond);
            }
        }
    }
}