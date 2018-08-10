using UnityEngine;

public class AnimFinished4Animator : MonoBehaviour
{
	public bool IsFinished { get; private set; }
	public void AnimationFinish ()
	{
		IsFinished = true;
	}
}