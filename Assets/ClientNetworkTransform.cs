using Unity.Netcode.Components;


public class NewBehaviourScript : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
