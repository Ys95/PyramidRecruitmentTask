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
        [SerializeField] private SpawnSettings            _playerSpawnSettings;
        [SerializeField] private List<SpawnSettings>      _objectsSpawnSettings;
        [SerializeField] private Transform                _objectsContainer;
        [Inject]         private CinemachineVirtualCamera _cmMainCam;

        [Inject] private DiContainer   _diContainer;
        private          Player.Player _player;

        private void OnDrawGizmos()
        {
            if (_objectsSpawnSettings == null)
            {
                return;
            }

            foreach (var objSettings in _objectsSpawnSettings.Append(_playerSpawnSettings))
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
            var obj = _diContainer.InstantiatePrefab(_playerSpawnSettings.ObjectPrefab);
            obj.transform.position = GetSpawnPosition(_playerSpawnSettings, obj);
            obj.transform.RotateAround(_playerSpawnSettings.SpawnPointCenter.position, _playerSpawnSettings.SpawnPointCenter.up, _playerSpawnSettings.SpawnPointCenter.eulerAngles.y);

            _player = obj.GetComponent<Player.Player>();

            _cmMainCam.LookAt = _player.P_CameraTarget;
            _cmMainCam.Follow = _player.P_CameraTarget;
            ;
        }

        private void RespawnObjects()
        {
            foreach (var objSettings in _objectsSpawnSettings)
            {
                var obj = _diContainer.InstantiatePrefab(objSettings.ObjectPrefab);
                obj.transform.position = GetSpawnPosition(objSettings, obj);
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
                    obj.transform.position = GetSpawnPosition(objSettings, obj);
                    obj.transform.RotateAround(objSettings.SpawnPointCenter.position, objSettings.SpawnPointCenter.up, objSettings.SpawnPointCenter.eulerAngles.y);
                    obj.transform.parent = _objectsContainer;
                }
            }
        }

        private Vector3 GetSpawnPosition(SpawnSettings spawnSettings, GameObject obj)
        {
            var center    = spawnSettings.SpawnPointCenter.position;
            var collider  = obj.GetComponent<Collider>();
            var bounds    = collider != null ? collider.bounds.extents : Vector3.zero;
            var spawnArea = spawnSettings.SpawnAreaSize;

            // Calculate spawn area taking object bounds into account
            float rangeMinX = center.x - spawnArea.x / 2 + bounds.x;
            float rangeMaxX = center.x + spawnArea.x / 2 - bounds.x;

            float rangeMinZ = center.z - spawnArea.z / 2 + bounds.z;
            float rangeMaxZ = center.z + spawnArea.z / 2 - bounds.z;

            // Choose random point in spawn area
            var spawnPos = new Vector3(Random.Range(rangeMinX, rangeMaxX), 0, Random.Range(rangeMinZ, rangeMaxZ));
            spawnPos.y = center.y + bounds.y;

            Debug.Log($"spawn {spawnSettings.ObjectPrefab.name} woth bounds size {bounds} ");

            return spawnPos;
        }

        [Serializable]
        private struct SpawnSettings
        {
            public GameObject ObjectPrefab;
            public Transform  SpawnPointCenter;
            public Vector3    SpawnAreaSize;
            public Color      GizmoColor;
        }
    }
}