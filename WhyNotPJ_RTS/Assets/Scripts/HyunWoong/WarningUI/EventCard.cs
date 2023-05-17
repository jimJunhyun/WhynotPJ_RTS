using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventCard : MonoBehaviour
{
    Transform _receiver;
    Button _button;

    [SerializeField] private Transform _camRig;
    [SerializeField] private Transform _moveToPos;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private bool _isActive = false;

    private void Awake()
    {
        _camRig = Camera.main.transform;
        _receiver = FindObjectOfType<EventReceiver>().transform;
        _moveToPos = _receiver.transform;
        _button = GetComponent<Button>();

        _button.onClick.AddListener(() =>
        {
            MoveTo();
        });
    }

    float _xPos;
    float _yPos;
    float _zPos;

    public void MoveTo()
    {
        if (!_isActive)
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
                break;
            }
            else
            {
                _xPos = Mathf.Lerp(_camRig.position.x, _moveToPos.position.x, Time.deltaTime * _moveSpeed);
                _zPos = Mathf.Lerp(_camRig.position.z, _moveToPos.position.z, Time.deltaTime * _moveSpeed);

                _camRig.position = new Vector3(_xPos, _camRig.position.y, _zPos);
            }
            yield return null;
        }

        _isActive = false;
    }
}
