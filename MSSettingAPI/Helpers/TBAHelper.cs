using System.Text.Json;


namespace RRScout.Helpers
{
    public class TBAHelper
    {

        public static async Task<List<TBAMatch_2025>> getMatchData(string eventID)
        {
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.thebluealliance.com/api/v3/event/" + eventID + "/matches");
            request.Headers.Add("X-TBA-Auth-Key", "e6O1xGxIT7zsDwNUM1gAb7cNsH71EZ4JhyyvAkBwiw1qDRcEvhsW8CBUCkXxCVA8");

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new List<TBAMatch_2025>();
            }

            var TBAMatches = JsonSerializer.Deserialize<List<TBAMatch_2025>>(await response.Content.ReadAsStringAsync());

            if (TBAMatches is not null)
            {
                TBAHelper.dataCleanup(TBAMatches);
            }

            return TBAMatches;
        }
        public static async Task<List<EventData>> getAllEvents()
        {
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.thebluealliance.com/api/v3/events/2025");
            request.Headers.Add("X-TBA-Auth-Key", "e6O1xGxIT7zsDwNUM1gAb7cNsH71EZ4JhyyvAkBwiw1qDRcEvhsW8CBUCkXxCVA8");

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new List<EventData>();
            }

            var events = JsonSerializer.Deserialize<List<EventData>>(await response.Content.ReadAsStringAsync());


            return events;
        }
        public static void dataCleanup(List<TBAMatch_2025> data)
        {
            foreach (var match in data)
            {
                if (match.score_breakdown != null)
                {
                    match.alliances.red.team_keys[0] = match.alliances.red.team_keys[0].Substring(3);
                    match.alliances.red.team_keys[1] = match.alliances.red.team_keys[1].Substring(3);
                    match.alliances.red.team_keys[2] = match.alliances.red.team_keys[2].Substring(3);
                    match.alliances.blue.team_keys[0] = match.alliances.blue.team_keys[0].Substring(3);
                    match.alliances.blue.team_keys[1] = match.alliances.blue.team_keys[1].Substring(3);
                    match.alliances.blue.team_keys[2] = match.alliances.blue.team_keys[2].Substring(3);

                    match.score_breakdown.red.autoL1 = match.score_breakdown.red.autoReef.trough;
                    match.score_breakdown.red.autoL2 = getNumberCoralScored(match.score_breakdown.red.autoReef.botRow);
                    match.score_breakdown.red.autoL3 = getNumberCoralScored(match.score_breakdown.red.autoReef.midRow);
                    match.score_breakdown.red.autoL4 = getNumberCoralScored(match.score_breakdown.red.autoReef.topRow);

                    match.score_breakdown.red.teleL1 = match.score_breakdown.red.teleopReef.trough;
                    match.score_breakdown.red.teleL2 = getNumberCoralScored(match.score_breakdown.red.teleopReef.botRow);
                    match.score_breakdown.red.teleL3 = getNumberCoralScored(match.score_breakdown.red.teleopReef.midRow);
                    match.score_breakdown.red.teleL4 = getNumberCoralScored(match.score_breakdown.red.teleopReef.topRow);

                    match.score_breakdown.blue.autoL1 = match.score_breakdown.blue.autoReef.trough;
                    match.score_breakdown.blue.autoL2 = getNumberCoralScored(match.score_breakdown.blue.autoReef.botRow);
                    match.score_breakdown.blue.autoL3 = getNumberCoralScored(match.score_breakdown.blue.autoReef.midRow);
                    match.score_breakdown.blue.autoL4 = getNumberCoralScored(match.score_breakdown.blue.autoReef.topRow);

                    match.score_breakdown.blue.teleL1 = match.score_breakdown.blue.teleopReef.trough;
                    match.score_breakdown.blue.teleL2 = getNumberCoralScored(match.score_breakdown.blue.teleopReef.botRow);
                    match.score_breakdown.blue.teleL3 = getNumberCoralScored(match.score_breakdown.blue.teleopReef.midRow);
                    match.score_breakdown.blue.teleL4 = getNumberCoralScored(match.score_breakdown.blue.teleopReef.topRow);


                }
            }
        }
        public static int getNumberCoralScored(Row row)
        {
            int count = 0;

            if (row.nodeA) count++;
            if (row.nodeB) count++;
            if (row.nodeC) count++;
            if (row.nodeD) count++;
            if (row.nodeE) count++;
            if (row.nodeF) count++;
            if (row.nodeG) count++;
            if (row.nodeH) count++;
            if (row.nodeI) count++;
            if (row.nodeJ) count++;
            if (row.nodeK) count++;
            if (row.nodeL) count++;

            return count;
        }
    }
}
