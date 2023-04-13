using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : MonoBehaviour
{
    public IProducable producing;
	public bool isProducing = false;
	public bool pSide = false;

	public void SetProduct(IProducable pro)
	{
		
		producing = pro;
		Produce();
	} 

    void Produce()
	{
		if (!isProducing && producing != null)
		{
			StartCoroutine(DelayMake(producing));
		}
		
	}

	IEnumerator DelayMake(IProducable product)
	{
		isProducing = true;
		yield return new WaitForSeconds(product._produceTime);
		product._onCompleted.Invoke();
		producing = null;
		
		isProducing = false;
	}
}
