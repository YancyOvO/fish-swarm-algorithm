using UnityEngine;
using System.Collections;

public class FishBehavior : MonoBehaviour {


	public int groupId = 0;
	public float moveSpeed=2.0f,lowSpeed = 0.3f, highSpeed = 2.1f,rotateSpeed=15;//移动旋转速度,最低时速，最高时速
	private Quaternion elevationAngle; //向上或向下转动角度（四元数）
	private Quaternion rotationAngle;  //向左或向右转动角度
	private float limitAngle = 20.0f;//旋转角度限定
	private float target = - 0.2f;//偏移值

	public Vector3 position{
		get{return transform.position;}
	}

	public Vector3 movement{
		get{return myMovement;}
	}

	private Vector3 myMovement=Vector3.zero;
	private FishGroup myGroup;
	private Vector3 man1; //人的position


	private float currentSpeed;//当前速度
	private float speed = 0;//初始速度（其实没用，本来打算鱼从速度0加速到移动速度moveSpeed）

	public void SetGroup(int index){
		myGroup=FishGroup.GetFishGroup(index);
	}

	// Use this for initialization
	void Start () {
		SetGroup(groupId);//按照ID放入所有的组进入LIST
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 displacement = myGroup.targetPosition - position;//得到指向食物的方向
		Vector3 direction;

		if ((position - myGroup.targetPosition).magnitude < myGroup.minCorach) {//如果鱼离食物太近
			direction = displacement.normalized * myGroup.targetWeightAway;//鱼群大方向不再指向食物，而是偏移食物
		} 
		else {
			direction = displacement.normalized * myGroup.targetWeightCroach;//给食物方向添加权值
		}
		speed = moveSpeed;
		direction += GetGroupPosition ();//指向食物方向加上其他鱼的影响
		direction += GetManPosition ();//再加上人（障碍物）给鱼的影响
		Drive (direction, speed);
	}

	private Vector3 GetManPosition(){

		Vector3 dis1= Vector3.zero, v = Vector3.zero;
		Collider[] m = Physics.OverlapSphere (position, myGroup.keepManDistance, myGroup.mask2);	
		man1 = ManUnderWater.getPosition;
		for (int i = 0; i < m.Length; i++) {
				
				dis1 = position - man1;
				v = dis1.normalized;
				Debug.DrawLine (position, man1, Color.yellow);

		}
	
		return v * myGroup.keepManWeight;
	}

	private Vector3 GetGroupPosition(){
		Collider[] c = Physics.OverlapSphere (position, myGroup.keepDistance, myGroup.mask);//查找周围同群种的鱼
		Vector3 dis= Vector3.zero, v = Vector3.zero;
		for ( int i = 0; i < c.Length; i++) {
				FishBehavior otherFish = c [i].GetComponent<FishBehavior> ();
			dis = (position - otherFish.position).normalized;
			if ((position.y - myGroup.targetPosition.y) > myGroup.keepY||(position.y - myGroup.targetPosition.y) < -myGroup.keepY)//限定鱼的Y轴在一定范围以内
				v = target*dis;
			else
				v = dis;
		//	v = dis.normalized;	
				Debug.DrawLine(position, otherFish.position, Color.yellow);
			}
		return v*myGroup.keepWeight;
	}

	private void Drive(Vector3 direction, float spd){
		Vector3 finialDirection = direction.normalized;
		float finialSpeed = spd, finialRotateRight = 0, finialRotateUp = 0;
		float rotateSign = Vector3.Dot (finialDirection, Vector3.right);//（右正左负）
		float upDir = Vector3.Dot (finialDirection, Vector3.up);//得到鱼向下还是向上旋转的COS Y（上正下负）
		Vector3 map= new Vector3(direction.x,0,direction.z);
		float rotateDir = Vector3.Dot (map, Vector3.forward);//向左向右转
		float elevateDir = Vector3.Dot (finialDirection, map);//向上向下转
		float R,E;

		R = Vector3.Angle(Vector3.forward,map);//得到鱼旋转的角度

		if (rotateSign > 0) {//如果鱼向前
			finialRotateRight = R;
		} else {
				finialRotateRight = -R;
		}

		//添加上下旋转角度（0.02防抖）
		if (upDir < -0.02f) {//如果COS Y小于-0。02则是向下转动
			elevationAngle = Quaternion.Euler (limitAngle, finialRotateRight, 0);
		} else if (upDir > 0.02f) {
			elevationAngle = Quaternion.Euler (-limitAngle, finialRotateRight, 0);		
		}
		else//如果上下偏移角度过小，就忽略
			elevationAngle = Quaternion.Euler (0, finialRotateRight, 0);
		transform.Translate (Vector3.forward * finialSpeed * Time.deltaTime);
		transform.rotation = Quaternion.Slerp (transform.rotation, elevationAngle, Time.deltaTime*0.5f);
		currentSpeed=finialSpeed;
	}
		

}

