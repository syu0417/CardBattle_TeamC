using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class Billboard : MonoBehaviour
{
    
        [SerializeField, Header("対象のカメラ")]
        private Camera m_TargetCamera;
        public Camera targetCamera
        {
            get { return m_TargetCamera; }
            set { m_TargetCamera = value; }
        }
        private void LateUpdate()
        {
            if (targetCamera == null)
            {
                return;
            }
            // カメラの向いている方向ベクトル
            var cameraV = targetCamera.transform.rotation * Vector3.forward;
            var reverse = cameraV * -1f;
            transform.localRotation = Quaternion.FromToRotation(Vector3.back, reverse);
            // x固定
            reverse.y = 0;
            transform.localRotation = Quaternion.FromToRotation(Vector3.back, reverse);
        }

}
