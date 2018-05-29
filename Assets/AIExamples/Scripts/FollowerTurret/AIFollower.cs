using UnityEngine;
using TMPro;
namespace B2BGAI
{

    public enum AIFollowerTurretState { Idle, Following, Alert, Spoted }

    public class AIFollower : MonoBehaviour
    {
        [SerializeField]
        TextMeshPro _text;
        [SerializeField]
        private Transform objective;
        [SerializeField]
        private float rotationSpeed = 2.0f;
        [SerializeField]
        private float viewDistance = 20f;
        [SerializeField]
        private float viewAngle = 30f;
        [SerializeField]
        private float alertStatusTime = 5f;
        [SerializeField]
        private float spotedTime = 0.5f;

        private float timer = 0f;
        private AIFollowerTurretState _state = AIFollowerTurretState.Idle;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 vDirection = objective.position - transform.position;
            // DEBUG
            Vector3 maxDistance = transform.position + transform.forward * viewDistance;
            Debug.DrawLine(transform.position, maxDistance, Color.blue);
            //
            float diffAngle = Vector3.Angle(vDirection, transform.forward);
            CalculateStatus(vDirection, diffAngle);
            

        }

        protected void CalculateStatus(Vector3 vDirection, float diffAngle)
        {
            if (vDirection.magnitude < viewDistance && diffAngle < viewAngle)
            {
                if (_state == AIFollowerTurretState.Idle)
                {
                    if (_text) _text.text = "Spoted";
                    timer = 0f;
                    _state = AIFollowerTurretState.Spoted;
                }
                if (_state == AIFollowerTurretState.Spoted)
                {
                    timer += Time.deltaTime;
                    if (timer > spotedTime) {
                        if (_text) _text.text = "Following";
                        _state = AIFollowerTurretState.Following;
                    } 
                }
                if (_state == AIFollowerTurretState.Following)
                {
                    // Restrict rotation
                    vDirection.y = 0;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vDirection), Time.deltaTime * rotationSpeed);
                }
                if (_state == AIFollowerTurretState.Alert)
                {
                    if (_text) _text.text = "Following";
                    _state = AIFollowerTurretState.Following;
                }



            }
            else
            {
                if (_state == AIFollowerTurretState.Following)
                {
                    timer = 0;
                    if (_text) _text.text = "Alerted";
                    _state = AIFollowerTurretState.Alert;
                }
                if (_state == AIFollowerTurretState.Alert)
                {
                    timer += Time.deltaTime;
                    if (timer > alertStatusTime) {
                        if (_text) _text.text = "Follower Turret";
                        _state = AIFollowerTurretState.Idle;
                    } 
                }

            }
        }



    }
}


