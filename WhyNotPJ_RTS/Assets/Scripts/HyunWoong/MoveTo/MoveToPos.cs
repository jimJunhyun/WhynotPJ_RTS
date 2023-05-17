using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToPos : MonoBehaviour
{
    [SerializeField] private Button _moveToBtn;

    [SerializeField] private Transform _camRig;
    [SerializeField] private Transform _moveToPos;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _isActive = false;

    float _xPos;
    float _yPos;
    float _zPos;

    private void Update()
    {
        if(Vector3.Distance(_camRig.position, _moveToPos.position) >= 1f)
        {
            _moveToBtn.interactable = true;
        }
    }

    public void MoveTo()
    {
        if( !_isActive)
        {
            _isActive = true;
            StartCoroutine(MoveCam());
        }
    }

    private IEnumerator MoveCam()
    {
        while (true)
        {
            if (Vector3.Distance(_camRig.position, _moveToPos.position) <= .1f)
            {
                _camRig.position = _moveToPos.position;
                _moveToBtn.interactable = false;
                break;
            }
            else
            {
                _xPos = Mathf.Lerp(_camRig.position.x, _moveToPos.position.x, Time.deltaTime * _moveSpeed);
                _yPos = Mathf.Lerp(_camRig.position.y, _moveToPos.position.y, Time.deltaTime * _moveSpeed);
                _zPos = Mathf.Lerp(_camRig.position.z, _moveToPos.position.z, Time.deltaTime * _moveSpeed);

                _camRig.position = new Vector3(_xPos, _yPos, _zPos);
            }
            yield return null;
        }

        _isActive = false;
    }
}
