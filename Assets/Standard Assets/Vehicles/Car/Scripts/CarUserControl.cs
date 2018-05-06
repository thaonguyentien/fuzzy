using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof (CarController))]
	public class CarUserControl : MonoBehaviour
	{

		bool flag_chuong_ngai_vat=false;
		Ray ray;
		RaycastHit hit;
		public GameObject chuong_ngai_vat;
		public GameObject timer;
		float timeLeft=10.0f;

		public Camera overview_camera;
		public Camera main_camera;
		private CarController m_Car; // the car controller we want to use
		public GameObject car;
		public GameObject[] inters = new GameObject[20];
		float dist_pre = 999999f;
		bool[] isTurns=new bool[20];
		bool[] turnDones=new bool[20];
		private Vector3[] interPositions = new Vector3[20]; 
		int index;
		Vector3 carPosition;
		int[] checkpoint = new int[20];
		private void Awake()
		{
			
			ShowOverView ();
			ShowMain ();
			StartCoroutine(GetText(0,4));
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
			if (flag_chuong_ngai_vat == true) {
				
				timeLeft -= Time.deltaTime;
				timer.GetComponent<Text> ().text = " " + timeLeft.ToString();
			} else {
				timer.GetComponent<Text> ().text = " ";
			}
			ray=Camera.main.ScreenPointToRay(Input.mousePosition);
			if (timeLeft < 0) {
				GameObject cnt = GameObject.Find ("chuong_ngai_vat(Clone)");
				print("destroy");
				Destroy (cnt);
				timeLeft = 10.0f;
				flag_chuong_ngai_vat = false;
			}
			if(Physics.Raycast(ray,out hit))
			{

				if(Input.GetKey(KeyCode.Mouse0) && flag_chuong_ngai_vat==false)
				{
					GameObject obj=Instantiate(chuong_ngai_vat,new Vector3(hit.point.x,hit.point.y,hit.point.z), Quaternion.identity) as GameObject;
					flag_chuong_ngai_vat = true;

				}

			}

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

		private void ShowOverView() {
			overview_camera.enabled = true;
			main_camera.enabled = false;
		}

		private void ShowMain() {
			overview_camera.enabled = false;
			main_camera.enabled = true;
		}

		IEnumerator GetText(int start,int target)
		{
			String url = "http://127.0.0.1:3000/:" + start + "/:" + target;
			using (UnityWebRequest www = UnityWebRequest.Get(url))
			{
				yield return www.Send();

				if (www.isNetworkError || www.isHttpError)
				{
					Debug.Log(www.error);
				}
				else
				{
					// Show results as text
//					Debug.Log(www.downloadHandler.text);

					// Or retrieve results as binary data
					byte[] results = www.downloadHandler.data;
//					print ("huong" + results[1]);
					String str = System.Text.Encoding.Default.GetString(results);

					str = str.Substring (1,str.Length-2);
//					print ("res: " + str);
					String[] trace = str.Split(","[0]);
					for(int i= 0; i < trace.Length; i++){
//						print (trace [i]);
						checkpoint[i]= System.Int32.Parse(trace[i]);
//						Debug.Log (checkpoint [i]);
					}
				}
			}
		}
	}
}