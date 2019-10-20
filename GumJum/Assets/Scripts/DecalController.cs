using System.Collections.Generic;
using UnityEngine;

public class DecalController : MonoBehaviour {

	public static DecalController instance;

	[SerializeField]
	[Tooltip("The prefab for the bullet hole")]
	private GameObject decalPrefab;

	[SerializeField]
	[Tooltip("The number of decals to keep alive at a time.  After this number are around, old ones will be replaced.")]
	private int maxConcurrentDecals = 100;

	private Queue<GameObject> decalsInPool;
	private Queue<GameObject> decalsActiveInWorld;

	private void Awake () {
		if (instance) {
			Destroy(gameObject);
		} else {
			instance = this;
		}

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
		var spawned = GameObject.Instantiate(decalPrefab);
		spawned.transform.SetParent(this.transform);

		decalsInPool.Enqueue(spawned);
		spawned.SetActive(false);
	}

	public static void SpawnDecal (RaycastHit hit) {
		GameObject decal = instance.GetNextAvailableDecal();
		if (decal != null) {
			decal.transform.position = hit.point;
			decal.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);

			decal.SetActive(true);

			instance.decalsActiveInWorld.Enqueue(decal);
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