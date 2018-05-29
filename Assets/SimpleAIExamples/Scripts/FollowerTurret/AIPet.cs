
using UnityEngine;

namespace B2BGAI {
    public class AIPet : MonoBehaviour
    {
        [SerializeField]
        private float speed = 2.0f;
        [SerializeField]
        private float accuracy = 5.0f;
        [SerializeField]
        private Transform objective;

        // Update is called once per frame
        void Update()
        {
            this.transform.LookAt(objective.position);
            Vector3 direction = objective.position - this.transform.position;
            if (direction.magnitude > accuracy)
                this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
    }
}


