using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState {
	patrolling,
	phasing,
	chasing
}

public class Monster : MonoBehaviour {
	public GameObject player;
	public bool seesPlayer;
	public bool up;
	public bool down;
	public bool left;
	public bool right;
	public bool forward;
	public bool back;

	MapGen mapGenerator;

	private RaycastHit playerHit;
	private RaycastHit upHit;
	private RaycastHit downHit;
	private RaycastHit leftHit;
	private RaycastHit rightHit;
	private RaycastHit forwardHit;
	private RaycastHit backHit;

	public MonsterState state;
	private MonsterState stateLF;

	public float speed = 1f;
	public Vector3 targetPosition;
	public Vector3 targetVelocity;
	

	private void Start () {
		seesPlayer = false;

		if (!player) {
			player = GameObject.FindWithTag("Player");
		}

		if (!mapGenerator) {
			mapGenerator = FindObjectOfType<MapGen>();
		}

		transform.position = mapGenerator.GetRandomStartPosition();
	}

	private void Update () {
		if (!player) return;

		UpdateStates();
		StateController();
	}

	private void ShootRaycasts () {
		if (up = Physics.Raycast(transform.position, Vector3.up, out upHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (down = Physics.Raycast(transform.position, Vector3.down, out downHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (left = Physics.Raycast(transform.position, Vector3.left, out rightHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (right = Physics.Raycast(transform.position, Vector3.right, out rightHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (forward = Physics.Raycast(transform.position, Vector3.forward, out forwardHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (back = Physics.Raycast(transform.position, Vector3.back, out backHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (seesPlayer = Physics.Raycast(transform.position, (player.transform.position - transform.position),
										 out playerHit, float.PositiveInfinity, LayerMask.GetMask("Player", "Wall"))) {
			
		}
	}

	private void StateController () {
		switch (state) {
			case MonsterState.patrolling:
				if (stateLF != MonsterState.patrolling) {

				}

				if (!forward) {
					targetVelocity = transform.forward * speed;
					return;
				}

				targetPosition = forwardHit.point;
				targetVelocity = (targetPosition - transform.position).normalized * speed;


				//Switch directions
				if (forwardHit.distance < 0.25f) {
					List<Vector3> directions = new List<Vector3>();
					if (up && upHit.distance > 0.25f) {
						directions.Add(Vector3.up);
					}
					if (down && downHit.distance > 0.25f) {
						directions.Add(Vector3.down);
					}
					if (left && leftHit.distance > 0.25f) {
						directions.Add(Vector3.left);
					}
					if (right && rightHit.distance > 0.25f) {
						directions.Add(Vector3.right);
					}

					if (directions.Count == 0) {
						transform.forward = -transform.forward;

					}
					//if (left && leftHit.distance > 0.25f) {
					//	directions.Add(Vector3.back);
					//}
				}
				break;
			case MonsterState.phasing:
				if (stateLF != MonsterState.phasing) {
					
				}
				targetPosition = player.transform.position;
				targetVelocity = (targetPosition - transform.position).normalized * speed;
				if (!Physics.CheckSphere(transform.position, 0.2f, LayerMask.GetMask("Wall"))) {
					state = MonsterState.patrolling;
				}
				break;
			case MonsterState.chasing:
				if (stateLF != MonsterState.chasing) {

				}
				break;
			default:
				break;
		}
	}

	private void UpdateStates () {
		ShootRaycasts();
		if (state != MonsterState.phasing) {
			if (Random.value < 0.1f * Time.deltaTime) {
				state = MonsterState.phasing;
			}
		}

		stateLF = state;
	}
}
