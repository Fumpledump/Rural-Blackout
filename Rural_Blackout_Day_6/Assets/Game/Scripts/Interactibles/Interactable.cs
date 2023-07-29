using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	[SerializeField] protected Sprite icon;
	[SerializeField] protected string promptText;
	public event Action OnPromptTextChanged;

	public virtual void OnPress()
	{

	}

	public virtual void OnRelease()
	{

	}

	public Sprite GetIcon()
	{
		return icon;
	}

	public string GetPromptText()
	{
		return promptText;
	}

	protected void SetPromptText(string newText)
	{
		promptText = newText;
		OnPromptTextChanged?.Invoke();
	}
}
