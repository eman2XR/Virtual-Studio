using UnityEngine;
using System.Collections;

	public class Rotate : MonoBehaviour {

		public float speed;

        void Start()
        {
        }
        
        // Update is called once per frame
        void FixedUpdate () 
		{
			this.transform.Rotate (0, speed * Time.deltaTime, 0);
           // this.transform.Translate(Vector3.up * Time.deltaTime * 0.01f);
		}
	}

