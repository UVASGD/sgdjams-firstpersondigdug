using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ExitHandler : MonoBehaviour
{
	public float secondsToExit = 2f;
	private float currentSeconds;
	public TMP_Text text;
	private int periodCount;
	private float opacity;
    // Start is called before the first frame update
    void Start()
    {
		currentSeconds = 0f;
		periodCount = 0;
		opacity = -2f;
		InvokeRepeating("TextUpdate", 0f, 0.4f);
		text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
		StartCoroutine(Fade());
	}

	void TextUpdate () {
		text.text = "Exiting";
		for (int i = 0; i < periodCount; i++) {
			text.text += ".";
		}
		periodCount++;
		periodCount %= 4;
	}

	IEnumerator Fade () {
		Color unfadedColor = new Color(text.color.r, text.color.g, text.color.b, 1f);
		Color fadedColor = new Color(text.color.r, text.color.g, text.color.b, 0f);
		float t = opacity;
		while (true) {
			text.color = Color.Lerp(fadedColor, unfadedColor, t);
			float direction = Mathf.Sign(opacity - t);

			if (direction > 0f) {
				if (t < 1f) {
					t += Time.deltaTime;
				} else {
					t = 1f;
				}
			} else {
				if (t > 0f) {
					t -= Time.deltaTime;
				} else {
					t = 0f;
				}
			}
			t = Mathf.Clamp01(t);
			yield return new WaitForEndOfFrame();
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			opacity = 2f;
		}
		if (Input.GetKeyUp(KeyCode.Escape)) {
			opacity = -1f;
		}

		if (Input.GetKey(KeyCode.Escape)) {
			if (currentSeconds > secondsToExit) {
                GameManager.Instance.MainMenu();
			}
			currentSeconds += Time.deltaTime;
		} else {
			currentSeconds = 0f;
		}
    }
}
