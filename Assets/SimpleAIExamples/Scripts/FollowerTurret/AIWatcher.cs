
using UnityEngine;

namespace B2BGAI
{
    public class AIWatcher : MonoBehaviour
    {
        [SerializeField]
        private Transform objective;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (objective)
            {
                transform.LookAt(objective);
            }
        }
    }

}

    
