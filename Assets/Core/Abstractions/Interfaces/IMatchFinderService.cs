using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public interface IMatchFinderService
{
    IReadOnlyList<Match> FindMatches(GridModel Grid);
}
