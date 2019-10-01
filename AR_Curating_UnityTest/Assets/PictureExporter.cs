using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureExporter : MonoBehaviour
{

    public Picture pictureInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public ExhibitionPicture GetExhibitionPicture()
    {
        Transform t = gameObject.transform;
        PicturePosition pos = new PicturePosition(t.position.x, t.position.y, t.position.z);
        PictureRotation rot = new PictureRotation(t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.w);
        ExhibitionPicture p = new ExhibitionPicture(pictureInfo, rot, pos);
        return p;
    }
}
