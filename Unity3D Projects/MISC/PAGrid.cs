using System.Collections.Generic;
using UnityEngine;

public class PAGrid
{
    private readonly int[,] GridTiles;
    private int[,] MatchedPieces, SpecialMatches;
    private bool[,] IsBomb;
    private readonly piece[,] GridPieces;
    private List<match> CurrentMatches;
    private int MatchedPieceCount;

    private readonly Stack<piece> UnusedPieces;
    //public Stack<match> UnusedMatches;

    private readonly Vector3 GridCentrePos;

    private readonly float tileSize;
    private readonly float fallTime;

    public PAGrid(PALevel l, Vector3 centre)
    {
        GridCentrePos = centre;

        GridTiles = new int[variables.GridWidth, variables.GridHeight];

        GridPieces = new piece[variables.GridWidth, variables.GridHeight];
        CurrentMatches = new List<match>();

        UnusedPieces = new Stack<piece>();
        //UnusedMatches = new Stack<match>();
        MatchedPieces = new int[variables.GridWidth, variables.GridHeight];
        SpecialMatches = new int[variables.GridWidth, variables.GridHeight];

        tileSize = variables.TileSize / 100f;
        fallTime = variables.TileFallTime;
    }

    bool IsTileBelowEmpty(int x, int y)
    {
        if (x > variables.GridWidth - 1) return false;
        if (x < 0) return false;

        if (y > variables.GridHeight - 1) return false;
        if (y < 0) return false;

        return GridTiles[x, y - 1] == (int)GridTileTypes.normal;
    }

    public void MovePiece(coords from, coords to)
    {
        piece targetpiece = GridPieces[to.x, to.y];
        piece sourcepiece = GridPieces[from.x, from.y];

        if (!IsGridTileEmpty(to.x, to.y)) return;
        if (!IsGridTileEmpty(from.x, from.y)) return;


        //if (targetpiece != null) targetpiece.SnapToPosition(GridPosToVector3(from.x, from.y, GridBottomLeft, tileSize));
        if (targetpiece != null)
        {
            targetpiece.SlideToPosition(GridTools.GridCellToVector3(from.x, from.y, GridCentrePos), 0.1f);
            //targetpiece.SnapToPosition(GridPosToVector3(from.x, from.y, GridBottomLeft, tileSize));
        }

        if (sourcepiece != null)
        {
            sourcepiece.SnapToPosition(GridTools.GridCellToVector3(to.x, to.y, GridCentrePos));
            // TODO:SFX ONESHOTS events.RequestOneshotSFX(SFXOneShots.movedpiece, sourcepiece.PieceGraphics.position);
        }

        GridPieces[to.x, to.y] = sourcepiece;
        GridPieces[from.x, from.y] = targetpiece;

        //CheckPieceMatches();
    }

    public bool IsGridTileEmpty(Vector3 pos)
    {
        coords place = GridTools.Vector3ToGridCell(pos, GridCentrePos);

        return GridTiles[place.x, place.y] == 0;
    }

    public bool IsGridTileEmpty(int x, int y)
    {
        return GridTiles[x, y] == 0;
    }

    public bool IsPieceNull(int x, int y)
    {
        if (x > variables.GridWidth - 1) return true;
        if (y > variables.GridHeight - 1) return true;

        return GridPieces[x, y] == null;
    }

    public bool AreCoordinatesEmpty(int x, int y)
    {
        return IsGridTileEmpty(x, y) && IsPieceNull(x, y);
    }

    public bool CanMoveIntoTile(int x, int y)
    {
        if (GridPieces[x, y + 1] == null) return false;

        return IsGridTileEmpty(x, y);
    }

    public bool IsTopRowClear()
    {
        return IsRowClear(variables.GridHeight - 1);
    }

    bool IsRowClear(int h)
    {
        bool returner = true;

        for (int i = 0; i < variables.GridWidth; i++)
        {
            if (GridPieces[i, h] != null) returner = false;
        }

        return returner;
    }

    piece CreatePiece(int x, int y)
    {
        piece p;

        if (UnusedPieces.Count > 0)
        {
            p = UnusedPieces.Pop();
        }
        else
        {
            p = new piece();
        }

        p.InitializePiece();
        p.SetRandomColour();
        Vector3 pos = GridTools.GridCellToVector3(x, y, GridCentrePos);
        //Debug.Log("creating piece at " + pos);
        p.SnapToPosition(pos);
        //p.FallToPosition(pos, fallTime);

        p.PieceGraphics.gameObject.SetActive(true);

        return p;
    }

    piece CreatePiece(int x, int y, PieceColours c)
    {
        piece z = CreatePiece(x, y);
        z.SetPieceColour(c);
        return z;
    }

    public void CreateTopRow()
    {
        int h = variables.GridHeight - 1;
        //events.RequestOneshotSFX(SFXOneShots.movedpiece, Vector3.zero);

        for (int w = 0; w < variables.GridWidth; w++)
        {
            if (GridTiles[w, h] != 0) continue;

            piece p;

            if (UnusedPieces.Count > 0)
            {
                p = UnusedPieces.Pop();
            }
            else
            {
                p = new piece();
            }

            p.InitializePiece();
            Vector3 pos = GridTools.GridCellToVector3(w, h, GridCentrePos);
            p.SnapToPosition(pos + Vector3.up * tileSize);
            p.FallToPosition(pos, fallTime);

            GridPieces[w, h] = p;
        }
    }
    public void CheckPieceGravity(int x, int y)
    {
        if (y == 0) return;

        if (IsTileBelowEmpty(x, y) && IsPieceNull(x, y - 1))
        {
            if (!GridPieces[x, y].IsPieceFalling)
            {
                GridPieces[x, y].FallToPosition(GridTools.GridCellToVector3(x, y - 1, GridCentrePos), fallTime);

                GridPieces[x, y - 1] = GridPieces[x, y];
                GridPieces[x, y] = null;
            }
        }
    }

    public void CreatePieceSet(int[,] colours)
    {

        for (int h = 0; h < variables.GridHeight; h++)
        {
            for (int w = 0; w < variables.GridWidth; w++)
            {
                if (GridPieces[w, h] == null)
                {
                    GridPieces[w, h] = CreatePiece(w, h, (PieceColours)colours[w, h]);
                }
                else
                {
                    Debug.LogError("Creating a piece set with a non-null grid. Nevah ain't not supposed to happen");
                }
            }
        }
    }

    public void ScrambleAllPieces()
    {
        Debug.Log("triggering piece scramble");

        for (int h = 1; h < variables.GridHeight; h++)
        {
            for (int w = 0; w < variables.GridWidth; w++)
            {
                if (GridPieces[w, h] != null) GridPieces[w, h].SetRandomColour();
            }
        }

        CheckPieceMatches();
    }


    public void GeneralGravityCheck()
    {
        for (int h = 1; h < variables.GridHeight; h++)
        {
            for (int w = 0; w < variables.GridWidth; w++)
            {
                if (GridPieces[w, h] != null) CheckPieceGravity(w, h);
            }
        }

        CheckPieceMatches();
    }

    public void GeneralGravityCheck(ref int pieceCount)
    {
        pieceCount = 0;

        for (int h = 1; h < variables.GridHeight; h++)
        {
            for (int w = 0; w < variables.GridWidth; w++)
            {
                if (GridPieces[w, h] != null)
                {
                    CheckPieceGravity(w, h);
                    pieceCount++;
                }
            }
        }

        CheckPieceMatches();
    }

    public void CheckPieceMatches()
    {
        ResetAllPieceStates();

        for (int h = 0; h < variables.GridHeight; h++)
        {
            for (int w = 0; w < variables.GridWidth; w++)
            {
                if (IsPieceNull(w, h)) continue; //no piece, no match
                if (!IsGridTileEmpty(w, h)) continue; //obstacle, no match. duh
                if (GridPieces[w, h].IsPieceFalling) continue;
                //if (IsTileBelowEmpty(w, h)) continue;

                CheckPieceHorizontals(w, h);
                CheckPieceVericals(w, h);
            }
        }
    }

    public void UpdateMatchStates()
    {
        for (int i = 0; i < variables.GridHeight; i++)
        {
            for (int j = 0; j < variables.GridWidth; j++)
            {
                if (GridPieces[j, i] == null) continue;
                //if (IsTileBelowEmpty(j, i)) continue;
                if (GridPieces[j, i].IsPieceFalling) continue;

                if (GridPieces[j, i].MatchState != (PieceMatchState)MatchedPieces[j, i])
                {
                    //Debug.Log("updating match state " + GridPieces[j, i].MatchState + " to " + (PieceMatchState)MatchedPieces[j, i] + " at " + Time.time);
                    GridPieces[j, i].SetMatchState((PieceMatchState)MatchedPieces[j, i]);
                }
            }
        }
    }

    void CheckPieceHorizontals(int x, int y)
    {
        piece p = GridPieces[x, y];

        //if (p.MatchState == PieceMatchState.horizontal || p.MatchState == PieceMatchState.both)
        //{
        //    MatchedPieces[x, y] = (int)p.MatchState;
        //    return;
        //}

        if (CheckNextHorizontals(x, y))
        {
            //match m = UnusedMatches.Count > 0 ? UnusedMatches.Pop() : new match();

            match m = new match();

            m.SetType(MatchTypes.horizontal);
            m.AddCoords(new coords(x, y));


            MatchedPieces[x, y] = (int)PieceMatchState.horizontal;

            for (int i = x + 1; i < variables.GridWidth; i++)
            {
                piece pc = GridPieces[i, y];

                if (pc == null) break;
                if (GridTiles[i, y] != 0) break;
                if (pc.PieceColour != p.PieceColour)
                {
                    break;
                }

                if (pc.PieceColour == p.PieceColour)
                {
                    MatchedPieces[i, y] = (int)PieceMatchState.horizontal;
                    m.AddCoords(new coords(i, y));
                }
                else
                {
                    break;
                }

            }

            CurrentMatches.Add(m);
            MatchedPieceCount += m.Members.Count;
        }
    }

    public void ResetAllPieceStates()
    {

        MatchedPieces = new int[variables.GridWidth, variables.GridHeight];
        SpecialMatches = new int[variables.GridWidth, variables.GridHeight];
        for (int i = 0; i < CurrentMatches.Count; i++)
        {
            CurrentMatches[i].ReleaseStuff();
            //UnusedMatches.Push(CurrentMatches[i]);
        }

        CurrentMatches.Clear();
        MatchedPieceCount = 0;
    }

    void CheckPieceVericals(int x, int y)
    {

        if (MatchedPieces[x, y] == (int)PieceMatchState.vertical || MatchedPieces[x, y] == (int)PieceMatchState.both)
        {
            return;
        }

        if (CheckNextVerticals(x, y))
        {

            piece p = GridPieces[x, y];

            //match m = UnusedMatches.Count > 0 ? UnusedMatches.Pop() : new match();

            match m = new match();

            m.SetType(MatchTypes.vertical);
            m.AddCoords(new coords(x, y));



            MatchedPieces[x, y] = MatchedPieces[x, y] == (int)PieceMatchState.unmatched
                ? (int)PieceMatchState.vertical
                : (int)PieceMatchState.both;

            for (int i = y + 1; i < variables.GridHeight; i++)
            {
                piece pc = GridPieces[x, i];

                if (GridTiles[x, i] != 0) break;
                if (pc == null) break;

                if (pc.PieceColour == p.PieceColour)
                {
                    MatchedPieces[x, i] = MatchedPieces[x, i] == (int)PieceMatchState.unmatched
                        ? (int)PieceMatchState.vertical
                        : (int)PieceMatchState.both;
                    m.AddCoords(new coords(x, i));
                }
                else
                {
                    break;
                }

            }

            CurrentMatches.Add(m);
            MatchedPieceCount += m.Members.Count;
        }
    }

    public bool CheckNextVerticals(int x, int y)
    {
        if (x > variables.GridWidth - 1) return false;
        if (y > variables.GridHeight - 1) return false;

        if (IsPieceNull(x, y + 1)) return false;
        if (IsPieceNull(x, y + 2)) return false;

        if (MatchedPieces[x, y] == (int)PieceMatchState.vertical || MatchedPieces[x, y] == (int)PieceMatchState.both)
            return false;

        if (MatchedPieces[x, y + 1] == (int)PieceMatchState.vertical || MatchedPieces[x, y + 1] == (int)PieceMatchState.both)
            return false;

        if (MatchedPieces[x, y + 2] == (int)PieceMatchState.vertical || MatchedPieces[x, y + 2] == (int)PieceMatchState.both)
            return false;


        return GridPieces[x, y].PieceColour == GridPieces[x, y + 1].PieceColour &&
               GridPieces[x, y].PieceColour == GridPieces[x, y + 2].PieceColour;
    }

    public bool CheckNextHorizontals(int x, int y)
    {

        if (x > variables.GridWidth - 1) return false;
        if (y > variables.GridHeight - 1) return false;

        if (IsPieceNull(x + 1, y)) return false;
        if (IsPieceNull(x + 2, y)) return false;


        if (MatchedPieces[x, y] == (int)PieceMatchState.horizontal || MatchedPieces[x, y] == (int)PieceMatchState.both)
            return false;

        if (MatchedPieces[x + 1, y] == (int)PieceMatchState.horizontal || MatchedPieces[x + 1, y] == (int)PieceMatchState.both)
            return false;

        if (MatchedPieces[x + 2, y] == (int)PieceMatchState.horizontal || MatchedPieces[x + 2, y] == (int)PieceMatchState.both)
            return false;

        return GridPieces[x, y].PieceColour == GridPieces[x + 1, y].PieceColour &&
               GridPieces[x, y].PieceColour == GridPieces[x + 2, y].PieceColour;
    }

    public int ScoreToDamage(int score)
    {
        return (score + Mathf.Clamp(score - 5, 0, 4) * 2);
    }

    public void SendScoreMessage(int count, PieceColours col)
    {

        events.ReportScore(ScoreToDamage(count), col);
        //Debug.Log("processing score add: " + count + " actual points: " + score + " at " + Time.time);
        //send count*slash event
    }

    public List<match> ConsolidatedMatches()
    {
        if (CurrentMatches.Count > 1)
        {
            List<match> newMatches = new List<match>();

            foreach (match match in CurrentMatches)
            {
                if (match.MatchType == MatchTypes.none) continue;

                foreach (match currentMatch in CurrentMatches)
                {
                    if (match == currentMatch) continue;
                    if (currentMatch.Members.Count == 0) continue;
                    if (currentMatch.MatchType == MatchTypes.none) continue;

                    if (match.IntersectsMatch(currentMatch))
                    {
                        match.AddMatch(currentMatch);
                        currentMatch.SetType(MatchTypes.none);
                    }

                    if (!newMatches.Contains(match)) newMatches.Add(match);
                }
            }

            return newMatches;
        }

        return null;
    }

    public void ConsolidateMatches()
    {
        if (CurrentMatches.Count > 1)
        {
            List<match> newMatches = new List<match>();

            foreach (match match in CurrentMatches)
            {
                if (match.MatchType == MatchTypes.none) continue;

                foreach (match currentMatch in CurrentMatches)
                {
                    if (match == currentMatch) continue;
                    if (currentMatch.Members.Count == 0) continue;
                    if (currentMatch.MatchType == MatchTypes.none) continue;

                    if (match.IntersectsMatch(currentMatch))
                    {
                        match.AddMatch(currentMatch);
                        currentMatch.SetType(MatchTypes.none);
                    }

                    if (!newMatches.Contains(match)) newMatches.Add(match);
                }
            }

            CurrentMatches = newMatches;

        }

    }

    public void ProcessAllMatches()
    {
        ConsolidateMatches();

        for (int i = 0; i < CurrentMatches.Count; i++)
        {
            match m = CurrentMatches[i];

            if (m.MatchType == MatchTypes.none) continue;

            bool givepoints = true;

            if (IsMatchPierce(m))
            {
                events.ReportSpecial(PieceStates.pierce, m.Members[0]);
                givepoints = false;
            }

            if (IsMatchSlash(m))
            {
                events.ReportSpecial(PieceStates.slash, m.Members[0]);
                givepoints = false;
            }

            if (IsMatchScramble(m))
            {
                Debug.Log("found scramble match");
                events.ReportSpecial(PieceStates.scramble, m.Members[0]);
                events.RequestOneShotFX(OneShotFXTypes.reflectongrid, GridTools.GridCellToVector3(m.Members[0].x, m.Members[0].y));
                givepoints = false;
            }

            if (IsMatchHealingCross(m))
            {
                Debug.Log("found healing match");
                events.RequestOneShotFX(OneShotFXTypes.healongrid, GridTools.GridCellToVector3(m.Members[0].x, m.Members[0].y));
                events.ReportSpecial(PieceStates.heal, m.Members[0]);
                givepoints = false;
            }

            if (!givepoints) continue;

            if (m.MatchType == MatchTypes.horizontal || m.MatchType == MatchTypes.vertical ||
                m.MatchType == MatchTypes.both)
            {
                if (CurrentMatches[i].Members == null) continue;
                if (CurrentMatches[i].Members.Count == 0) continue;

                int score = CurrentMatches[i].Members.Count;

                SendScoreMessage(score, GridPieces[m.Members[0].x, m.Members[0].y].PieceColour);
            }
        }

        foreach (match match in CurrentMatches)
        {
            ClearMatch(match);
            match.ReleaseStuff();
        }

    }

    public void ProcessSpecialMatches()
    {
        IsBomb = new bool[variables.GridWidth, variables.GridHeight];

        for (int y = 0; y < variables.GridHeight; y++)
        {
            for (int x = 0; x < variables.GridWidth; x++)
            {
                if (MatchedPieces[x, y] != 0)
                {
                    if (GridPieces[x, y] != null)
                    {
                        if (IsMatchExplosive(x, y))
                        {
                            SpecialMatches[x, y] = 1;

                            for (int h = -1; h < 2; h++)
                            {
                                for (int w = -1; w < 2; w++)
                                {
                                    IsBomb[x + w, y + h] = true;
                                }
                            }

                            //Debug.Log("set match state " + x + "," + y + " to explosive");
                        }

                        if (IsMatchPierce(x, y))
                        {
                            GridPieces[x, y].SetPieceState(PieceStates.pierce);

                            //SpecialMatches[x, y] = 2;
                            Debug.Log("set match state " + x + "," + y + " to pierce");
                        }

                        if (IsMatchSlash(x, y))
                        {
                            GridPieces[x, y].SetPieceState(PieceStates.slash);
                            //SpecialMatches[x, y] = 3;
                            //Debug.Log("set match state " + x + "," + y + " to slash");
                        }


                    }
                }
            }
        }
    }

    public bool IsMatchPierce(match m)
    {

        foreach (coords coords in m.Members)
        {
            if (IsMatchPierce(coords.x, coords.y)) return true;
        }

        return false;
    }

    public bool IsMatchSlash(match m)
    {
        foreach (coords coords in m.Members)
        {
            if (IsMatchSlash(coords.x, coords.y))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsMatchHealingCross(match m)
    {
        foreach (coords coords in m.Members)
        {
            if (IsMatchHealingCross(coords.x, coords.y))
            {
                return true;
            }
        }

        return false;
    }

    bool IsMatchScramble(match m)
    {
        foreach (coords c in m.Members)
        {
            if (IsMatchScramble(c.x, c.y))
            {
                return true;
            }
        }

        return false;
    }

    bool IsMatchPierce(int x, int y)
    {

        if (x != 0) return false;

        piece p = GridPieces[x, y];

        for (int w = 0; w < variables.GridWidth; w++)
        {
            if (IsPieceNull(w, y)) return false;
            if (p.PieceColour != GridPieces[w, y].PieceColour) return false;
        }

        return true;
    }

    bool IsMatchSlash(int x, int y)
    {

        if (y != 0) return false;

        piece p = GridPieces[x, y];

        for (int h = 0; h < variables.GridHeight; h++)
        {
            if (IsPieceNull(x, h)) return false;
            if (p.PieceColour != GridPieces[x, h].PieceColour) return false;
        }


        return true;

    }

    bool IsMatchHealingCross(int x, int y)
    {
        if (x < 2) return false;
        if (x > variables.GridWidth - 2) return false;
        if (y < 2) return false;
        if (y > variables.GridHeight - 2) return false;

        piece p = GridPieces[x, y];

        if (p == null) return false;

        //TODO: come back to this and make a json file with coordinate offsets, created from editor - more easily extendible
        return (CheckPieceMatch(x, y, 0, 1) &&
                CheckPieceMatch(x, y, 0, 2) &&
                CheckPieceMatch(x, y, 0, -1) &&
                CheckPieceMatch(x, y, 0, -2) &&
                CheckPieceMatch(x, y, 1, 0) &&
                CheckPieceMatch(x, y, 2, 0) &&
                CheckPieceMatch(x, y, -1, 0) &&
                CheckPieceMatch(x, y, -2, 0));

    }

    bool IsMatchScramble(int x, int y)
    {
        if (x < 3) return false;
        if (x == variables.GridWidth - 3) return false;
        if (y < 1) return false;
        if (y == variables.GridHeight - 1) return false;

        return (CheckPieceMatch(x, y, 0, 1) &&
                CheckPieceMatch(x, y, 0, -1) &&
                CheckPieceMatch(x, y, -1, 1) &&
                CheckPieceMatch(x, y, -2, 1) &&
                CheckPieceMatch(x, y, 1, -1) &&
                CheckPieceMatch(x, y, 2, -1));

    }

    bool CheckPieceMatch(int x, int y, int offsetx, int offsety)
    {
        if (IsPieceNull(x + offsetx, y + offsety)) return false;

        if (GridPieces[x, y] == null) return false;
        if (GridPieces[x + offsetx, y + offsety] == null) return false;

        if (GridPieces[x, y].PieceColour != GridPieces[x + offsetx, y + offsety].PieceColour) return false;

        return true;
    }

    bool IsMatchExplosive(int x, int y)
    {
        //3x3 grid detection

        if (x < 1) return false;
        if (x == variables.GridWidth - 1) return false;
        if (y < 1) return false;
        if (y == variables.GridHeight - 1) return false;
        if (IsBomb[x, y]) return false;

        bool response = true;
        piece p = GridPieces[x, y];

        for (int h = -1; h < 2; h++)
        {
            for (int w = -1; w < 2; w++)
            {
                if (IsBomb[x + w, y + h]) return false;

                if (h == 0 && w == 0) continue;

                //Debug.Log("testing piece " + (x + w) + "," + (y + h) + " for tile " + x + "," + y);
                if (IsPieceNull(x + w, y + h)) response = false;
                else
                {
                    if (p.PieceColour != GridPieces[x + w, y + h].PieceColour) response = false;
                }
            }
        }
        return response;
    }

    public void ClearMatch(match m)
    {
        foreach (coords coords in m.Members)
        {
            piece p = GridPieces[coords.x, coords.y];

            if (p == null) continue;

            events.ClearPiece(coords, GridTools.GridCellToVector3(coords.x, coords.y, GridCentrePos), p.PieceColour);

            p.CleanupPiece();
            UnusedPieces.Push(p);
            GridPieces[coords.x, coords.y] = null;

        }
    }

    public void EmptyGrid()
    {
        for (int y = 0; y < variables.GridHeight; y++)
        {
            for (int x = 0; x < variables.GridWidth; x++)
            {

                piece p = GridPieces[x, y];

                if (p == null) continue;

                p.CleanupPiece();
                UnusedPieces.Push(p);
                GridPieces[x, y] = null;
            }
        }
    }

    public void CleanUp()
    {
        for (int y = 0; y < variables.GridHeight; y++)
        {
            for (int x = 0; x < variables.GridWidth; x++)
            {
                if (!IsPieceNull(x, y))
                {
                    GridPieces[x, y].thePool.DespawnAll();
                    x = variables.GridWidth;
                    y = variables.GridHeight;
                    return;
                    //GridPieces[x, y].CleanupPiece();
                }
            }
        }
    }

    public bool MatchContainsPiece(int x, int y)
    {
        if (CurrentMatches == null) return false;
        if (CurrentMatches.Count == 0) return false;

        foreach (match match in CurrentMatches)
        {
            foreach (coords c in match.Members)
            {
                if (c.x == x && c.y == y) return true;
            }
        }

        return false;
    }

    public int ReturnMatchedPieceCount()
    {
        return MatchedPieceCount;
    }

    public List<match> ReturnCurrentMatches()
    {
        return CurrentMatches;
    }

    public piece ReturnPiece(int x, int y)
    {
        return GridPieces[x, y];
    }
}
