using UnityEngine;
using UnityEngine.AI;
using System;
using RPG.Core;
using UnityEngine.EventSystems;

namespace RPG.Control.PlayerController
{
    public class InteractionHandler : MonoBehaviour
    {
        [field:SerializeField] public InputReader InputReader {get; private set;}

        private Camera mainCamera;
        private RaycastHit hit;
        public bool uiOpen = false;

        public event Action<Vector3> MoveEvent;
        public event Action<Transform> AttackEvent;
        public event Action<Transform> InteractEvent;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float maxNavPathLength = 25f;

        [SerializeField] private EventSystem _eventSystem;

        private CursorType currentCursor = CursorType.None;

        enum CursorType
        {
            None, 
            Movement,
            Combat,
            Interact,
            UI
        }
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;

        }
        [SerializeField] CursorMapping[] cursorMappings = null;
        
        private void SetCursor(CursorType type)
        {
            if(type != currentCursor)
            {   
                CursorMapping mapping = GetCursorMapping(type);
                Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
            }
        }
        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(var mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private void Start() {
            mainCamera = Camera.main;
        }
        private void OnEnable() 
        {
            InputReader.PlayerActionEvent += HandleClick;
        }
        private void OnDisable() 
        {
            InputReader.PlayerActionEvent -= HandleClick;
        }

        private void Update() {
            if(uiOpen){return;}
            if(InputReader.MouseDown)
            {
                HandleClick();
            }
            
        }
        private void FixedUpdate() {
            if(uiOpen){return;}
            Ray ray = mainCamera.ScreenPointToRay(InputReader.pointerPosition);

            bool hasHit = Physics.Raycast(ray, out hit);
            if(hasHit)
            {
                
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

                
                if(MouseOverUILayerObject.IsPointerOverUIObject(InputReader.pointerPosition))
                {
                    SetCursor(CursorType.UI);
                }
                else if(hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    SetCursor(CursorType.Interact);
                }
                else if(hit.transform.TryGetComponent<IAttackable>(out IAttackable target))
                {
                    if(target.Attackable())
                    {
                        if(hit.transform != transform)
                        {
                            SetCursor(CursorType.Combat);
                        }
                    }
                }
                else
                {
                    SetCursor(CursorType.Movement);
                }
            }
        }

        private void HandleClick()
        {
            if(uiOpen){return;}

            if(MouseOverUILayerObject.IsPointerOverUIObject(InputReader.pointerPosition))
            {
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(InputReader.pointerPosition);

            if(!Physics.Raycast(ray, out hit)){return;}

            IAttackable target;
            hit.transform.TryGetComponent<IAttackable>(out target);
            
            Debug.Log(hit.transform.position);
            
            if(target != null && target.Attackable())
            {
                AttackEvent?.Invoke(hit.transform);
                return;
            }
            if(hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                InteractEvent?.Invoke(hit.transform);
                return;
            }
            
            bool hasHitNavmesh = RaycastNavmesh(out Vector3 position);
            if(!hasHitNavmesh){return;}

            MoveEvent?.Invoke(position);
        }

        private bool RaycastNavmesh(out Vector3 target)
        {
            target = new Vector3();
            
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(InputReader.pointerPosition);
            bool hasHit = Physics.Raycast(ray, out hit);

            if(!hasHit){return false;}
            NavMeshHit navMeshHit;
            bool hasCastToNavmesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            
            if(!hasCastToNavmesh){return false;}

            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();

            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if(!hasPath){return false;}
            if(path.status != NavMeshPathStatus.PathComplete){return false;}

            if(GetPathLength(path) > maxNavPathLength){return false;}

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if(path.corners.Length < 2){return total;}
            for(int i = 0; i < path.corners.Length-1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i+1]);
            }
            return total;
        }
    }
}

