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
		public GameObject inter;
		float dist_pre = 999999f;
		bool[] isTurns=new bool[20];
		bool[] turnDones=new bool[20];
		public GameObject[] inters= new GameObject[20];
		private Vector3[] interPositions = new Vector3[20]; 
		int index=1;
		private void Awake()
		{
			// get the car controller
			m_Car = GetComponent<CarController>();
			inters= GameObject.FindGameObjectsWithTag("inter");
			//			inters.Add (inter);
			//			inters[1] = GameObject.Find("Inter2");
			car = GameObject.Find("Car");
			interPositions[0] = inters[0].transform.position;
			interPositions[1] = inters[1].transform.position;

		}


		private void FixedUpdate()
		{

			float h = 0;// am re phai, duong re trai
			float v = 1;
			//			float h = CrossPlatformInputManager.GetAxis("Horizontal");
			//			float v = CrossPlatformInputManager.GetAxis("Vertical");
			Vector3 carPosition = car.transform.position;


			float dist = Vector3.Distance(carPosition, interPositions[index]);
			print ("index: "+interPositions[index]);
			//			print("Distance to other: " + dist);
//			print (m_Car.CurrentSpeed +" h: " + h + " v: "+ v +"distance: "+ dist + " m_Car.transform.eulerAngles: "+ m_Car.transform.eulerAngles.y);
			//			print (car.transform.position.x);
			if (dist<=23f && (turnDones[index] == false)) {
				v = 0;
				h = 0.4f;
				isTurns[index] = true;

			} 
			if (dist>(dist_pre) && dist>20f && isTurns[index]) {
				m_Car.transform.eulerAngles=  (new Vector3 (0, 90, 0));
				v = 1;
				h = 0;
				print ("turn: " + index);
				turnDones[index] = true;
				index--;
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
