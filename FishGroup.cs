using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishGroup : MonoBehaviour {

	private static List<FishGroup> fishGroups;//所有组

	public LayerMask mask;//成员层
	public LayerMask mask2;//成员层
	public int groupID=0;//组id
	public float keepDistance=0.8f, keepWeight=1.0f;//成员保持距离和保持距离权重
	public float targetWeightCroach=2.0f,targetWeightAway=-1.0f;//靠近目标权重，远离目标权重
	public float keepManDistance = 1.0f,keepManWeight = 1.0f;//与人保持距离以及距离权重
	public float minCorach = 0.8f,maxCorach = 10.0f;//最小距离以及队伍总长
	public float keepY = 3.0f;//不超过食物高度
	//~ public Color color=Color.green;

	public Vector3 targetPosition{//位置
		get{return transform.position;}
	}

	public static void AddGroup(FishGroup group){
		if(fishGroups==null)
			fishGroups=new List<FishGroup>();

		if(!fishGroups.Contains(group))
			fishGroups.Add(group);
	}

	public static FishGroup GetFishGroup(int index){
		if(fishGroups==null)
			fishGroups=new List<FishGroup>(Object.FindObjectsOfType(typeof(FishGroup))as FishGroup[]);

		for(int i=0; i<fishGroups.Count; i++)
			if(fishGroups[i].groupID==index)
				return fishGroups[i];

		throw new System.Exception("groupID not find");
}
}
