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
		float h = 0;// am re phai, duong re trai
		float v = 1;

		bool huong_z=true;
		int len=-1;
		public Dropdown m_Dropdown;
		bool flag_inter=false;
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
		private Vector3[] interPositions= new Vector3[20]; 
		int index=0;
		Vector3 carPosition;
		int[] checkpoints = new int[20];
		float dist;
		private void Awake()
		{
			ShowOverView ();
			ShowMain ();

			m_Car = GetComponent<CarController>();

			//			inters.Add (inter);
			//			inters[1] = GameObject.Find("Inter2");
			car = GameObject.Find("Car");



		}
			
		private void FixedUpdate()
		{
			if (m_Dropdown.value != 0) {
				Destroy (m_Dropdown);
				StartCoroutine(GetMap(0,m_Dropdown.value));

				if(len!=-1)	{
	//					print ("checkpoints length: " + checkpoints.Length);
					if (flag_inter == false) {
						//					
						//					inters = GameObject.FindGameObjectsWithTag ("inter");
						//					index = inters.Length - 1;
						//					flag_inter = true;
						//					for (int i=0;i<inters.Length;++i) {
						//						 = inters[i].transform.position;
						//						print(inters[i].transform.position);
						//					}
					for(int i=0;i<len;i++){
	//						print(checkpoints[i]);
						String name = "Inter" + checkpoints[i];
						GameObject point = GameObject.Find (name);
//						print ("point "+point.transform.position);
						interPositions[i]=point.transform.position;
	//						print (point.transform.position);
						point.gameObject.tag = "inter";
							
						}
					}
					if (flag_chuong_ngai_vat == true) {
						
						timeLeft -= Time.deltaTime;
						timer.GetComponent<Text> ().text = " " + timeLeft.ToString ();
					} else {
						timer.GetComponent<Text> ().text = " ";
					}
					ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (timeLeft < 0) {
						GameObject cnt = GameObject.Find ("chuong_ngai_vat(Clone)");
						print ("destroy");
						Destroy (cnt);
						timeLeft = 10.0f;
						flag_chuong_ngai_vat = false;
					}
					if (Physics.Raycast (ray, out hit)) {

						if (Input.GetKey (KeyCode.Mouse0) && flag_chuong_ngai_vat == false) {
							GameObject obj = Instantiate (chuong_ngai_vat, new Vector3 (hit.point.x, hit.point.y, hit.point.z), Quaternion.identity) as GameObject;
							flag_chuong_ngai_vat = true;

						}

					}


					//			float h = CrossPlatformInputManager.GetAxis("Horizontal");
					//			float v = CrossPlatformInputManager.GetAxis("Vertical");
					carPosition = car.transform.position;


					dist = Vector3.Distance (carPosition, interPositions [index]);
//					print (index + " : " + interPositions [index]);
					float stop_distance=Vector3.Distance (carPosition, interPositions [len-1]);
//					if (car.transform.position.x > interPositions[len-1].x &&  car.transform.position.y > interPositions[len-1].y 
//						&& car.transform.position.z > interPositions[len-1].z) {
//						Time.timeScale = 0;
//					}
					if (stop_distance < 15) {
						Time.timeScale = 0;
					}
					logicDirection (interPositions [index], interPositions [index + 1]);
				
//					if (interPositions [index].x < interPositions [index + 1].x) {//turn right x
//						if (dist <= 23f && (turnDones [index] == false)) {
//							v = 0;
//							h = 0.36f;
//							isTurns [index] = true;
//							print ("dang re");
//						} 
//						if (dist > (dist_pre) && dist > 20f && isTurns [index]) {
//							m_Car.transform.eulerAngles = (new Vector3 (0, 80, 0));
//							v = 1;
//							h = 0;
//							print ("turn right x: " + index);
//							turnDones [index] = true;
//							index++;
//
//							dist_pre = 999999f;
//							print ("turnDone");
//							//				isTurn = false;
//						}
//					} else if (interPositions [index].x > interPositions [index + 1].x) {// turn left x
//						if (dist <= 23f && (turnDones [index] == false)) {
//							v = 0;
//							h = -0.36f;
//							isTurns [index] = true;
//							print ("dang re");
//						} 
//						if (dist > (dist_pre) && dist > 20f && isTurns [index]) {
//							m_Car.transform.eulerAngles = (new Vector3 (0, -90, 0));
//							v = 1;
//							h = 0;
//							print ("turn left x: " + index);
//							turnDones [index] = true;
//							index++;
//							if (index < 0) {
//	//									Time.timeScale = 0;
//							}
//							dist_pre = 999999f;
//							print ("turnDone");
//							//				isTurn = false;
//						}
//					} else if (interPositions [index].z < interPositions [index + 1].z) {// turn left z
//						if (dist <= 23f && (turnDones [index] == false)) {
//							v = 0;
//							h = -0.36f;
//							isTurns [index] = true;
//							print ("dang re");
//						} 
//						if (dist > (dist_pre) && dist > 20f && isTurns [index]) {
//							m_Car.transform.eulerAngles = (new Vector3 (0, 27, 0));
//							v = 1;
//							h = 0;
//							print ("turn left z: " + index);
//							turnDones [index] = true;
//							index++;
//							if (index < 0) {
//								Time.timeScale = 0;
//							}
//							dist_pre = 999999f;
//							print ("turnDone");
//							//				isTurn = false;
//						}
//					} else if (interPositions [index].z > interPositions [index +1 ].z) {// turn right z
//						if (dist <= 23f && (turnDones [index] == false)) {
//							v = 0;
//							h = 0.36f;
//							isTurns [index] = true;
//							print ("dang re");
//						} 
//						if (dist > (dist_pre) && dist > 20f && isTurns [index]) {
//							m_Car.transform.eulerAngles = (new Vector3 (0, 170, 0));
//							v = 1;
//							h = 0;
//							print ("turn right z: " + index);
//							turnDones [index] = true;
//							index++;
//							if (index < 0) {
//								Time.timeScale = 0;
//							}
//							dist_pre = 999999f;
//							print ("turnDone");
//							//				isTurn = false;
//						}
//					}


					dist_pre = dist;
					// pass the input to the car!

					//			print (h);
					//			print (v);
					#if !MOBILE_INPUT
					float handbrake = CrossPlatformInputManager.GetAxis ("Jump");
					m_Car.Move (h, v, v, handbrake);
					#else
					m_Car.Move(h, v, v, 0f);
					#endif
					}
			}
				
		}

		private void ShowOverView() {
			overview_camera.enabled = true;
			main_camera.enabled = false;
		}

		private void ShowMain() {
			overview_camera.enabled = false;
			main_camera.enabled = true;
		}

		private void logicDirection(Vector3 current,Vector3 next){
//			print (car.transform.eulerAngles);
			if (huong_z == true) {// dang di theo huong z
				if (current.z < next.z) {// tiep tuc di thang
					v = 1;
					h = 0;
					index++;
					print ("turn done" + index);
				}

				if (current.x < next.x) { // re phai theo chieu z
					
					if (dist <= 23f && (turnDones [index] == false)) {
						v = 0;
						h = 0.36f;
						isTurns [index] = true;
						print ("dang re");
					} 
					if (car.transform.eulerAngles.y>80) {
//						m_Car.transform.eulerAngles = (new Vector3 (0, 80, 0));
						v = 1;
						h = 0;
						print ("re phai theo chieu z: " + index);
						turnDones [index] = true;
						index++;

						dist_pre = 999999f;
						print ("turnDone" +index);
						huong_z = false;
						//				isTurn = false;
					}
				}
				// chua test
				if (current.x > next.x) { // re trai theo chieu z
					
					if (dist <= 23f && (turnDones [index] == false)) {
						v = 0;
						h = -0.36f;
						isTurns [index] = true;
						print ("dang re");
					} 
//					if (dist > (dist_pre) && dist > 20f && isTurns [index]) {
					if(m_Car.transform.eulerAngles.y>90){
//						m_Car.transform.eulerAngles = (new Vector3 (0, -90, 0));
						v = 1;
						h = 0;
						print ("turn left x: " + index);
						turnDones [index] = true;
						index++;
						if (index < 0) {
//									Time.timeScale = 0;
						}
						dist_pre = 999999f;
						print ("turnDone");
						huong_z = false;
						//				isTurn = false;
					}
				}

			} 
			if(huong_z == false){// huong theo chieu x
				print("theo chieu x");
				if ((current.x + 99)  < next.x) {// tiep tuc di thang
					print("di thang x");
					v = 1;
					h = 0;
					index++;
					print ("turn done" + index);
				}

				if (current.z > next.z) {// re phai theo x
					print("re phai theo chieu x");

					if (dist <= 23f && (turnDones [index] == false)) {
						v = 0;
						h = 0.36f;
						isTurns [index] = true;
						print ("dang re");
					} 
					if (car.transform.eulerAngles.y>170) {
//						m_Car.transform.eulerAngles = (new Vector3 (0, 170, 0));
						v = 1;
						h = 0;
						print ("turn right z: " + index);
						turnDones [index] = true;
						index++;
						if (index < 0) {
							Time.timeScale = 0;
						}
						dist_pre = 999999f;
						print ("turnDone" + index);
						huong_z=true;
						//				isTurn = false;
					}
				}
//
				if (current.z < next.z) {// re trai theo x
					print("re trai x");

					if (dist <= 23f && (turnDones [index] == false)) {
						v = 0;
						h = -0.36f;
						isTurns [index] = true;
//						print ("dang re");
					} 
//					if (dist > (dist_pre) && dist > 20f && isTurns [index]) {
					if(car.transform.eulerAngles.y<13){
//						m_Car.transform.rotation = ( Quaternion.Euler (0, 45, 0));

						Wait ();
						v = 1;
						h = 0;
						print ("turn left z: " + index);
						turnDones [index] = true;
						index++;
						if (index < 0) {
							Time.timeScale = 0;
						}
						dist_pre = 999999f;
						print ("turnDone" + index);
						huong_z=true;
						//				isTurn = false;
					}
				}

			}

		}

		IEnumerator GetMap(int start,int target)
		{
			String url = "http://127.0.0.1:3000/" + start + "/" + target;
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
					print ("checkpoints: " + str);
					String[] trace = str.Split(","[0]);
					len = trace.Length;
					for(int i= 0; i < trace.Length; i++){
//						print (trace [i]);
						checkpoints[i]= System.Int32.Parse(trace[i]);
//						Debug.Log (checkpoint [i]);
					}
				}
			}
		}

		IEnumerator Wait(){
			yield return new WaitForSeconds (5f);
			print ("wait");
		}
	}
}