using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supercube : MonoBehaviour
{
    public Dimension dimensionTop;
    public Dimension dimensionBottom;
    public Dimension dimensionForward;
    public Dimension dimensionBack;
    public Dimension dimensionLeft;
    public Dimension dimensionRight;

    public bool openTop;
    public bool openBottom;
    public bool openForward;
    public bool openBack;
    public bool openLeft;
    public bool openRight;

    private void OnValidate()
    {
        SetupDimensionBoundariesFromBooleans();
    }

    public void SetupDimensionBoundariesFromBooleans()
    {
        // probably need to set all dimension colliders active, then just selectively turn them off for the ones that are checked.
        dimensionTop.SetAllBoundaryDirectionsActive(true);
        dimensionBottom.SetAllBoundaryDirectionsActive(true);
        dimensionForward.SetAllBoundaryDirectionsActive(true);
        dimensionBack.SetAllBoundaryDirectionsActive(true);
        dimensionLeft.SetAllBoundaryDirectionsActive(true);
        dimensionRight.SetAllBoundaryDirectionsActive(true);
        if (openTop)
        {
            dimensionTop.SetAllBoundaryDirectionsActive(false);
            dimensionBack.SetBoundaryDirectionActive(Dimension.BoundaryDirection.North, false);
            dimensionLeft.SetBoundaryDirectionActive(Dimension.BoundaryDirection.East, false);
            dimensionForward.SetBoundaryDirectionActive(Dimension.BoundaryDirection.South, false);
            dimensionRight.SetBoundaryDirectionActive(Dimension.BoundaryDirection.West, false);
        }
        if (openBottom)
        {
            dimensionBottom.SetAllBoundaryDirectionsActive(false);
            dimensionForward.SetBoundaryDirectionActive(Dimension.BoundaryDirection.North, false);
            dimensionRight.SetBoundaryDirectionActive(Dimension.BoundaryDirection.East, false);
            dimensionBack.SetBoundaryDirectionActive(Dimension.BoundaryDirection.South, false);
            dimensionLeft.SetBoundaryDirectionActive(Dimension.BoundaryDirection.West, false);
        }
        if (openForward)
        {
            dimensionForward.SetAllBoundaryDirectionsActive(false);
            dimensionTop.SetBoundaryDirectionActive(Dimension.BoundaryDirection.North, false);
            dimensionLeft.SetBoundaryDirectionActive(Dimension.BoundaryDirection.North, false);
            dimensionRight.SetBoundaryDirectionActive(Dimension.BoundaryDirection.South, false);
            dimensionBottom.SetBoundaryDirectionActive(Dimension.BoundaryDirection.South, false);
        }
        if (openBack)
        {
            dimensionBack.SetAllBoundaryDirectionsActive(false);
            dimensionRight.SetBoundaryDirectionActive(Dimension.BoundaryDirection.North, false);
            dimensionBottom.SetBoundaryDirectionActive(Dimension.BoundaryDirection.North, false);
            dimensionLeft.SetBoundaryDirectionActive(Dimension.BoundaryDirection.South, false);
            dimensionTop.SetBoundaryDirectionActive(Dimension.BoundaryDirection.South, false);
        }
        if (openRight)
        {
            dimensionRight.SetAllBoundaryDirectionsActive(false);
            dimensionForward.SetBoundaryDirectionActive(Dimension.BoundaryDirection.East, false);
            dimensionTop.SetBoundaryDirectionActive(Dimension.BoundaryDirection.East, false);
            dimensionBack.SetBoundaryDirectionActive(Dimension.BoundaryDirection.West, false);
            dimensionBottom.SetBoundaryDirectionActive(Dimension.BoundaryDirection.West, false);
        }
        if (openLeft)
        {
            dimensionLeft.SetAllBoundaryDirectionsActive(false);
            dimensionBottom.SetBoundaryDirectionActive(Dimension.BoundaryDirection.East, false);
            dimensionBack.SetBoundaryDirectionActive(Dimension.BoundaryDirection.East, false);
            dimensionForward.SetBoundaryDirectionActive(Dimension.BoundaryDirection.West, false);
            dimensionTop.SetBoundaryDirectionActive(Dimension.BoundaryDirection.West, false);
        }
    }
}
