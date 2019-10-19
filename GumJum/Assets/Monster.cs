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
	private bool playerRaycast;
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

	private float switchDirectionsCD;
	

	private void Start () {
		seesPlayer = false;

		if (!player) {
			player = GameObject.FindWithTag("Player");
		}

		if (!mapGenerator) {
			mapGenerator = FindObjectOfType<MapGen>();
		}

		switchDirectionsCD = 0f;
	}

	private void Update () {

		//transform.forward = Random.onUnitSphere;
		if (!player) return;

		UpdateStates();
		StateController();
		transform.position += transform.forward * Time.deltaTime;
	}

	private void ShootRaycasts () {
		if (up = Physics.Raycast(transform.position, Vector3.up, out upHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (down = Physics.Raycast(transform.position, Vector3.down, out downHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (left = Physics.Raycast(transform.position, Vector3.left, out leftHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (right = Physics.Raycast(transform.position, Vector3.right, out rightHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (forward = Physics.Raycast(transform.position, Vector3.forward, out forwardHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (back = Physics.Raycast(transform.position, Vector3.back, out backHit, float.PositiveInfinity, LayerMask.GetMask("Wall"))) {

		}

		if (playerRaycast = Physics.Raycast(transform.position, (player.transform.position - transform.position),
										 out playerHit, float.PositiveInfinity, LayerMask.GetMask("Player", "Wall"))) {
			seesPlayer = playerHit.transform.CompareTag("Player");
		}
	}

	private void StateController () {
		switch (state) {
			case MonsterState.patrolling:
				if (stateLF != MonsterState.patrolling) {

				}
				//targetPosition = forwardHit.point;
				//targetVelocity = (targetPosition - transform.position).normalized * speed;


				//Switch directions
				//if (forward) {
				//	print("Distance: " + forwardHit.distance);
				//}

				if (switchDirectionsCD > 0f) {
					break;
				} else {
					switchDirectionsCD -= Time.deltaTime;
				}

				if (forward && forwardHit.distance < 1f) {
					print("SWITCH DIRECTIONS!");
					switchDirectionsCD = 0.5f;
					List<Vector3> directions = new List<Vector3>();
					if (up && upHit.distance > 1f) {
						directions.Add(Vector3.up);
					}
					if (down && downHit.distance > 1f) {
						directions.Add(Vector3.down);
					}
					if (left && leftHit.distance > 1f) {
						directions.Add(Vector3.left);
					}
					if (right && rightHit.distance > 1f) {
						directions.Add(Vector3.right);
					}

					print("Options: " + directions.Count);
					if (directions.Count == 0) {
						print("!!!!!");
						transform.forward = -transform.forward;
					} else {
						Vector3 direction = directions[Random.Range(0, directions.Count)];
						print("Setting Forward from " + transform.forward + " to " + direction);
						print("Before: " + transform.forward);
						transform.forward = direction;
						print("After: " + transform.forward);
						if (transform.forward != Vector3.up) {
							transform.up = Vector3.up;
						}
					}
				}
				//targetVelocity = transform.forward * speed;
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
		//if (state != MonsterState.phasing) {
		//	if (Random.value < 0.1f * Time.deltaTime) {
		//		state = MonsterState.phasing;
		//	}
		//}

		//stateLF = state;
	}

	private void OnDrawGizmos () {
		Gizmos.color = Color.cyan;
		if (up) {
			Debug.DrawLine(transform.position, upHit.point);
		} else {
			Debug.DrawRay(transform.position, transform.position + Vector3.up * 1000f);
		}

		if (down) {
			Debug.DrawLine(transform.position, downHit.point);
		} else {
			Debug.DrawRay(transform.position, transform.position + Vector3.down * 1000f);
		}

		if (left) {
			Debug.DrawLine(transform.position, leftHit.point);
		} else {
			Debug.DrawRay(transform.position, transform.position + Vector3.left * 1000f);
		}

		if (right) {
			Debug.DrawLine(transform.position, rightHit.point);
		} else {
			Debug.DrawRay(transform.position, transform.position + Vector3.right * 1000f);
		}

		if (forward) {
			Debug.DrawLine(transform.position, forwardHit.point);
		} else {
			Debug.DrawRay(transform.position, transform.position + Vector3.forward * 1000f);
		}

		if (back) {
			Debug.DrawLine(transform.position, backHit.point);
		} else {
			Debug.DrawRay(transform.position, transform.position + Vector3.back * 1000f);
		}

		if (playerRaycast) {
			Debug.DrawLine(transform.position, playerHit.point);
		}

		//Physics.Raycast(transform.position, Vector3.up, float.PositiveInfinity, LayerMask.GetMask("Wall"));
		//Physics.Raycast(transform.position, Vector3.down, float.PositiveInfinity, LayerMask.GetMask("Wall"));
		//Physics.Raycast(transform.position, Vector3.left, float.PositiveInfinity, LayerMask.GetMask("Wall"));
		//Physics.Raycast(transform.position, Vector3.right, float.PositiveInfinity, LayerMask.GetMask("Wall"));
		//Physics.Raycast(transform.position, Vector3.forward, float.PositiveInfinity, LayerMask.GetMask("Wall"));
		//Physics.Raycast(transform.position, Vector3.back, float.PositiveInfinity, LayerMask.GetMask("Wall"));
		Gizmos.color = Color.red;
		//Physics.Raycast(transform.position, (player.transform.position - transform.position),
		//				float.PositiveInfinity, LayerMask.GetMask("Player", "Wall"));
		}
}
