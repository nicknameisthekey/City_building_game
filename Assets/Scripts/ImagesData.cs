using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName = "new sprite collection", menuName ="create sprite collection", order =51)]
public class ImagesData : ScriptableObject
{
    [SerializeField] List<Sprite> sprites;
    public List<Sprite> Sprites { get => sprites; }
}
