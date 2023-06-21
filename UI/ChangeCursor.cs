using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    //---> Definir la cantidad de cursores de tipo CursorType
    [SerializeField] private CursorType cursorEnter, defaultCursor;





    //---> Metodo para setear la textura (cursorEnter) para el cursor al entrar
    public void OnCursorEnter()
    {
        SetCursorEnter();
    }



    //---> Metodo para setear la textura (defaultCursor) para el cursor al salir
    public void OnCursorExit()
    {
        SetDefaultCursor();
    }



    //---> Metodo para setear la textura (defaultCursor) para el cursor al hacer click
    public void OnCursorClick()
    {
        SetDefaultCursor();
    }



    //---> Metodo para el cursor por defecto
    private void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor.cursorTexture, defaultCursor.cursorHotSpot, CursorMode.Auto);
    }



    //---> Metodo para el cursor de entrada
    private void SetCursorEnter()
    {
        Cursor.SetCursor(cursorEnter.cursorTexture, cursorEnter.cursorHotSpot, CursorMode.Auto);
    }



    //---> Metodo para setear la textura del cursor al Entrar en un Collider
    private void OnMouseEnter()
    {
        SetCursorEnter();
    }



    //---> Metodo para setear la textura del cursor al Salir de un Collider
    private void OnMouseExit()
    {
        SetDefaultCursor();
    }
}
