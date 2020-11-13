using UnityEngine;

public class MainCamera : MonoBehaviour
{

    Portal[] portals;
    MinePortal[] minePortals;

    void Awake()
    {
        portals = FindObjectsOfType<Portal>();
        minePortals = FindObjectsOfType<MinePortal>();
    }

    void OnPreCull()
    {
        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].Render();
        }

        for (int i = 0; i < minePortals.Length; i++)
        {
            minePortals[i].Render();
        }

        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].PostPortalRender();
        }

    }

}