using UnityEngine;

public interface IThrowable
{
	IThrowable Catch (Transform player);
	void Throw (Vector2 Direction);
}