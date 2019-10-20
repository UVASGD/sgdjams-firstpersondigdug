using System.Collections.Generic;
using UnityEngine;

public class DecalController : MonoBehaviour {
	[SerializeField]
	[Tooltip("The prefab for the bullet hole")]
	private GameObject bulletHoleDecalPrefab;

	[SerializeField]
	[Tooltip("The number of decals to keep alive at a time.  After this number are around, old ones will be replaced.")]
	private int maxConcurrentDecals = 10;

	private Queue<GameObject> decalsInPool;
	private Queue<GameObject> decalsActiveInWorld;

	private void Awake () {
		InitializeDecals();
	}

	private void InitializeDecals () {
		decalsInPool = new Queue<GameObject>();
		decalsActiveInWorld = new Queue<GameObject>();

		for (int i = 0; i < maxConcurrentDecals; i++) {
			InstantiateDecal();
		}
	}

	private void InstantiateDecal () {
		var spawned = GameObject.Instantiate(bulletHoleDecalPrefab);
		spawned.transform.SetParent(this.transform);

		decalsInPool.Enqueue(spawned);
		spawned.SetActive(false);
	}

	public void SpawnDecal (RaycastHit hit) {
		GameObject decal = GetNextAvailableDecal();
		if (decal != null) {
			decal.transform.position = hit.point;
			decal.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);

			decal.SetActive(true);

			decalsActiveInWorld.Enqueue(decal);
		}
	}

	private GameObject GetNextAvailableDecal () {
		if (decalsInPool.Count > 0)
			return decalsInPool.Dequeue();

		var oldestActiveDecal = decalsActiveInWorld.Dequeue();
		return oldestActiveDecal;
	}

#if UNITY_EDITOR
	private void Update () {
		if (transform.childCount < maxConcurrentDecals)
			InstantiateDecal();
		else if (ShoudlRemoveDecal())
			DestroyExtraDecal();
	}

	private bool ShoudlRemoveDecal () {
		return transform.childCount > maxConcurrentDecals;
	}

	private void DestroyExtraDecal () {
		if (decalsInPool.Count > 0)
			Destroy(decalsInPool.Dequeue());
		else if (ShoudlRemoveDecal() && decalsActiveInWorld.Count > 0)
			Destroy(decalsActiveInWorld.Dequeue());
	}

#endif
}