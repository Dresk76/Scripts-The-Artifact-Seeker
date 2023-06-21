using UnityEngine;

//---> Para poder crear estos ScriptableObject desde el editor se agrega esta linea
[CreateAssetMenu]

//---> Clase para definir la plantilla con los datos que queremos que tengan los cursores que 
//---> se van a ir creando
public class CursorType : ScriptableObject
{
    [Header("DATOS PARA EL CURSOR")]
    public Texture2D cursorTexture;
    public Vector2 cursorHotSpot;
}
