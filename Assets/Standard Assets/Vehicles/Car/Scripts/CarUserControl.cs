using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
		public GameObject car;
		public GameObject inters;
		float dist_pre = 999999f;
		bool isTurn=false;
		bool turnDone=false;
        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
			inters[0] = GameObject.Find("Inter1");
			inters[1] = GameObject.Find("Inter2");
			car = GameObject.Find("Car");
        }


        private void FixedUpdate()
        {
			
			float h = 0;// am re phai, duong re trai
			float v = 1;
//			float h = CrossPlatformInputManager.GetAxis("Horizontal");
//			float v = CrossPlatformInputManager.GetAxis("Vertical");
			Vector3 carPosition = car.transform.position;
			Vector3 interPosition = inters.transform.position;
			float dist = Vector3.Distance(carPosition, interPosition);

//			print("Distance to other: " + dist);
			print (m_Car.CurrentSpeed +" h: " + h + " v: "+ v +"distance: "+ dist + " m_Car.transform.eulerAngles: "+ m_Car.transform.eulerAngles.y);
//			print (car.transform.position.x);
			if (dist<=23f && (turnDone == false)) {
				v = 0;
				h = 0.4f;
				isTurn = true;

			} 
			if (dist>(dist_pre) && dist>20f && isTurn) {
				m_Car.transform.eulerAngles=  (new Vector3 (0, 90, 0));
				v = 1;
				h = 0;
				turnDone = true;
//				isTurn = false;
			}

			dist_pre = dist;
            // pass the input to the car!
           
			print (h);
//			print (v);
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
