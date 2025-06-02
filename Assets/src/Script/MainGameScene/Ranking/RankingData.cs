using System;
using System.Collections.Generic;

[Serializable]
public class RankingData
{
    public float gameTime;
    public int score;
    public int rank;
}

[Serializable]
public class RankingDataList
{
    public List<RankingData> rankings = new();
}
