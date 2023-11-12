using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrailRenderer : MonoBehaviour
{
    private Transform _transform;

    // �O�t���[���̃��[���h�ʒu
    private Vector2 _prevPosition;
    public Transform _myTeamPlayer;

    private void Start()
    {
        _transform = transform;

        _prevPosition = _transform.position;
    }
    private void Update()
    {
        // ���݃t���[���̃��[���h�ʒu
        Vector2 position = _transform.position;

        // �ړ��ʂ��v�Z
        var delta = position - _prevPosition;

        // ����Update�Ŏg�����߂̑O�t���[���ʒu�X�V
        _prevPosition = position;

        // �Î~���Ă����Ԃ��ƁA�i�s���������ł��Ȃ����߉�]���Ȃ�
        if (delta == Vector2.zero)
            return;

        // �i�s�����i�ړ��ʃx�N�g���j�Ɍ����悤�ȃN�H�[�^�j�I�����擾
        var rotation = Quaternion.LookRotation(-delta, Vector3.up);

        // �I�u�W�F�N�g�̉�]�ɔ��f
        _transform.rotation = rotation;
        //_transform.RotateAround(_myTeamPlayer,Vector);
    }
}
