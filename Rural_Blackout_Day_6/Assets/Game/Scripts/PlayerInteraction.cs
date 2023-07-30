using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
	[SerializeField] Camera raycastCamera;
	[SerializeField] GameObject promptUI;
	[SerializeField] Image promptIcon;
	[SerializeField] TMP_Text promptText;

	Interactable target = null;

	bool pressing = false;

	[SerializeField]
	private GameObject _carriedPlankUI;

	[SerializeField]
	public int _planksCollectedCount = 0;

	[SerializeField]
	public int _maxPlanks = 1;


	void Update() 
	{
        if (IntroVideo.Instance.IntroPlaying)
        {
			return;
        }


		// Planks UI
		if(_planksCollectedCount > 0) {
			_carriedPlankUI.SetActive(true);
		} else {
			_carriedPlankUI.SetActive(false);
		}

		// Interactions
		if(Physics.Raycast(raycastCamera.ViewportPointToRay(Vector2.one * 0.5f), out RaycastHit hit, 3f, -1))
		{
			
			if(hit.collider.TryGetComponent(out Interactable interactable))
			{
				target = interactable;
				target.OnPromptTextChanged += OnTargetPromptTextChanged;

				if (_planksCollectedCount > 0 || target.GetComponent<WindowBolt>() == null) {
					Focus();
				} else {
					LoseFocus();
				}

				if(_planksCollectedCount >= _maxPlanks && target.GetComponent<PlanksOnGround>() != null) {
					promptText.text = "You cannot carry more";
				}

			}
			else
			{
				if (target != null)
				{
					LoseFocus();
				}
				if (promptUI.activeSelf)
				{
					promptUI.SetActive(false);

				}
			}
		}
		else
		{
			if (target != null)
			{
				LoseFocus();
			}
			if (promptUI.activeSelf)
			{
				promptUI.SetActive(false);

			}
		}
		
		if (target != null)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				pressing = true;
				target.OnPress();
			}
			else if(Input.GetKeyUp(KeyCode.E))
			{
				pressing = false;
				target.OnRelease();
			}
		}
	}

	void Focus()
	{
		promptUI.SetActive(true);
		promptIcon.sprite = target.GetIcon();
		promptText.text = target.GetPromptText();
	}

	void OnTargetPromptTextChanged()
	{
		// idk why this is null
		if(target != null)
		{
			promptText.text = target.GetPromptText();
		}
	}

	void LoseFocus()
	{
		if (target!=null)
		{
			target.OnPromptTextChanged -= OnTargetPromptTextChanged;

			if (pressing)
			{
				target.OnRelease();
				pressing = false;
			}
			target = null;
		}

		promptUI.SetActive(false);


	}
}
