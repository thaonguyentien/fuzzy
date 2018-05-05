using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
using System.Collections;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof (CarController))]
	public class CarUserControl : MonoBehaviour
	{
		private CarController m_Car; // the car controller we want to use
		public GameObject car;
		//		public GameObject inter;
		float dist_pre = 999999f;
		bool[] isTurns=new bool[20];
		bool[] turnDones=new bool[20];
		public GameObject[] inters= new GameObject[20];
		private Vector3[] interPositions = new Vector3[20]; 
		int index;
		Vector3 carPosition;
		private void Awake()
		{
			StartCoroutine(GetText());
			// get the car controller
			m_Car = GetComponent<CarController>();
			inters= GameObject.FindGameObjectsWithTag("inter");
			index=inters.Length-1;
			//			inters.Add (inter);
			//			inters[1] = GameObject.Find("Inter2");
			car = GameObject.Find("Car");
			for (int i=0;i<inters.Length;++i) {
				interPositions[i] = inters[i].transform.position;
				//				print(interPositions[i]);
			}


		}


		private void FixedUpdate()
		{

			float h = 0;// am re phai, duong re trai
			float v = 1;
			//			float h = CrossPlatformInputManager.GetAxis("Horizontal");
			//			float v = CrossPlatformInputManager.GetAxis("Vertical");
			carPosition = car.transform.position;


			float dist = Vector3.Distance(carPosition, interPositions[index]);
			print (index+" : " +interPositions[index]);

			//			print("Distance to other: " + dist);
			//			print (" h: " + h + " v: "+ v +"distance: "+ dist + " pre_dist:" + dist_pre + " isTurn " + isTurns[index] );
			//			print (car.transform.position.x);
			//			print("dist: " +dist + "turnDones: "+  turnDones[index]); 

			if (interPositions [index].x < interPositions [index - 1].x) {//turn right x
				if (dist<=23f && (turnDones[index] == false)) {
					v = 0;
					h = 0.36f;
					isTurns[index] = true;
					print ("dang re");
				} 
				if (dist > (dist_pre) && dist > 20f && isTurns [index]) {
					m_Car.transform.eulerAngles = (new Vector3 (0, 80, 0));
					v = 1;
					h = 0;
					print ("turn right x: " + index);
					turnDones [index] = true;
					index--;
					if (index < 0) {
						Time.timeScale = 0;
					}
					dist_pre = 999999f;
					print ("turnDone");
					//				isTurn = false;
				}
			} else if (interPositions [index].x > interPositions [index - 1].x){// turn left x
				if (dist<=23f && (turnDones[index] == false)) {
					v = 0;
					h = -0.36f;
					isTurns[index] = true;
					print ("dang re");
				} 
				if (dist > (dist_pre) && dist > 20f && isTurns [index]) {
					m_Car.transform.eulerAngles = (new Vector3 (0, -90, 0));
					v = 1;
					h = 0;
					print ("turn left x: " + index);
					turnDones [index] = true;
					index--;
					if (index < 0) {
						Time.timeScale = 0;
					}
					dist_pre = 999999f;
					print ("turnDone");
					//				isTurn = false;
				}
			}else if (interPositions [index].z < interPositions [index - 1].z){// turn left z
				if (dist<=23f && (turnDones[index] == false)) {
					v = 0;
					h = -0.36f;
					isTurns[index] = true;
					print ("dang re");
				} 
				if (dist > (dist_pre) && dist > 20f && isTurns [index]) {
					m_Car.transform.eulerAngles = (new Vector3 (0, 27, 0));
					v = 1;
					h = 0;
					print ("turn left z: " + index);
					turnDones [index] = true;
					index--;
					if (index < 0) {
						Time.timeScale = 0;
					}
					dist_pre = 999999f;
					print ("turnDone");
					//				isTurn = false;
				}
			}else if (interPositions [index].z > interPositions [index - 1].z){// turn right z
				if (dist<=23f && (turnDones[index] == false)) {
					v = 0;
					h = 0.36f;
					isTurns[index] = true;
					print ("dang re");
				} 
				if (dist > (dist_pre) && dist > 20f && isTurns [index]) {
					m_Car.transform.eulerAngles = (new Vector3 (0, 170, 0));
					v = 1;
					h = 0;
					print ("turn right z: " + index);
					turnDones [index] = true;
					index--;
					if (index < 0) {
						Time.timeScale = 0;
					}
					dist_pre = 999999f;
					print ("turnDone");
					//				isTurn = false;
				}
			}


			dist_pre = dist;
			// pass the input to the car!

			//			print (h);
			//			print (v);
			#if !MOBILE_INPUT
			float handbrake = CrossPlatformInputManager.GetAxis("Jump");
			m_Car.Move(h, v, v, handbrake);
			#else
			m_Car.Move(h, v, v, 0f);
			#endif
		}
		IEnumerator GetText()
		{
			using (UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:3000"))
			{
				yield return www.Send();

				if (www.isNetworkError || www.isHttpError)
				{
					Debug.Log(www.error);
				}
				else
				{
					// Show results as text
					Debug.Log(www.downloadHandler.text);

					// Or retrieve results as binary data
					byte[] results = www.downloadHandler.data;
					print (results);
				}
			}
		}
	}
}