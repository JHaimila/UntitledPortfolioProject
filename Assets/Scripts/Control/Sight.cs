using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Control
{
    public class Sight : MonoBehaviour
    {
        [SerializeField] private float _distance = 10;
        [SerializeField] private float _angle = 30;
        [SerializeField] private float _height = 1.0f;
        [SerializeField] private Color _meshColor = Color.blue;
        [SerializeField] private int _scanFrequency = 30;
        [SerializeField] private LayerMask _layers;

        private GameObject target;

        public event Action SeesTargetEvent;
        public event Action LostTargetEvent;

        private List<GameObject> _objects = new List<GameObject>();

        private Collider[] _colliders = new Collider[50];

        private Mesh _mesh;

        private int _count;
        private float _scanInterval;
        private float scanTimer;
        public bool seesPlayer = false;
        public Transform playerPosition;

        // Start is called before the first frame update
        void Start()
        {
            _scanInterval = 1.0f/_scanFrequency;
        }

        // Update is called once per frame
        void Update()
        {
            scanTimer -= Time.deltaTime;
            if(scanTimer < 0)
            {
                scanTimer += _scanInterval;
                Scan();
            }
        }

        private void Scan()
        {
            _count = Physics.OverlapSphereNonAlloc(transform.position, _distance, _colliders, _layers, QueryTriggerInteraction.Collide);
            _objects.Clear();
            for(int i = 0; i < _count; i++)
            {
                GameObject obj = _colliders[i].gameObject;
                if(IsInSight(obj))
                {
                    _objects.Add(obj);
                }
            }
            if(_objects.Count > 0)
            {
                foreach(var obj in _objects)
                {
                    if(obj.tag.Equals("Player"))
                    {
                        if(!seesPlayer)
                        {
                            seesPlayer = true;
                            SeesTargetEvent?.Invoke();
                        }
                    }
                }
            }
            else
            {
                if(seesPlayer)
                {
                    seesPlayer = false;
                    LostTargetEvent?.Invoke();
                }
            }
        }

        public bool IsInSight(GameObject obj)
        {
            Vector3 origin = transform.position;
            Vector3 dest = obj.transform.position;
            Vector3 direction = dest - origin;

            if(direction.y < -0.09f || direction.y > _height)
            {
                return false;
            }
            direction.y = 0;
            float deltaAngle = Vector3.Angle(direction, transform.forward);
            if(deltaAngle > _angle)
            {
                return false;
            }
            return true;
        }

        public void SetTarget(GameObject target)
        {
            this.target = target;
        }
        Mesh CreateWedgeMesh()
        {
            Mesh mesh = new Mesh();

            int segments = 10;
            // 2+2 is for the left and right side of the wedge. Segments will have 4 triangles each, 
            int numTriangles = (segments * 4)+2+2;
            int numVerticies = numTriangles * 3;

            Vector3[] verticies = new Vector3[numVerticies];
            int[] triangles = new int[numVerticies];

            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, -_angle, 0) * Vector3.forward * _distance;
            Vector3 bottomRight = Quaternion.Euler(0, _angle, 0) * Vector3.forward * _distance;

            Vector3 topCenter = bottomCenter + Vector3.up * _height;
            Vector3 topLeft = bottomLeft + Vector3.up * _height;
            Vector3 topRight = bottomRight + Vector3.up * _height;

            int vert = 0;

            // left side
            verticies[vert++] = bottomCenter;
            verticies[vert++] = bottomLeft;
            verticies[vert++] = topLeft;

            verticies[vert++] = topLeft;
            verticies[vert++] = topCenter;
            verticies[vert++] = bottomCenter;

            // right side
            verticies[vert++] = bottomCenter;
            verticies[vert++] = topCenter;
            verticies[vert++] = topRight;

            verticies[vert++] = topRight;
            verticies[vert++] = bottomRight;
            verticies[vert++] = bottomCenter;

            // What this section does is it will subdivide one line of sight wedge into multiple wedges so that it looks good and funtions better when you make give it a bigger viewing angle
            float currentAngle = -_angle;
            float deltaAngle = (_angle*2)/segments;
            for(int i = 0; i< segments; i++)
            {
                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * _distance;
                bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * _distance;

                topLeft = bottomLeft + Vector3.up * _height;
                topRight = bottomRight + Vector3.up * _height;

                // far side
                verticies[vert++] = bottomLeft;
                verticies[vert++] = bottomRight;
                verticies[vert++] = topRight;

                verticies[vert++] = topRight;
                verticies[vert++] = topLeft;
                verticies[vert++] = bottomLeft;
                // top
                verticies[vert++] = topCenter;
                verticies[vert++] = topLeft;
                verticies[vert++] = topRight;
                // bottom
                verticies[vert++] = bottomCenter;
                verticies[vert++] = bottomRight;
                verticies[vert++] = bottomLeft;

                currentAngle += deltaAngle;
            }
            

            for(int i = 0; i< numVerticies; i++)
            {
                triangles[i] = i;
            }

            mesh.vertices = verticies;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }

        private void OnValidate() {
            _mesh = CreateWedgeMesh();
        }

        private void OnDrawGizmos() {
            if(_mesh)
            {
                Gizmos.color = _meshColor;
                Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
            }

            Gizmos.DrawWireSphere(transform.position, _distance);
            for(int i = 0; i < _count; i++)
            {
                Gizmos.DrawSphere(_colliders[i].transform.position, 0.2f);
            }

            Gizmos.color = Color.green;
            foreach(var obj in _objects)
            {
                Gizmos.DrawSphere(obj.transform.position, 0.2f); 
            }
        }
    }
}

