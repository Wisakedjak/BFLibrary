using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BFL
{
    public class ScreenShooter : MonoBehaviour
    {
        [SerializeField] private bool ios;
        [SerializeField] private bool android;
        [SerializeField] private bool isVertical;
        [HideInInspector] public bool custom;
        
        [Header("Custom Fields")]
        public List<ScreenShootData> screenShootDatas = new List<ScreenShootData>();
        
        static int _count = 0;
        
        [FolderPath]
        [SerializeField] private string path;
        
        private bool isTakingScreenshot = false;
        
        
        private readonly List<ScreenShootData> _iosData = new List<ScreenShootData>()
        {
            new ScreenShootData(new Vector2(2868, 1320), "iPhone 6.9"),
            new ScreenShootData(new Vector2(2688, 1242), "iPhone 6.5"),
            new ScreenShootData(new Vector2(2752, 2064), "iPad 12.9"),
                
        };
        
        private readonly List<ScreenShootData> _androidData = new List<ScreenShootData>()
        {
            new ScreenShootData(new Vector2(1920, 1080), "16:9"),
        };

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isTakingScreenshot)
            {
                StartCoroutine(TakeScreenShotsInOrder());
                _count++;
            }
        }
        
        private IEnumerator TakeScreenShotsInOrder()
        {
            isTakingScreenshot = true;
            
            if (ios)
            {
                yield return StartCoroutine(TakeScreenShot(_iosData));
            }

            if (android)
            {
                yield return StartCoroutine(TakeScreenShot(_androidData));
            }

            if (custom)
            {
                yield return StartCoroutine(TakeScreenShot(screenShootDatas));
            }
            
            isTakingScreenshot = false;
        }

        private IEnumerator TakeScreenShot(List<ScreenShootData> screenShootData)
        {
            foreach (var t in screenShootData)
            {
                if (!GameViewUtils.SizeExists(GameViewUtils.GetCurrentGroupType(), (int)t.screenResolution.x, (int)t.screenResolution.y))
                {
                    SetScreenResolution(t.screenResolution, t.name);
                    
                }

                if (isVertical)
                {
                    (t.screenResolution.x, t.screenResolution.y) = (t.screenResolution.y, t.screenResolution.x);
                }
                GameViewUtils.SetSize(GameViewUtils.FindSize(GameViewUtils.GetCurrentGroupType(), (int)t.screenResolution.x, (int)t.screenResolution.y));
                yield return new WaitForSeconds(1f);
                ScreenCapture.CaptureScreenshot($"{path}/screenshot--{t.name}-{_count}.png");
                yield return new WaitForSeconds(1f);
            }

           
        }
        
        private void SetScreenResolution(Vector2 resolution,string name)
        {
            GameViewUtils.AddCustomSize(GameViewUtils.GameViewSizeType.FixedResolution,GameViewUtils.GetCurrentGroupType(), (int)resolution.x, (int)resolution.y, name);
        }
    }

    [CustomEditor(typeof(ScreenShooter))]
    public class ScreenShooterEditorScript : Editor
    {
        SerializedProperty customProp;
        SerializedProperty iosProp;
        SerializedProperty androidProp;
        SerializedProperty screenShootDatasProp;
        SerializedProperty pathProp;

        private void OnEnable()
        {
            iosProp = serializedObject.FindProperty("ios");
            androidProp = serializedObject.FindProperty("android");
            customProp = serializedObject.FindProperty("custom");
            screenShootDatasProp = serializedObject.FindProperty("screenShootDatas");
            pathProp = serializedObject.FindProperty("path");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(iosProp);
            EditorGUILayout.PropertyField(androidProp);
            EditorGUILayout.PropertyField(customProp);
            EditorGUILayout.PropertyField(pathProp);

            if (customProp.boolValue)
            {
                EditorGUILayout.PropertyField(screenShootDatasProp, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
    
    [CustomPropertyDrawer(typeof(FolderPathAttribute))]
    public class FolderPathDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect textFieldPosition = new Rect(position.x, position.y, position.width - 60, position.height);
            Rect buttonPosition = new Rect(position.x + position.width - 55, position.y, 55, position.height);

            property.stringValue = EditorGUI.TextField(textFieldPosition, label, property.stringValue);

            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            if (GUI.Button(buttonPosition, "Browse"))
            {
                string path = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                if (!string.IsNullOrEmpty(path))
                {
                    property.stringValue = path;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }

            EditorGUI.EndProperty();
        }
    }

    public class FolderPathAttribute : PropertyAttribute
    {
    }

    [Serializable]
        public class ScreenShootData
        {
            public Vector2 screenResolution;
            public string name;
            
            public ScreenShootData(Vector2 screenResolution, string name)
            {
                this.screenResolution = screenResolution;
                this.name = name;
            }
        }
    
}