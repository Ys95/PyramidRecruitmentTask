using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace PyramidRecruitmentTask.Managers
{
    public class ObjectsSpawner : MonoBehaviour
    {
        [SerializeField] private SpawnSettings       _playerSpawnSettings;
        [SerializeField] private List<SpawnSettings> _objectsSpawnSettings;
        [SerializeField] private Transform           _objectsContainer;

        private Player.Player _player;

        [Inject] private CinemachineVirtualCamera _cmMainCam;
        [Inject] private DiContainer              _diContainer;

        private void OnDrawGizmos()
        {
            if (_objectsSpawnSettings == null)
            {
                return;
            }

            foreach (var objSettings in _objectsSpawnSettings.Append(_playerSpawnSettings))
            {
                foreach (var spawnSettings in objSettings.SpawnAreas)
                {
                    if (spawnSettings.Center == null)
                    {
                        continue;
                    }

                    Gizmos.color  = objSettings.GizmoColor;
                    Gizmos.matrix = Matrix4x4.TRS(spawnSettings.Center.position, spawnSettings.Center.rotation, spawnSettings.Center.lossyScale);
                    Gizmos.DrawCube(Vector3.zero, new Vector3(spawnSettings.Size.x, spawnSettings.Size.y, spawnSettings.Size.z));
                }
            }
        }

        public void RespawnEverything()
        {
            WipeEverything();
            RespawnPlayer();
            RespawnObjects();
        }

        public void WipeEverything()
        {
            if (_player != null)
            {
                Destroy(_player.gameObject);
            }

            foreach (Transform child in _objectsContainer)
            {
                Destroy(child.gameObject);
            }
        }

        private void RespawnPlayer()
        {
            var obj           = _diContainer.InstantiatePrefab(_playerSpawnSettings.ObjectPrefab);
            var spawnPosition = _playerSpawnSettings.GetRandomSpawnArea();
            obj.transform.position = GetSpawnPosition(spawnPosition, obj);
            obj.transform.RotateAround(spawnPosition.Center.position, spawnPosition.Center.up, spawnPosition.Center.eulerAngles.y);

            _player = obj.GetComponent<Player.Player>();

            _cmMainCam.LookAt = _player.P_CameraTarget;
            _cmMainCam.Follow = _player.P_CameraTarget;
            ;
        }

        private void RespawnObjects()
        {
            foreach (var objSettings in _objectsSpawnSettings)
            {
                var obj           = _diContainer.InstantiatePrefab(objSettings.ObjectPrefab);
                var spawnSettings = objSettings.GetRandomSpawnArea();
                obj.transform.position = GetSpawnPosition(spawnSettings, obj);
                obj.transform.RotateAround(spawnSettings.Center.position, spawnSettings.Center.up, spawnSettings.Center.eulerAngles.y);
                obj.transform.parent = _objectsContainer;
            }
        }

        private Vector3 GetSpawnPosition(SpawnSettings.SpawnArea spawnAreaSettings, GameObject obj)
        {
            var center    = spawnAreaSettings.Center.position;
            var collider  = obj.GetComponent<Collider>();
            var bounds    = collider != null ? collider.bounds.extents : Vector3.zero;
            var spawnArea = spawnAreaSettings.Size;

            // Calculate spawn area taking object bounds into account
            float rangeMinX = center.x - spawnArea.x / 2 + bounds.x;
            float rangeMaxX = center.x + spawnArea.x / 2 - bounds.x;

            float rangeMinZ = center.z - spawnArea.z / 2 + bounds.z;
            float rangeMaxZ = center.z + spawnArea.z / 2 - bounds.z;

            // Choose random point in spawn area
            var spawnPos = new Vector3(Random.Range(rangeMinX, rangeMaxX), 0, Random.Range(rangeMinZ, rangeMaxZ));
            spawnPos.y = center.y;

            return spawnPos;
        }

        [Serializable]
        private struct SpawnSettings
        {
            [Serializable]
            public struct SpawnArea
            {
                public Transform Center;
                public Vector3   Size;
            }

            public List<SpawnArea> SpawnAreas;
            public GameObject      ObjectPrefab;
            public Color           GizmoColor;

            public SpawnArea GetRandomSpawnArea() => SpawnAreas[Random.Range(0, SpawnAreas.Count)];
        }
    }
}