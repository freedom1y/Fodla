using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
	[Tooltip("In ms^-1")][SerializeField] float controllSpeed = 33f;
	[Tooltip("In m")][SerializeField] float xRange = 20f;
	[Tooltip("In m")][SerializeField] float yRange = 12f;
    [SerializeField] GameObject[] guns;

    [Header("Screen-position Based")]
    [SerializeField] float positionPitchFactor = -1.25f;
    [SerializeField] float positionYawFactor = 1.25f;

    [Header("Controll-throw Based")]
    [SerializeField] float controlPitchFactor = -20f;  
	[SerializeField] float controlRollFactor = -20f;

	float xThrow,yThrow;
    bool isControlEnabled = true;


    void Update ()
    {
        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
	}

    // 文字列参照によって呼び出される
    void OnPlayerDeath()
    {
        isControlEnabled = false;
    }

	private void ProcessRotation()
    {
		float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
		float pitchDueToControlThrow = yThrow * controlPitchFactor;
		float pitch = pitchDueToPosition + pitchDueToControlThrow;

		float yaw = transform.localPosition.x * positionYawFactor;

		float roll = xThrow * controlRollFactor;

		// 3つの角度の値を X、Y、Z 軸に順に当てはめて回転を表す
		transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
	}

	private void ProcessTranslation()
    {
		// Horizontal、Verticalの各項目がそれぞれどのキーと対応するのか、などの設定をInputManagerで行う
		xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
		yThrow = CrossPlatformInputManager.GetAxis("Vertical");

		float xOffset = xThrow * controllSpeed * Time.deltaTime;
		float yOffset = yThrow * controllSpeed * Time.deltaTime;

		float rawXPos = transform.localPosition.x + xOffset;
		float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);
		// -xRange <= rawXPos <= xRange
		float rawYPos = transform.localPosition.y + yOffset;
		float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);
		// -yRange <= rawYPos <= yRange

		transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
	}

    void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            SetGunsActive(true);
        }
        else
        {
            SetGunsActive(false);
        }
    }

    private void SetGunsActive(bool isActive)
    {
        foreach(GameObject gun in guns)
        {
            var emissionModule = gun.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
