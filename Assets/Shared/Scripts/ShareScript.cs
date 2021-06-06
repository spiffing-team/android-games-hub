using System.Collections;
using EasyMobile;
using UnityEngine;

public class ShareScript : MonoBehaviour
{
    public void Share()
    {
        StartCoroutine(CaptureScreenshot());
    }

    IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = Sharing.CaptureScreenshot();
        Sharing.ShareTexture2D(texture, "HighwayRacer_screenshot", "Patrz ile mam punktów!");
    }
}
