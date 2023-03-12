using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    public static Player_Camera instance;
    Player_Input _Input;
    Player_Manager manager;
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;

    private LayerMask ignoreLayer;

    Transform myTransform;
    public float defaultPosition;
    public float lookAngle;
    public float pivotAngle;
    public float minimumPivot = -35;
    public float maximumPivot = 35;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public float lookSpeed = 0.01f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;
    public float Smooth = 5f;
    public float Search_Radius = 40.0f;
    private float targetPosition;

    private Vector3 cameraTransformPosition;
    public float cameraSphereRadious = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float MinimumCollisionOffset = 0.2f;
    public float MaximunDistanceLockon = 10;


    
    public Transform CurrentLockonTarget;
    public Transform nearLockonTarget;
    List<Transform> AvilableTarget = new List<Transform>();
    public LayerMask EnviromentLayer;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayer = ~(1 << 8 | 1 << 9 | 1 << 10);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    

    public void FollowTarget(float delta)
    {

        Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        myTransform.position = targetPosition;
        HandleCameraCollisions(delta);


    }


    private void HandleCameraCollisions(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();


        Debug.DrawRay(cameraPivotTransform.position, direction, Color.yellow);
        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadious, direction, out hit, Mathf.Abs(targetPosition), ignoreLayer))
        {
            if (!hit.collider.CompareTag("Enemy_Hit_Box"))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

        }
        if (Mathf.Abs(targetPosition) < MinimumCollisionOffset)
        {
            targetPosition = -MinimumCollisionOffset;


        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = new Vector3(0, 1, cameraTransformPosition.z);


    }



    public void HandleCameraRotation(float delta, float MouseX, float MouseY)
    {

        if (manager.LockOnMode == false && CurrentLockonTarget == null)
        {
            lookAngle += (MouseX * lookSpeed) / delta;
            pivotAngle -= (MouseY * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);
            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        else
        {
            


            Vector3 Dir = CurrentLockonTarget.position - transform.position;
            Dir.Normalize();
            Dir.y = 0;

         
            Quaternion TargetRotation = Quaternion.LookRotation(Dir);


            transform.rotation = TargetRotation;
            Dir = CurrentLockonTarget.position - cameraPivotTransform.position;
            Dir.Normalize();
            TargetRotation = Quaternion.LookRotation(Dir);

            Vector3 EulerAngle = TargetRotation.eulerAngles;
            EulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = EulerAngle;

            //�Ͽ����������� ������ġ�� ���ư��°Ź���
            lookAngle = transform.rotation.eulerAngles.y;//y��
            pivotAngle = EulerAngle.x;//x��
        }
        

    }

    public void HandleLockOn()
    {
        float distance = Mathf.Infinity;
        Collider[] colliders = Physics.OverlapSphere(targetTransform.position, Search_Radius);//ĳ���� ���� ���������� ��Ž��
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Transform>().CompareTag("Lock_on"))
            //Lock_on�±׸� ����������Ʈ�� Ÿ���� �߽���ġ������ ī�޶� �̻��Ѱ��� �ٶ󺸱����
            {
                Transform TargetList = colliders[i].GetComponent<Transform>();
                Vector3 lockTargetDirection = TargetList.transform.position - targetTransform.position;
                float distanceFormTarget = Vector3.Distance(targetTransform.position, TargetList.transform.position);
                float ViewAbleAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                RaycastHit hit;
                if (TargetList.transform.root != targetTransform.transform.root 
                && ViewAbleAngle > -120 && ViewAbleAngle < 120 
                && distanceFormTarget <= MaximunDistanceLockon)
                {
                    
                    if (Physics.Linecast(manager.Char_Center_Posi.transform.position, TargetList.position, out hit))
                    {
                        //ĳ���Ϳ��� Ÿ�ٹ������� ����ĳ��Ʈ�� ������ �浹 �Ǵ� ������Ʈ�� ���� ȭ���� ���������� �˻�
                        if (hit.collider.gameObject.layer != EnviromentLayer)
                            //ȭ�鿡 �������� �ʴ´ٸ� ī�޶� �������� ���ص��������Ƿ� ����Ʈ�� �߰�
                        {
                            AvilableTarget.Add(TargetList);
                        }
                    }

                }
            }

        }
        //���� ���������ʰ� ȭ�鿡 ������ ������ ���� ���尡��� �� �к�
        for (int i = 0; i < AvilableTarget.Count; i++)
        {
            float DistanceFormTarget = Vector3.Distance(targetTransform.position, AvilableTarget[i].transform.position);
            if (DistanceFormTarget < distance)
            {
                distance = DistanceFormTarget;
                nearLockonTarget = AvilableTarget[i].transform;

            }

        }
    }
    public void ClearListTarget()
    {
        AvilableTarget.Clear();
        CurrentLockonTarget = null;
        nearLockonTarget = null;


    }







    public void Init(Transform Posi)
    {
        targetTransform = Posi;
        _Input = Posi.GetComponent<Player_Input>();
        manager = Posi.GetComponent<Player_Manager>();

    }
}
