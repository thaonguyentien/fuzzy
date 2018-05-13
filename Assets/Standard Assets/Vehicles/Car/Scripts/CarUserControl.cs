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
		bool[] col = new bool[20];
		GameObject target,pre_target;
		float angle;
		GameObject a,b,c;
		bool flag_traffic=false;
		bool flag_traffic2=false;
		bool flag_getMap= true;
		int flag_handle=0,flag_handle2=0;
		float speed;
		float h = 0;// am re phai, duong re trai
		float v = 1;
		bool chieu_duong=true;
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
		GameObject obj;
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
		float time=0;
		string light_status;
		int time_light;
		Vector3 traffic = new Vector3 (5, 0, 100);
		Vector3 traffic2 = new Vector3 (100, 0, 400);
		private void Awake()
		{
			ShowOverView ();
			ShowMain ();
//			a=GameObject.Find("A");
//			b=GameObject.Find("B");
//			c=GameObject.Find("C");
//			Vector2 A = new Vector2 (a.transform.position.x, a.transform.position.z);
//			Vector2 B = new Vector2 (b.transform.position.x, b.transform.position.z);
//			Vector2 C = new Vector2 (c.transform.position.x, c.transform.position.z);
//			print (Vector2.Angle (B - A, B - C));
				
			//			print (light.);
			m_Car = GetComponent<CarController>();

			//			inters.Add (inter);
			//			inters[1] = GameObject.Find("Inter2");
			car = GameObject.Find("Car");

		}

		private void FixedUpdate()
		{
			
			time += Time.deltaTime;
			double adj_time = time;
			if (adj_time <= 20) {
				light_status = "GREEN";
				time_light = 20 -(int)Math.Ceiling(adj_time);
			} else if (adj_time <= 25) {
				light_status = "YELLOW";
				time_light =  5 - (int )Math.Ceiling(adj_time) + 20;
			} else {
				light_status = "RED";
				time_light = 10 -(int)Math.Ceiling(adj_time) + 25;
				if (time_light == 0) {
					time = 0;
				}
			}
			//			print ("timeL "+adj_time);
			timer.GetComponent<Text> ().text = " " + time_light.ToString ();
			if (m_Dropdown.value != 0) {
				Destroy (m_Dropdown);
				if (flag_getMap == true) {
					StartCoroutine(GetMap(0,m_Dropdown.value));
					flag_getMap = false;
				}



				if(len!=-1)	{
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

					if ((flag_handle%10)==0 && flag_traffic == false) {

						flag_handle = 0;
						float distance_to_traffic= (int)(Vector3.Distance (carPosition, traffic));

						StartCoroutine (GetSpeedTraffic (distance_to_traffic-2,light_status,time_light));
						if (speed > 30) {
							speed -= 20;
						}
						m_Car.SetSpeed (speed, huong_z);
						//						print("GetSpeedTraffic"+ speed);
						//							}
					}
					if (flag_traffic == false) {

						flag_handle++;
					}

					if (flag_traffic2 == false  ) {

						flag_handle2++;
					}
					if ((flag_handle2%10)==0 && flag_traffic2 == false && carPosition.x > 90 && carPosition.x <110 && carPosition.z > 300) {

						flag_handle2 = 0;
						float distance_to_traffic= (int)(Vector3.Distance (carPosition, traffic2));

						StartCoroutine (GetSpeedTraffic (distance_to_traffic,light_status,time_light));
						if (speed > 30) {
							speed -= 20;
						}
						m_Car.SetSpeed (speed, huong_z);
						//						print("GetSpeedTraffic"+ speed);
						//							}
					}

					if (isTurns[0]) {
						flag_traffic = true;
					}

					if (flag_chuong_ngai_vat == true) {
						flag_handle++;
						timeLeft -= Time.deltaTime;
						timer.GetComponent<Text> ().text = " " + timeLeft.ToString ();
						//						print ("getsppeed");
						//						print((timeLeft * 10) % 2);
						if ((flag_handle%10)==0) {
							flag_handle = 0;
							float distance_to_chuong_ngai_vat=(int)(Vector3.Distance (carPosition, obj.transform.position));
							print ("distance: "+   distance_to_chuong_ngai_vat  +"+ speed" + speed);
							//							if (flag_chuong_ngai_vat == false) {
							StartCoroutine (GetSpeed (distance_to_chuong_ngai_vat,"barrier"));
							m_Car.SetSpeed (speed, huong_z);
							//							}
						}


					} 
					//					else {
					//						timer.GetComponent<Text> ().text = " ";
					//					}
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
							obj = Instantiate (chuong_ngai_vat, new Vector3 (hit.point.x, hit.point.y, hit.point.z), Quaternion.identity) as GameObject;
							flag_chuong_ngai_vat = true;
							float distance_to_chuong_ngai_vat=(int)(Vector3.Distance (carPosition, obj.transform.position));

							StartCoroutine(GetSpeed(distance_to_chuong_ngai_vat,"barrier"));
							m_Car.SetSpeed(speed,huong_z);
							print ("set speed cho chuong ngai vat" + speed + " distance   " + distance_to_chuong_ngai_vat);
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


		IEnumerator GetSpeed(float distance,string type)
		{
			//			print ("getspped");
			String url = "http://127.0.0.1:3000/getSpeed/" + distance  ;
			//			print (url);
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

					str = str.Substring (2,str.Length-4);
					//					print ("checkpoints: " + str);
					//					String[] trace = str.Split(","[0]);
					//					len = trace.Length;
					//					for(int i= 0; i < trace.Length; i++){
					//						print (trace [i]);

					speed= float.Parse(str);
					//					print ("speed" + speed);
					//						Debug.Log (checkpoint [i]);
					//					}
				}
			}
		}

		IEnumerator GetSpeedTraffic(float distance,string light, int time)
		{
			//			print ("getspped");
			String url = "http://127.0.0.1:3000/getSpeedTraffic/" + distance + "/" + light +"/" + time  ;
			//			print (url);
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

					str = str.Substring (2,str.Length-4);
					//					print ("checkpoints: " + str);
					//					String[] trace = str.Split(","[0]);
					//					len = trace.Length;
					//					for(int i= 0; i < trace.Length; i++){
					//						print (trace [i]);

					speed= float.Parse(str);
					//					print ("speed" + speed);
					//						Debug.Log (checkpoint [i]);
					//					}
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

		void OnCollisionEnter(Collision other)
		{
			int next= Int32.Parse(other.transform.name) +1;
			string nextTarget = next.ToString ();
			if (col [next] == false) {
				col [next] = true;
				print ("va cham");
				Destroy (other.collider);
				pre_target = target;

				print (nextTarget);
				target = GameObject.Find (nextTarget);
				Vector2 B = new Vector2 (pre_target.transform.position.x, pre_target.transform.position.z);
				Vector2 A = new Vector2 (target.transform.position.x, target.transform.position.z);
				Vector2 C = new Vector2 (pre_target.transform.position.x + 10, pre_target.transform.position.z);
				angle = 90 - Vector2.Angle (B - A, B - C);
				print (A);
				print (B);
				print (C);
				print (angle);
				target = GameObject.Find ((next.ToString ()));
//				if (other.transform.name == "Node0") {
//					road = GameObject.Find ("Road2");
//				}
			}

		}


		private void logicDirection(Vector3 current,Vector3 next){
			//			print (car.transform.eulerAngles);
			//			print("logicDirection" + index) ;
			if (huong_z == true) {// dang di theo huong z
				if (current.z < next.z) {// tiep tuc di thang
					v = 1;
					h = 0;
					index++;
					print ("turn done" + index);
				}

				if (current.x < next.x && chieu_duong == true) { // re phai theo chieu z

					if (dist <= 23f && (turnDones [index] == false)) {
						v = 0;
						h = 0.36f;
						isTurns [index] = true;
						print ("dang re");
					} 
					if (car.transform.eulerAngles.y>80 && isTurns[index]==true) {
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

				if (current.x < next.x && chieu_duong == false) { // re phai theo chieu z

					if (dist <= 23f && (turnDones [index] == false)) {
						v = 0;
						h = -0.36f;
						isTurns [index] = true;
						print ("dang re");
					} 
					if (car.transform.eulerAngles.y<100 && isTurns[index]==true ) {
						//						m_Car.transform.eulerAngles = (new Vector3 (0, 80, 0));
						v = 1;
						h = 0;
						print (" chieu am :re phai theo chieu z: " + index);
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
					if(car.transform.eulerAngles.y>90 && isTurns[index]==true ){
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
					car.transform.eulerAngles = new Vector3(car.transform.eulerAngles.x,car.transform.eulerAngles.y - 2,car.transform.eulerAngles.z);

				}

				if (current.z > next.z) {// re phai theo x
					print("re phai theo chieu x");

					if (dist <= 23f && (turnDones [index] == false)) {
						v = 0;
						h = 0.36f;
						isTurns [index] = true;
						print ("dang re");
					} 
					if (car.transform.eulerAngles.y>170 && isTurns[index]==true ) {
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
						chieu_duong = false;
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
					if(car.transform.eulerAngles.y<13 && isTurns[index]==true ){
						//						m_Car.transform.rotation = ( Quaternion.Euler (0, 45, 0));
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