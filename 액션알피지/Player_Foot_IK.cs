using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Foot_IK : MonoBehaviour
{
    [SerializeField]
    Animator _animator;


    public LayerMask GroundLayer;

    [Range(0f, 1f)]
    public float DistanceToGround;
    

    // Update is called once per frame





    private void OnAnimatorIK(int layerIndex)
    {
        if(_animator!=null)
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);


            RaycastHit hit;
            Ray ray = new Ray(_animator.GetIKPosition(AvatarIKGoal.LeftFoot)+Vector3.up,Vector3.down);
            if(Physics.Raycast(ray,out hit,DistanceToGround+1f,GroundLayer))
            {
                
                Vector3 footPosition = hit.point;
                footPosition.y += DistanceToGround;
                _animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                
            }

            
            ray = new Ray(_animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, GroundLayer))
            {
                
                Vector3 footPosition = hit.point;
                footPosition.y += DistanceToGround;
                _animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                
            }
        }
    }
}
