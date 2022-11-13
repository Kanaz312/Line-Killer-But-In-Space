using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    private LineHandler _lineHandler = null;

    private void Start()
    {
        _lineHandler = FindObjectOfType<LineHandler>();
        if (_lineHandler == null)
        {
            Debug.LogError("Cutter did not find a LineHandler object");
        }

    }

    public void CutSection(int startIndex, Vector3 startPoint, int endIndex, Vector3 endPoint)
    {
        int numPoints = _lineHandler.GetNumPoints(),
            newIndexA = startIndex + 1,
            newIndexB = endIndex + 1;

        // Add point near end of the list first so 
        //   so that index numbers are preserved
        if (startIndex < endIndex)
        {
            _lineHandler.InsertPoint(newIndexB, endPoint);
            _lineHandler.InsertPoint(newIndexA, startPoint);

            // Inserting startPoint pushed endPoint one index later
            newIndexB++;
        }
        else
        {
            _lineHandler.InsertPoint(newIndexA, startPoint);
            _lineHandler.InsertPoint(newIndexB, endPoint);

            // Inserting endPoint pushed startPoint one index later
            newIndexA++;
        }

        // Optimize to check on side with less points?
        float areaAToB = _lineHandler.GetAreaBetweenIndeces(newIndexA, newIndexB),
            areaBToA = _lineHandler.GetAreaBetweenIndeces(newIndexB, newIndexA);
        if (areaAToB < areaBToA)
        {
            _lineHandler.RemoveIndices(_lineHandler.IncrementPointIndex(newIndexA)
                , newIndexB);
        }
        else
        {
            _lineHandler.RemoveIndices(_lineHandler.IncrementPointIndex(newIndexB)
                , newIndexA);
        }
    }
}
