using UnityEngine;
using UnityEngine.VFX;

public class TeleporterChild : MonoBehaviour
{
    VisualEffect myChildVFX;
    TeleporterParent teleporterParent;
    private void Awake()
    {
        myChildVFX = transform.GetChild(0).GetComponent<VisualEffect>();
        teleporterParent = transform.parent.GetComponent<TeleporterParent>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (GameMaster.GetGameObjectID(other.gameObject) == -1)
            return;
        teleporterParent.onCollisionEnterChildren(transform,other.gameObject.transform);
    }

    public void VFXColorSetting(Color getColor)
    {
        Debug.Log(getColor);
        Debug.Log(myChildVFX.name);
        myChildVFX.SetVector4("ThisVFXColor",getColor);
    }
    public void VFXBoolSetting(bool getBool)//�e��Dicitonary,bool[]����utrue�v,�ufalse�v���擾�B
    {
        myChildVFX.gameObject.SetActive(getBool);
    }
}
