////======= Copyright (c) Valve Corporation, All rights reserved. ===============
//using UnityEngine;
//using System.Collections;
//using System;
//using Newtonsoft.Json;
//using System.Collections.Generic;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace Valve.VR.Extras
//{
//    public class SteamVR_LaserPointer : MonoBehaviour
//    {
//        public SteamVR_Behaviour_Pose pose;

//        // Input interaction value
//        public SteamVR_Action_Single interactWithUI = SteamVR_Input.GetSingleAction("Squeeze");

//        public bool active = true;
//        public Color color;
//        public float thickness = 0.002f;
//        public Color clickColor = Color.green;
//        public GameObject holder;
//        public GameObject pointer;
//        bool isActive = false;
//        public bool addRigidBody = false;
//        public Transform reference;
//        public event PointerEventHandler PointerIn;
//        public event PointerEventHandler PointerOut;
//        public event PointerEventHandler PointerClick;
//        private GraphicRaycaster graphicRaycaster;
//        //private PointerEventData pointerEventData;

//        public Sprite[] images;

//        Transform previousContact = null;


//        private void Start()
//        {
//            images = Resources.LoadAll<Sprite>("Photos");
//            ImageController.images = images;

//            if (pose == null && !TryGetComponent<SteamVR_Behaviour_Pose>(out pose))
//                Debug.LogError("No SteamVR_Behaviour_Pose component found on this object", this);

//            if (interactWithUI == null)
//                Debug.LogError("No ui interaction action has been set on this component.", this);


//            holder = new GameObject();
//            holder.transform.parent = this.transform;
//            holder.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

//            pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
//            pointer.transform.parent = holder.transform;
//            pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
//            pointer.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 50f), Quaternion.identity);
//            BoxCollider collider = pointer.GetComponent<BoxCollider>();
//            if (addRigidBody)
//            {
//                if (collider)
//                {
//                    collider.isTrigger = true;
//                }
//                Rigidbody rigidBody = pointer.AddComponent<Rigidbody>();
//                rigidBody.isKinematic = true;
//            }
//            else
//            {
//                if (collider)
//                {
//                    Destroy(collider);
//                }
//            }
//            Material newMaterial = new(Shader.Find("Unlit/Color"));
//            newMaterial.SetColor("_Color", color);
//            pointer.GetComponent<MeshRenderer>().material = newMaterial;
//            //graphicRaycaster = GetComponent<GraphicRaycaster>();
//            //pointerEventData = new PointerEventData(EventSystem.current);
//        }

//        public virtual void OnPointerIn(PointerEventArgs e)
//        {
//            PointerIn?.Invoke(this, e);
//        }

//        public virtual void OnPointerClick(PointerEventArgs e)
//        {
//            PointerClick?.Invoke(this, e);
//        }

//        public virtual void OnPointerOut(PointerEventArgs e)
//        {
//            PointerOut?.Invoke(this, e);
//        }


//        private void Update()
//        {
//            //Ray ray = new Ray(transform.position, transform.forward);

//            //// Raycast against the UI using the GraphicRaycaster
//            //pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);
//            //List<RaycastResult> results = new List<RaycastResult>();
//            //graphicRaycaster.Raycast(pointerEventData, results);

//            //// Check if there are any UI elements hit
//            //if (results.Count > 0)
//            //{
//            //    // Handle UI interaction
//            //    Debug.Log("UI Element Clicked: " + results[0].gameObject.name);
//            //}


//            if (!isActive)
//            {
//                isActive = true;
//                this.transform.GetChild(0).gameObject.SetActive(true);
//            }

//            float dist = 100f;

//            Ray raycast = new(transform.position, transform.forward);
//            bool bHit = Physics.Raycast(raycast, out RaycastHit hit);

//            if (bHit)
//            {
//                Debug.Log(hit.collider.gameObject.name);
//            }

//            // If hit object has InfoSphere tag
//            if (bHit && hit.collider.gameObject.CompareTag("InfoSphere"))
//            {
//                Debug.Log("Hit component with InfoSphere tag");
//                Debug.Log(hit.collider.gameObject.name);
//                Debug.Log(hit.collider.gameObject.transform);
//                // Set gameObject canvas child visible
//                hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(true);
//            }

//            if (previousContact && previousContact != hit.transform)
//            {
//                PointerEventArgs args = new PointerEventArgs
//                {
//                    fromInputSource = pose.inputSource,
//                    distance = 0f,
//                    flags = 0,
//                    target = previousContact
//                };
//                OnPointerOut(args);
//                previousContact = null;
//            }
//            if (bHit && previousContact != hit.transform)
//            {
//                PointerEventArgs argsIn = new PointerEventArgs
//                {
//                    fromInputSource = pose.inputSource,
//                    distance = hit.distance,
//                    flags = 0,
//                    target = hit.transform
//                };
//                OnPointerIn(argsIn);
//                previousContact = hit.transform;
//            }
//            if (!bHit)
//            {
//                previousContact = null;
//            }
//            if (bHit && hit.distance < 100f)
//            {
//                dist = hit.distance;
//            }

//            if (bHit && interactWithUI.GetAxis(pose.inputSource) == 1f)
//            {
//                PointerEventArgs argsClick = new PointerEventArgs
//                {
//                    fromInputSource = pose.inputSource,
//                    distance = hit.distance,
//                    flags = 0,
//                    target = hit.transform
//                };
//                OnPointerClick(argsClick);
//            }

//            if (interactWithUI != null && interactWithUI.GetAxis(pose.inputSource) == 1f)
//            {
//                pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, dist);
//                pointer.GetComponent<MeshRenderer>().material.color = clickColor;

//                // Routing logic
//                // Output currently hit object
//                if(hit.collider != null)
//                {
//                    Debug.Log(hit.collider.gameObject.name);
//                    Position position = hit.collider.gameObject.GetComponent<ObjectController>().objectInfo;
//                    Debug.Log(position.description);
//                    ImageController.NavigationActivate(position.navigateToImage);
//                    ImageController.ChangeInfo(position.title, position.description);
//                }
//            }
//            else
//            {
//                pointer.transform.localScale = new Vector3(thickness, thickness, dist);
//                pointer.GetComponent<MeshRenderer>().material.color = color;
//            }
//            pointer.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
//        }
//    }

//    public struct PointerEventArgs
//    {
//        public SteamVR_Input_Sources fromInputSource;
//        public uint flags;
//        public float distance;
//        public Transform target;
//    }

//    public delegate void PointerEventHandler(object sender, PointerEventArgs e);


//}