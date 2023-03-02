using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PyramidRecruitmentTask
{
    public class InteractablesSpawner : MonoBehaviour
    {
        [Serializable]
        private struct SpawnSettings
        {
            public InteractableObject ObjectPrefab;
            public Transform          SpawnPointCenter;
            public Vector3            SpawnAreaSize;
            public Color              GizmoColor;
        }

        [SerializeField] private List<SpawnSettings> _objectsSpawnSettings;
        [SerializeField] private Transform           _objectsContainer;


        
        public void RespawnObjects()
        {
            foreach (Transform child in _objectsContainer)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var objSettings in _objectsSpawnSettings)
            {
                var obj = Instantiate(objSettings.ObjectPrefab);
                obj.transform.position = GetSpawnPosition(objSettings);
                obj.transform.RotateAround(objSettings.SpawnPointCenter.position, objSettings.SpawnPointCenter.up, objSettings.SpawnPointCenter.eulerAngles.y);
                obj.transform.parent = _objectsContainer;
            }
        }
        

        [ContextMenu("Respawn 100")]
        public void Respawn100()
        {
            foreach (Transform child in _objectsContainer)
            {
                DestroyImmediate(child.gameObject);
            }
            
            for (int i = 0; i < 100; i++)
            {
                foreach (var objSettings in _objectsSpawnSettings)
                {
                    var obj = Instantiate(objSettings.ObjectPrefab);
                    obj.transform.position = GetSpawnPosition(objSettings);
                    obj.transform.RotateAround(objSettings.SpawnPointCenter.position, objSettings.SpawnPointCenter.up, objSettings.SpawnPointCenter.eulerAngles.y);
                    obj.transform.parent   = _objectsContainer;
                }
            }
        }

        private Vector3 GetSpawnPosition(SpawnSettings spawnSettings)
        {
            var center    = spawnSettings.SpawnPointCenter.position;
            var bounds    = spawnSettings.ObjectPrefab.GetComponent<Renderer>().bounds;
            var spawnArea = spawnSettings.SpawnAreaSize;
            
            // Calculate spawn area taking object bounds into account
            var rangeMinX = (center.x - (spawnArea.x / 2)) + bounds.extents.x;
            var rangeMaxX = (center.x + (spawnArea.x / 2)) - bounds.extents.x;
            
            var rangeMinZ = (center.z - (spawnArea.z / 2)) + bounds.extents.z;
            var rangeMaxZ = (center.z + (spawnArea.z / 2)) - bounds.extents.z;
            
            // Choose random point in spawn area
            var spawnPos = new Vector3(Random.Range(rangeMinX, rangeMaxX), 0, Random.Range(rangeMinZ, rangeMaxZ));
            spawnPos.y = center.y + bounds.extents.y;

            return  spawnPos;
        }

        private void OnDrawGizmos()
        {
            if (_objectsSpawnSettings == null)
            {
                return;
            }
            
            foreach (var objSettings in _objectsSpawnSettings)
            {
                if (objSettings.SpawnPointCenter == null)
                {
                    continue;
                }
                Gizmos.color  = objSettings.GizmoColor;
                Gizmos.matrix = Matrix4x4.TRS(objSettings.SpawnPointCenter.position, objSettings.SpawnPointCenter.rotation, objSettings.SpawnPointCenter.lossyScale);
                Gizmos.DrawCube(Vector3.zero, new Vector3(objSettings.SpawnAreaSize.x, objSettings.SpawnAreaSize.y, objSettings.SpawnAreaSize.z));
            }
        }
    }
}