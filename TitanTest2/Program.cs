using LXProtocols.AvolitesWebAPI;
using System.Reflection.Emit;

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
            await titan.Programmer.ClearAll();
            await Test1();
            await Test2();
            await Test3();
            await Test4();
        }

        private async Task Test1() {
            int numFixtures = 7;
            int startFixture = 17;
            double hue = 0;
            for (int fixture = startFixture; fixture < startFixture + numFixtures; ++fixture, hue += 360 / numFixtures) {
                await AddFixture(fixture);
                await SetAttributes(100, hue, 1, 1);
            }
            await titan.Programmer.SetSelectedDimmerxFade(true);
            await SavePlayback(0, 0);

            await titan.Programmer.ClearAll();
        }

        private async Task Test2() {
            int numGroups = 5;
            int startGroup = 11;
            double hue = 0;
            for (int group = startGroup; group < startGroup + numGroups; ++group, hue += 360 / numGroups) {
                await AddGroup(group);
                await SetAttributes(100, hue, 1, 1);
            }
            await titan.Programmer.SetSelectedDimmerxFade(true);
            await SavePlayback(0, 1);

            await titan.Programmer.ClearAll();
        }

        private async Task Test3() {
            int numGroups = 3;
            int startGroup = 8;
            double hue = 0;
            for (int group = startGroup; group < startGroup + numGroups; ++group, hue += 360 / numGroups) {
                await AddGroup(group);
                await SetAttributes(100, hue, 1, 1);
            }
            await titan.Programmer.SetSelectedDimmerxFade(true);
            await SavePlayback(0, 2);

            await titan.Programmer.ClearAll();
        }

        private async Task Test4() {
            int numGroups = 15;
            int startGroup = 23;
            double hue = 0;
            for (int group = startGroup; group < startGroup + numGroups; ++group, hue += 360 / numGroups) {
                await AddGroup(group);
                await SetAttributes(100, hue, 1, 1);
            }
            await titan.Programmer.SetSelectedDimmerxFade(true);
            await SavePlayback(0, 3);

            await titan.Programmer.ClearAll();
        }

        private async Task AddFixture(double fixture) {
            await titan.Selection.Clear();
            await titan.Selection.SelectFixtureFromHandle(HandleReference.FromUserNumber(fixture));
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

        private async Task SavePlayback(int playbackPage, int playbackIndex) {
            List<HandleInformation> playbacks = (await titan.Playbacks.GetPlaybacks("PlaybackWindow", playbackPage)).ToList();
            HandleInformation? h = playbacks.FirstOrDefault(p => p.HandleLocation.Index == playbackIndex);
            if (null == h) {
                _ = await titan.Playbacks.StoreCue("PlaybackWindow", playbackIndex);
            } else {
                await titan.Playbacks.ReplaceCue(h.TitanId);
            }
        }

    }
}
