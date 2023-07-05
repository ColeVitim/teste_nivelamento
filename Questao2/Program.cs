using Newtonsoft.Json;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoalsAsync(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoalsAsync(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoalsAsync(string team, int year)
    {
        string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(responseBody);

            int totalGols = 0;
            foreach (var match in data.Data)
            {
                totalGols += match.Team1Goals;
            }

            return totalGols;
        }
    }

    public class Response
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<Match> Data { get; set; }
    }

    public class Match
    {
        public int Team1Goals { get; set; }
        // Outros campos relevantes que podem ser mapeados
    }
}