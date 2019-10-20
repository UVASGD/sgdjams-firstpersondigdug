﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState {
	patrolling,
	phasing,
	chasing,
	stunned,
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Stickable))]
[RequireComponent(typeof(Squishable))]
public class Monster : MonoBehaviour {

    public int points = 50;

	public GameObject player;
	public bool seesPlayer;
	public bool canBeginPhasing;
	public bool insideWall;
	private bool playerRaycast;
	private bool up;
	private bool down;
	private bool left;
	private bool right;
	private bool forward;
	private bool back;

	MapGen mapGenerator;
	Animator anim;

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

	private float distanceSinceLastChangedDirection;
	private float timePhasing;
	private float timeUntilPhase;

	public float inflationLevel;
	public float targetInflationLevel;
	public bool inflating;
	public bool stunned;
	public float timeSinceLastInflated;
	public float maxTimeSinceLastInflated = 2f;


	private Material mat;

	public GameObject bloodSplatter, deflate, pop;
	

	private void Start () {
		seesPlayer = false;
		canBeginPhasing = false;

		if (!player) {
			player = GameObject.FindWithTag("Player");
		}

		if (!mapGenerator) {
			mapGenerator = FindObjectOfType<MapGen>();
		}

		distanceSinceLastChangedDirection = 999f;
		anim = GetComponent<Animator>();
		timePhasing = 0f;

		timeUntilPhase = Random.Range(7f, 12f);

		inflationLevel = 0f;
		targetInflationLevel = 0f;
		inflating = false;
		stunned = false;

		mat = transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material;

        GameManager.Instance.monst++;
	}

	private void Update () {
		if (!player) return;

		if (Input.GetKeyDown(KeyCode.K)) {
			Inflate();
			//Squish();
		} else if (Input.GetKeyDown(KeyCode.J)) {
			Deflate();
		}

		UpdateStates();
		StateController();
		if (stunned) return;
		transform.position += transform.forward * speed * Time.deltaTime;
		distanceSinceLastChangedDirection += speed * Time.deltaTime;
	}

	public void Inflate (float percentage = 0.25f) {
		if (targetInflationLevel >= 1f) return;
		timeSinceLastInflated = 0f;
		targetInflationLevel += percentage;
		if (!inflating) {
			inflating = true;	
			StartCoroutine(InflateCR());
		}
	}

	IEnumerator InflateCR () {
		inflating = true;
		while (inflationLevel < targetInflationLevel) {
			//print("ASDasdfadf");
			inflationLevel += 0.5f * Time.deltaTime;
			mat.SetFloat("_ExplosionAmount", inflationLevel);
			if (inflationLevel < 0.85f) {
				
			} else {
				if (inflationLevel < 0.85f + 0.5f * Time.deltaTime) {
					//Material newMat = new Material(Shader.Find("Shader Graphs/MonsterDissolve"));
					   /* newMat.SetTexture("_Albedo", mat.GetTexture("_Albedo"));
					    newMat.SetColor("_AlbedoColor", mat.GetColor("_AlbedoColor"));
					    newMat.SetTexture("_Roughness", mat.GetTexture("_Roughness"));
					    newMat.SetFloat("_ExplosionRadius", mat.GetFloat("_ExplosionRadius"));
					    newMat.SetFloat("_ExplosionAmount", mat.GetFloat("_ExplosionAmount"));
					    newMat.SetFloat("_DissolveEdgeWidth", mat.GetFloat("_DissolveEdgeWidth"));
					    //mat = newMat;
					    transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material = newMat;
					    mat = transform.Find("Body").GetComponent<SkinnedMeshRenderer>().material;*/
                    if (pop) Instantiate(pop, transform.position, Quaternion.identity);
                    Instantiate(bloodSplatter, transform.position + transform.up * 1.0f, Quaternion.identity);
					GenerateDeathRaycasts(50);
				}
			}
			yield return new WaitForEndOfFrame();
		}
		if (inflationLevel >= 1f) {
			inflationLevel = 1f;
			mat.SetFloat("_ExplosionAmount", inflationLevel);
            Die();
		}
		inflating = false;
	}

    public void Squish()
    {
		stunned = true;
		StartCoroutine(SquishCR());
    }

	private void GenerateDeathRaycasts (int raycastCount) {
		for (int i = 0; i < raycastCount; i++) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position + transform.up, Random.onUnitSphere, out hit, 5f, LayerMask.GetMask("Wall"))) {
				DecalController.SpawnDecal(hit);
			}
		}
	}

	IEnumerator SquishCR () {
		stunned = true;
		float squishAmount = 0f;
		Transform parent = new GameObject("SQUISH HELPER").transform;
		parent.position = transform.position;
		transform.SetParent(parent);

		while (squishAmount < 1f) {
			stunned = true;
			parent.localScale = Vector3.Lerp(Vector3.one, new Vector3(1f, 0.2f, 1f), squishAmount);
			squishAmount += 10f * Time.deltaTime;
			yield return new WaitForEndOfFrame();
			stunned = true;
		}
		stunned = true;
		transform.SetParent(null);
        Instantiate(bloodSplatter, transform.position + transform.up * 1.0f, Quaternion.identity);
        Destroy(parent.gameObject);
		Die(2f);
	}

	public void Die (float delay = 0f) {
		GameManager.Instance.KillMonster(points);
		Destroy(gameObject, delay);
	}

	public void Deflate (float percentage = 0.25f) {
		//print("DEFLATE");
		if (targetInflationLevel <= 0f) return;
		if (inflating) return;
		targetInflationLevel -= percentage;
		if (!inflating) {
			StartCoroutine(DeflateCR());
		}
	}

	IEnumerator DeflateCR () {
		//print("Target:")
		while (inflationLevel > targetInflationLevel) {
			if (inflating) break;
            if (deflate) Instantiate(deflate, transform);
			//print("ASDasdfadf");
			inflationLevel -= 0.5f * Time.deltaTime;
			mat.SetFloat("_ExplosionAmount",	 inflationLevel);
			yield return new WaitForEndOfFrame();
		}
		if (inflationLevel <= 0f) {
			inflationLevel = 0f;
			mat.SetFloat("_ExplosionAmount", inflationLevel);
			stunned = false;
		}
	}

	private void ChangeDirections (Vector3 direction) {
		if (distanceSinceLastChangedDirection < 1f)
			return;
		distanceSinceLastChangedDirection = 0f;
		if (direction != Vector3.up) {
			transform.up = Vector3.up;
		}
		transform.forward = direction;
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

				if (Physics.Raycast(transform.position, transform.forward, 1f, LayerMask.GetMask("Wall"))) {
					List<Vector3> directions = new List<Vector3>();
					if (up && upHit.distance > 1f && transform.forward != Vector3.up && transform.forward != Vector3.down) {
						directions.Add(Vector3.up);
					} else if (!up) {
						directions.Add(Vector3.up);
					}
					if (down && downHit.distance > 1f && transform.forward != Vector3.down && transform.forward != Vector3.up) {
						directions.Add(Vector3.down);
					} else if (!down) {
						directions.Add(Vector3.down);
					}
					if (left && leftHit.distance > 1f && transform.forward != Vector3.left && transform.forward != Vector3.right) {
						directions.Add(Vector3.left);
					} else if (!left) {
						directions.Add(Vector3.left);
					}
					if (right && rightHit.distance > 1f && transform.forward != Vector3.right && transform.forward != Vector3.left) {
						directions.Add(Vector3.right);
					} else if (!right) {
						directions.Add(Vector3.right);
					}
					if (forward && forwardHit.distance > 1f && transform.forward != Vector3.forward && transform.forward != Vector3.back) {
						directions.Add(Vector3.forward);
					} else if (!forward) {
						directions.Add(Vector3.forward);
					}
					if (back && backHit.distance > 1f && transform.forward != Vector3.back && transform.forward != Vector3.forward) {
						directions.Add(Vector3.back);
					} else if (!back) {
						directions.Add(Vector3.back);
					}

					if (directions.Count == 0) {
						transform.forward = -transform.forward;
					} else {
						Vector3 direction = directions[Random.Range(0, directions.Count)];
						ChangeDirections(direction);
					}
				}
				break;
			case MonsterState.phasing:
				//if (stateLF != MonsterState.phasing) {
					
				//}
				
				//break;
			case MonsterState.chasing:
				//if (stateLF != MonsterState.chasing) {

				//}

				Vector3 targetDirection = Vector3.up;
				float playerDot = Vector3.Dot(Vector3.up, player.transform.position - transform.position);
				if (Vector3.Dot(Vector3.down, player.transform.position - transform.position) > playerDot) {
					playerDot = Vector3.Dot(Vector3.down, player.transform.position - transform.position);
					targetDirection = Vector3.down;
				}
				if (Vector3.Dot(Vector3.left, player.transform.position - transform.position) > playerDot) {
					playerDot = Vector3.Dot(Vector3.left, player.transform.position - transform.position);
					targetDirection = Vector3.left;
				}
				if (Vector3.Dot(Vector3.right, player.transform.position - transform.position) > playerDot) {
					playerDot = Vector3.Dot(Vector3.right, player.transform.position - transform.position);
					targetDirection = Vector3.right;
				}
				if (Vector3.Dot(Vector3.forward, player.transform.position - transform.position) > playerDot) {
					playerDot = Vector3.Dot(Vector3.forward, player.transform.position - transform.position);
					targetDirection = Vector3.forward;
				}
				if (Vector3.Dot(Vector3.back, player.transform.position - transform.position) > playerDot) {
					playerDot = Vector3.Dot(Vector3.back, player.transform.position - transform.position);
					targetDirection = Vector3.back;
				}

				ChangeDirections(targetDirection);
				break;
			default:
				targetDirection = Vector3.zero;
				break;
		}
	}

	private void UpdateStates () {
		//Sees player
		ShootRaycasts();

		//Inside wall
		if (Physics.CheckSphere(transform.position, 0.2f, LayerMask.GetMask("Wall"))) {
			insideWall = true;
		} else {
			insideWall = false;
		}

		//Phasing
		if (state == MonsterState.patrolling) {
			if (timeUntilPhase <= 0f) {
				canBeginPhasing = true;
			}
			timeUntilPhase -= Time.deltaTime;
			timePhasing = 0f;
		} else if (state == MonsterState.chasing) {
			timePhasing = 0f;
		} else if (state == MonsterState.phasing) {
			if (timePhasing < Time.deltaTime) {
				timeUntilPhase = Random.Range(7f, 12f);
			}

			if (timePhasing > 3f) {
				canBeginPhasing = false;
			}
			timePhasing += Time.deltaTime;
		}

		//Inflation
		stunned = inflationLevel > 0f;
		if (timeSinceLastInflated >= maxTimeSinceLastInflated && inflationLevel > 0f) {
			timeSinceLastInflated = maxTimeSinceLastInflated - 1f;
			Deflate();
		}
		timeSinceLastInflated += Time.deltaTime;

		anim.SetBool("seesPlayer", seesPlayer);
		anim.SetBool("canBeginPhasing", canBeginPhasing);
		anim.SetBool("insideWall", insideWall);
		anim.SetFloat("speed", speed);
		anim.SetBool("stunned", stunned);

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Patrolling")) {
			state = MonsterState.patrolling;
		} else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Chasing")) {
			state = MonsterState.chasing;
		} else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Phasing")) {
			state = MonsterState.phasing;
		} else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Stunned")) {
			state = MonsterState.stunned;
		}
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
