public class TBAMatch_2025
{
    public long actual_time { get; set; }
    public Alliances alliances { get; set; }
    public string comp_level { get; set; }
    public string event_key { get; set; }
    public string key { get; set; }
    public int match_number { get; set; }
    public long? post_result_time { get; set; }
    public long? predicted_time { get; set; }
    public ScoreBreakdown score_breakdown { get; set; }
    public int? set_number { get; set; }
    public long? time { get; set; }
    public List<Video> videos { get; set; }
    public string winning_alliance { get; set; }
}

public class Alliances
{
    public Alliance blue { get; set; }
    public Alliance red { get; set; }
}

public class Alliance
{
    public List<string> dq_team_keys { get; set; }
    public int score { get; set; }
    public List<string> surrogate_team_keys { get; set; }
    public List<string> team_keys { get; set; }
}

public class ScoreBreakdown
{
    public Breakdown blue { get; set; }
    public Breakdown red { get; set; }
}

public class Breakdown
{
    public int adjustPoints { get; set; }
    public int algaePoints { get; set; }
    public bool autoBonusAchieved { get; set; }
    public int autoCoralCount { get; set; }
    public int autoCoralPoints { get; set; }
    public string autoLineRobot1 { get; set; }
    public string autoLineRobot2 { get; set; }
    public string autoLineRobot3 { get; set; }
    public int autoMobilityPoints { get; set; }
    public int autoPoints { get; set; }
    public Reef autoReef { get; set; }
    public bool bargeBonusAchieved { get; set; }
    public bool coopertitionCriteriaMet { get; set; }
    public bool coralBonusAchieved { get; set; }
    public int endGameBargePoints { get; set; }
    public string endGameRobot1 { get; set; }
    public string endGameRobot2 { get; set; }
    public string endGameRobot3 { get; set; }
    public int foulCount { get; set; }
    public int foulPoints { get; set; }
    public bool g206Penalty { get; set; }
    public bool g408Penalty { get; set; }
    public bool g424Penalty { get; set; }
    public int netAlgaeCount { get; set; }
    public int rp { get; set; }
    public int techFoulCount { get; set; }
    public int teleopCoralCount { get; set; }
    public int teleopCoralPoints { get; set; }
    public int teleopPoints { get; set; }
    public Reef teleopReef { get; set; }
    public int totalPoints { get; set; }
    public int wallAlgaeCount { get; set; }
    public int autoL1 { get; set; }
    public int autoL2 { get; set; }
    public int autoL3 { get; set; }
    public int autoL4 { get; set; }
    public int teleL1 { get; set; }
    public int teleL2 { get; set; }
    public int teleL3 { get; set; }
    public int teleL4 { get; set; }
}

public class Reef
{
    public Row botRow { get; set; }
    public Row midRow { get; set; }
    public Row topRow { get; set; }
    public int trough { get; set; }
}

public class Row
{
    public bool nodeA { get; set; }
    public bool nodeB { get; set; }
    public bool nodeC { get; set; }
    public bool nodeD { get; set; }
    public bool nodeE { get; set; }
    public bool nodeF { get; set; }
    public bool nodeG { get; set; }
    public bool nodeH { get; set; }
    public bool nodeI { get; set; }
    public bool nodeJ { get; set; }
    public bool nodeK { get; set; }
    public bool nodeL { get; set; }
}

public class Video
{
    public string key { get; set; }
    public string type { get; set; }
}










public class EventData
{
    //public string? address { get; set; }
    //public string? city { get; set; }
    //public string? country { get; set; }
    //public string? district { get; set; }
    //public List<string>? division_keys { get; set; }
    //public string? end_date { get; set; }
    public string? event_code { get; set; }
    //public int? event_type { get; set; }
    public string? event_type_string { get; set; }
    //public string? first_event_code { get; set; }
    //public string? first_event_id { get; set; }
    //public string? gmaps_place_id { get; set; }
    //public string? gmaps_url { get; set; }
    //public string? key { get; set; }
    //public double? lat { get; set; }
    //public double? lng { get; set; }
    //public string? location_name { get; set; }
    public string? name { get; set; }
    //public string? parent_event_key { get; set; }
    //public int? playoff_type { get; set; }
    //public string? playoff_type_string { get; set; }
    //public string? postal_code { get; set; }
    public string? short_name { get; set; }
    public string? start_date { get; set; }
    //public string? state_prov { get; set; }
    //public string? timezone { get; set; }
    //public List<string>? webcasts { get; set; }
    //public string? website { get; set; }
    public int? week { get; set; }
    public int? year { get; set; }
}


