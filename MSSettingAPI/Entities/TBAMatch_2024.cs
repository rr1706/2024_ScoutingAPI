public class TBAMatch_2024
{
    public string key { get; set; }
    public string comp_level { get; set; }
    public int set_number { get; set; }
    public int match_number { get; set; }
    public Alliances alliances { get; set; }
    public string winning_alliance { get; set; }
    public string event_key { get; set; }
    public long time { get; set; }
    public long actual_time { get; set; }
    public long predicted_time { get; set; }
    public long post_result_time { get; set; }
    public ScoreBreakdown score_breakdown { get; set; }
    public List<Video> videos { get; set; }
}

public class Alliances
{
    public Alliance red { get; set; }
    public Alliance blue { get; set; }
}

public class Alliance
{
    public int score { get; set; }
    public List<string> team_keys { get; set; }
    public List<string> surrogate_team_keys { get; set; }
    public List<string> dq_team_keys { get; set; }
}

public class ScoreBreakdown
{
    public Breakdown blue { get; set; }
    public Breakdown red { get; set; }
    public string coopertition { get; set; }
    public int coopertition_points { get; set; }
}

public class Breakdown
{
    public int adjustPoints { get; set; }
    public int autoAmpNoteCount { get; set; }
    public int autoAmpNotePoints { get; set; }
    public int autoLeavePoints { get; set; }
    public string autoLineRobot1 { get; set; }
    public string autoLineRobot2 { get; set; }
    public string autoLineRobot3 { get; set; }
    public int autoPoints { get; set; }
    public int autoSpeakerNoteCount { get; set; }
    public int autoSpeakerNotePoints { get; set; }
    public int autoTotalNotePoints { get; set; }
    public bool coopNotePlayed { get; set; }
    public bool coopertitionBonusAchieved { get; set; }
    public bool coopertitionCriteriaMet { get; set; }
    public int endGameHarmonyPoints { get; set; }
    public int endGameNoteInTrapPoints { get; set; }
    public int endGameOnStagePoints { get; set; }
    public int endGameParkPoints { get; set; }
    public string endGameRobot1 { get; set; }
    public string endGameRobot2 { get; set; }
    public string endGameRobot3 { get; set; }
    public int endGameSpotLightBonusPoints { get; set; }
    public int endGameTotalStagePoints { get; set; }
    public bool ensembleBonusAchieved { get; set; }
    public int ensembleBonusOnStageRobotsThreshold { get; set; }
    public int ensembleBonusStagePointsThreshold { get; set; }
    public int foulCount { get; set; }
    public int foulPoints { get; set; }
    public bool g206Penalty { get; set; }
    public bool g408Penalty { get; set; }
    public bool g424Penalty { get; set; }
    public bool melodyBonusAchieved { get; set; }
    public int melodyBonusThreshold { get; set; }
    public int melodyBonusThresholdCoop { get; set; }
    public int melodyBonusThresholdNonCoop { get; set; }
    public bool micCenterStage { get; set; }
    public bool micStageLeft { get; set; }
    public bool micStageRight { get; set; }
    public int rp { get; set; }
    public int techFoulCount { get; set; }
    public int teleopAmpNoteCount { get; set; }
    public int teleopAmpNotePoints { get; set; }
    public int teleopPoints { get; set; }
    public int teleopSpeakerNoteAmplifiedCount { get; set; }
    public int teleopSpeakerNoteAmplifiedPoints { get; set; }
    public int teleopSpeakerNoteCount { get; set; }
    public int teleopSpeakerNotePoints { get; set; }
    public int teleopTotalNotePoints { get; set; }
    public int totalPoints { get; set; }
    public bool trapCenterStage { get; set; }
    public bool trapStageLeft { get; set; }
    public bool trapStageRight { get; set; }
}


public class Video
{
    public string type { get; set; }
    public string key { get; set; }
}
