using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    ETCJoystick joy;

    void Start()
    {
        joy = GameObject.FindObjectOfType<ETCJoystick>();
        if (joy != null)
        {
            joy.onMoveStart.AddListener(StartMoveCallBack);
            joy.onMove.AddListener(MoveCallBack);
            joy.onMoveEnd.AddListener(EndMoveCallBack);
        }
        Speed = new FixedPointF(200, 100);
        Camera mCamera = transform.Find("camera").GetComponent<Camera>();
        mCamera.enabled = true;
    }

    bool _IsMove;
    void StartMoveCallBack()
    {
        Debug.Log("StartMoveCallBack");
        _IsMove = true; 
        Animation animation = transform.Find("rotate").Find("actor").transform.GetComponent<Animation>();
        if (animation != null)
        {
            animation.wrapMode = WrapMode.Loop;
            animation.Play("run");
        }
    }

    void EndMoveCallBack()
    {
        Debug.Log("EndMoveCallBack");
        _IsMove = false;
        Animation animation = transform.Find("rotate").Find("actor").transform.GetComponent<Animation>();
        if (animation != null)
        {
            animation.wrapMode = WrapMode.Loop;
            animation.Play("idle");
        }
    }

    private Transform mRotateTransform;
    public Transform RotateTransform
    {
        get
        {
            if (mRotateTransform == null)
                mRotateTransform = transform.Find("rotate").transform;
            return mRotateTransform;
        }
    }
    public int Angle
    {
        get
        {
            CustomTransform mTrans = RotateTransform.GetComponent<CustomTransform>();
            return mTrans.Angle;
        }
        set
        {
            CustomTransform mTrans = RotateTransform.GetComponent<CustomTransform>();
            mTrans.Angle = value;
        }
    }

    public FixedPointF Speed { set; get; }
    public void Move()
    {
        CustomTransform mTrans = GetComponent<CustomTransform>();
        CustomTransform mRotateTrans = RotateTransform.GetComponent<CustomTransform>();
        CustomVector3 temp;

        FixedPointF x = CustomMath.GetCos(mRotateTrans.Angle);
        FixedPointF z = CustomMath.GetSin(mRotateTrans.Angle);

        temp.x = x * Speed * LockStepConfig.mRenderFrameRate;
        temp.y = new FixedPointF(0);
        temp.z = z * Speed * LockStepConfig.mRenderFrameRate;

        mTrans.LocalPosition += temp;
    }

    void MoveCallBack(Vector2 tVec2)
    {
        Debug.Log("MoveCallBack v2" + tVec2);
        //发送遥感角度
        if (tVec2.x != 0)
        {
            int angle = (int)(Mathf.Atan2(tVec2.y, tVec2.x) * 180 / 3.14f);
            if (Mathf.Abs(Angle - angle) > 5)
            {
                Debug.Log("MoveCallBack angle" + angle);
                Angle = angle;
                CustomTransform mTrans = GetComponent<CustomTransform>();
                CustomTransform mRotateTrans = RotateTransform.GetComponent<CustomTransform>();
                CustomVector3 temp;

                FixedPointF x = CustomMath.GetCos(mRotateTrans.Angle);
                FixedPointF z = CustomMath.GetSin(mRotateTrans.Angle);

                temp.x = x * Speed * LockStepConfig.mRenderFrameRate;
                temp.y = new FixedPointF(0);
                temp.z = z * Speed * LockStepConfig.mRenderFrameRate;

                mTrans.LocalPosition += temp;
            }
        }
        else
        {
            int angle = tVec2.y > 0 ? 90 : -90;
            if (Mathf.Abs(Angle - angle) > 5)
            {
                Debug.Log("MoveCallBack angle" + angle);
                Angle = angle;
                CustomTransform mTrans = GetComponent<CustomTransform>();
                CustomTransform mRotateTrans = RotateTransform.GetComponent<CustomTransform>();
                CustomVector3 temp;

                FixedPointF x = CustomMath.GetCos(mRotateTrans.Angle);
                FixedPointF z = CustomMath.GetSin(mRotateTrans.Angle);

                temp.x = x * Speed * LockStepConfig.mRenderFrameRate;
                temp.y = new FixedPointF(0);
                temp.z = z * Speed * LockStepConfig.mRenderFrameRate;

                mTrans.LocalPosition += temp;
            }
        }
    }

    void OnDestroy()
    {
        if (joy != null)
        {
            joy.onMoveStart.RemoveListener(StartMoveCallBack);
            joy.onMove.RemoveListener(MoveCallBack);
            joy.onMoveEnd.RemoveListener(EndMoveCallBack);
        }
    }

    void Update()
    {
        if (_IsMove)
        {
            Move();
        }
    }
}
