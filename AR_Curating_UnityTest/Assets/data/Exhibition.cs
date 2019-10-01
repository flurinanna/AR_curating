using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Exhibition
{
    public int id;
    public string Time;
    public DateTime Timestamp;
    public string Author;
    public string Title;
    public int Likes;
    public string PreviewImg;
    public string Email;
    public List<ExhibitionPicture> Pictures = new List<ExhibitionPicture>();
}
