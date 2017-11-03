using UnityEngine;
using System.Collections;

public class swarm : MonoBehaviour {
	public Animator animator;

	public const int STATE_IDLE = 0;
	public const int STATE_APPROACH = 1;
	public const int STATE_BACK = 2;
	public const int FISH_VIEW = 2;

	private int fishState;
	private GameObject man;
	private GameObject plant1;
	private GameObject terrain;
	private bool manIn;//主人公是否来过
	Quaternion R;
	Quaternion M;

	private GameObject group1;

	//测试
	string show;
	int j;
	// Use this for initialization
	void Start () {
		manIn = false;
		plant1 = GameObject.Find ("Plant1");
		man = GameObject.Find ("Man");
		terrain = GameObject.Find ("Terrain");
		R = Quaternion.Euler (new Vector3 (0, 90, 0));
		group1 = GameObject.Find ("Group1");
	}

	// Update is called once per frame
	void Update () {

		dist();
		//if (Vector3.Distance (transform.position, terrain.transform.position) < FISH_VIEW/2) {
		//	transform.LookAt (plant1.transform);
			//transform.Rotate(new Vector3(0,90,0)); 
		//}
		if (Vector3.Distance (transform.position, man.transform.position) < 2*FISH_VIEW) {
			animator.SetBool ("approach2idle", false);
			animator.SetBool ("idle2approach", true);
			transform.LookAt (man.transform);
			fishState = STATE_APPROACH;
			manIn = true;


		} else {
			if (manIn == false) {
				animator.SetBool ("idle2approach", false);
				animator.SetBool ("approach2idle", true);
				fishState = STATE_IDLE;

			} else {
				if (Vector3.Distance (transform.position, plant1.transform.position) > FISH_VIEW) {
					animator.SetBool ("idle2approach", false);
					animator.SetBool ("approach2idle", true);
					transform.LookAt (plant1.transform);
					fishState = STATE_BACK;
				} else {
					//进行方向调整
					transform.rotation = Quaternion.Slerp (transform.rotation, R, 1);

					//transform.Rotate (0, 90, 0);
					manIn = false;
				}
			}
		}

		switch (fishState) {
		case STATE_IDLE:
			//正常情况下绕围绕植物
			transform.RotateAround (plant1.transform.position, -Vector3.up, 2f);
			break;
		case STATE_APPROACH:
			//当主人公小于一倍鱼视距，鱼远离
			if (Vector3.Distance (transform.position, man.transform.position) < FISH_VIEW) {

				transform.Translate (Vector3.forward * Time.deltaTime * -1);
			} 
			//当主人公小于2倍鱼视距大于1倍鱼视距，鱼围绕主人公
			else {
				transform.RotateAround (man.transform.position, -Vector3.up, 2f);
			}
			break;
		case STATE_BACK:
	
			transform.Translate (Vector3.forward * Time.deltaTime);
			break;
		}
	}

	void dist(){
		for (int i = 0; i < group1.transform.childCount; i++) {
			GameObject g1 = group1.transform.GetChild (i).gameObject;
			if (g1 != plant1 && Vector3.Distance (transform.position, g1.transform.position) < FISH_VIEW/2) {
				transform.Translate ((transform.position - g1.transform.position) * Time.deltaTime);
				//	j += 1;
				//		show ="gg"+j;
				//	GUI.Label(new Rect(100,0,300,40),show);
			}
		}
	}
}
