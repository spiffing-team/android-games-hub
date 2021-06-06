using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text text = default;

    [SerializeField] private bool displayAll = true;
    [HideIf("displayAll")]
    [SerializeField] private PointsDatabase.Field pointsField = default;
    private void Start()
    {
        if (displayAll)
            DisplayPoints(PointsDatabase.SumAllPoints());
        else
            DisplayPoints(PointsDatabase.Load(pointsField));
    }

    private void DisplayPoints(int points)
    {
        text.text = points.ToString();
    }
}
