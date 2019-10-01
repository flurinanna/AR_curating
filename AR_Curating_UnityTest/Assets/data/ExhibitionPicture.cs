
[System.Serializable]
public class ExhibitionPicture
{
    public Picture Picture;
    public PictureRotation Rotation;
    public PicturePosition Position;

    public ExhibitionPicture(Picture picture, PictureRotation rotation, PicturePosition position)
    {
        Picture = picture;
        Rotation = rotation;
        Position = position;
    }
}