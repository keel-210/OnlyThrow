using UnityEngine;

public interface IThrowable
{
	void Catch (Transform player);
	void Throw (Vector2 Direction);
}