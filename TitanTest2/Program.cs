using LXProtocols.AvolitesWebAPI;

namespace TitanTest2 {
    internal class Program {
        static async Task Main(string[] args) {
            Console.WriteLine("Avo Test");

            await (new Tests()).Run();
        }
    }

    public class Tests {
        private Titan titan;

        public Tests() {
            titan = new Titan("localhost", Titan.InteractivePort);
//            titan = new Titan("localhost", Titan.NormalPort);
        }

        public async Task Run() {
            await ClearProgrammer();
            await MultiColourGroups(3, 8, 0);
            await MultiColourGroups(5, 11, 1);
            await MultiColourFixtures(7, 17, 2);
            await MultiColourGroups(15, 23, 3);
        }

        private async Task MultiColourFixtures(int numFixtures, int startFixture, int playbackOffset) {
            double hue = 0;
            for (int fixture = startFixture; fixture < startFixture + numFixtures; ++fixture, hue += 360 / numFixtures) {
                await AddFixture(fixture);
                await SetAttributes(100, hue, 1, 1);
            }
            await titan.Programmer.SetSelectedDimmerxFade(true);
            await SavePlayback(0, playbackOffset);

            await ClearProgrammer();
        }

        private async Task MultiColourGroups(int numGroups, int startGroup, int playbackOffset) {
            double hue = 0;
            for (int group = startGroup; group < startGroup + numGroups; ++group, hue += 360 / numGroups) {
                await AddGroup(group);
                await SetAttributes(100, hue, 1, 1);
            }
            await titan.Programmer.SetSelectedDimmerxFade(true);
            await SavePlayback(0, playbackOffset);

            await ClearProgrammer();
        }

        private async Task AddFixture(double fixture) {
            await titan.Selection.Clear();
            //HandleReference id = HandleReference.FromUserNumber(fixture);
            //Console.WriteLine($"FixtureId: {id.ToQueryArgument("id")}");
            await titan.Selection.SelectFixtureFromHandle(HandleReference.FromUserNumber(fixture));
            //Console.WriteLine($"Selected fixtures: {string.Join(",", await GetSelectedIds())}");
        }

        private async Task AddFixtures(double[] fixtures) {
            await titan.Selection.Clear();
            await titan.Selection.SelectFixturesFromHandles(HandleReference.FromUserNumbers(fixtures));
        }

        private async Task AddGroup(int group) {
            await titan.Selection.Clear();
            await titan.Selection.SelectGroupFromHandle(HandleReference.FromUserNumber(group));
        }

        private async Task SetAttributes(double level, double hue, double saturation, double intensity) {
            await titan.Programmer.SetDimmerLevel(level);
            await titan.Programmer.SetColourControlHSI(hue, saturation, intensity);
        }

        private async Task<List<int>> GetSelectedIds() {
            return (await titan.Selection.GetSelectedFixtureIds()).ToList();
        }

        private async Task SavePlayback(int playbackPage, int playbackIndex) {
            List<HandleInformation> playbacks = (await titan.Playbacks.GetPlaybacks("PlaybackWindow", playbackPage)).ToList();
            HandleInformation? h = playbacks.FirstOrDefault(p => p.HandleLocation.Index == playbackIndex);
            if (null == h) {
                _ = await titan.Playbacks.StoreCue("PlaybackWindow", playbackIndex);
            } else {
                await titan.Playbacks.ReplaceCue(h.TitanId);
            }
        }

        private async Task ClearProgrammer() {
            await titan.Programmer.ClearAll();
            await Task.Delay(500);
        }

    }
}
