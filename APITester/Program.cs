using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using FoosballAPI.Read.Results;
using FoosballAPI.Write.Requests;

namespace APITester
{
    class Program
    {
        private static HttpClient client = new HttpClient();
        private const string APIUrl = "http://localhost:6225/";
        private const string Teams = "api/Teams";
        private const string Games = "api/Games";

        private const string Team1 = "Team1";
        private const string Team2 = "Team2";
        private static readonly Guid Team1Id = Guid.NewGuid();
        private static readonly Guid Team2Id = Guid.NewGuid();
        private static readonly Guid GameId = Guid.NewGuid();
        private static readonly Guid SetId = Guid.NewGuid();

        static void Main(string[] args)
        {
            Start().GetAwaiter().GetResult();
        }

        private static async Task Start()
        {
            client.BaseAddress = new Uri(APIUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //AN ugly solution to wait for API
            Thread.Sleep(5000);

            try
            {
                var oldTeams = await GetTeams();
                Console.WriteLine($"Number of teams: {oldTeams.Length}");

                await CreateTeam(Team1, Team1Id);
                await CreateTeam(Team2, Team2Id);

                var newTeams = await GetTeams();
                Console.WriteLine($"Number of teams: {newTeams.Length}");

                var oldGames = await GetGames();
                Console.WriteLine($"Number of games: {oldGames.Length}");

                await CreateGame(GameId, Team1Id, Team2Id);

                var newGames = await GetGames();
                Console.WriteLine($"Number of games: {newGames.Length}");

                PrintGameDetails(await GetGame(GameId));

                await AddSetToGame(GameId, SetId);

                PrintGameDetails(await GetGame(GameId));

                await AddGoalToSet(GameId, SetId, true, false);

                PrintGameDetails(await GetGame(GameId));

                await AddGoalToSet(GameId, SetId, true, true);

                PrintGameDetails(await GetGame(GameId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        private static void PrintGameDetails(DetailedGameResult game)
        {
            Console.WriteLine();
            Console.WriteLine($"*************** Details ***************");
            Console.WriteLine($"{nameof(game.GameId)} = {game.GameId}");
            Console.WriteLine($"{nameof(game.CreatedDate)} = {game.CreatedDate}");
            Console.WriteLine($"{nameof(game.FirstTeamId)} = {game.FirstTeamId}");
            Console.WriteLine($"{nameof(game.FirstTeamName)} = {game.FirstTeamName}");
            Console.WriteLine($"{nameof(game.SecondTeamId)} = {game.SecondTeamId}");
            Console.WriteLine($"{nameof(game.SecondTeamName)} = {game.SecondTeamName}");
            Console.WriteLine($"Number of sets = {game.Sets.Length}");

            foreach(var s in game.Sets)
                Console.WriteLine($"Set result = {s.FirstTeamResult} to {s.SecondTeamResult}");
        }

        private static async Task<TeamResult[]> GetTeams()
        {
            var response = await client.GetAsync(Teams);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<TeamResult[]>();
        }

        private static async Task CreateTeam(string name, Guid id)
        {
            var response = await client.PostAsJsonAsync(Teams, new CreateTeamRequest { Name = name, TeamId = id });
            response.EnsureSuccessStatusCode();
        }

        private static async Task<DetailedGameResult> GetGame(Guid gameId)
        {
            var response = await client.GetAsync($"{Games}/{gameId}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<DetailedGameResult[]>();
            return result.SingleOrDefault();
        }

        private static async Task<GameResult[]> GetGames()
        {
            var response = await client.GetAsync(Games);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GameResult[]>();
        }

        private static async Task CreateGame(Guid gameId, Guid firstTeamId, Guid secondTeamId)
        {
            var response = await client.PostAsJsonAsync(Games, new CreateGameRequest
            {
                GameId = gameId,
                FirstTeamId = firstTeamId,
                SecondTeamId = secondTeamId
            });
            response.EnsureSuccessStatusCode();
        }

        private static async Task AddSetToGame(Guid gameId, Guid setId)
        {
            var response = await client.PutAsJsonAsync($"{Games}/{nameof(AddSetToGame)}/{gameId}",
                new AddSetToGameRequest
                {
                    SetId = setId
                });
            response.EnsureSuccessStatusCode();
        }

        private static async Task AddGoalToSet(Guid gameId, Guid setId, bool firstTeam, bool secondTeaam)
        {
            var response = await client.PutAsJsonAsync($"{Games}/{nameof(AddGoalToSet)}/{gameId}",
                new AddGoalToSetRequest
                {
                    SetId = setId,
                    FirstTeam = firstTeam,
                    SecondTeam = secondTeaam
                });

            response.EnsureSuccessStatusCode();
        }
    }
}
