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
            await titan.Programmer.ClearAll();
            await Test1();
            //await Test2();
        }

        private async Task Test1() {
            int num = 7;
            int start = 17;
            double hue = 0;
            for (int fixture = start; fixture < start + num; ++fixture, hue += 360 / num) {
                await AddFixture(fixture, 100, hue, 1, 1);
            }
            await titan.Programmer.SetSelectedDimmerxFade(true);
            await StorePlayback(0, 0);

            await titan.Programmer.ClearAll();
        }

        private async Task Test2() {
            int num = 5;
            int start = 11;
            double hue = 0;
            for (int group = start; group < start + num; ++group, hue += 360 / num) {
                await AddGroup(group, 100, hue, 1, 1);
            }
            await titan.Programmer.SetSelectedDimmerxFade(true);
            await StorePlayback(0, 1);

            await titan.Programmer.ClearAll();
        }

        private async Task AddFixture(double fixture, double level, double hue, double saturation, double intensity) {
            await titan.Selection.Clear();
            await titan.Selection.SelectFixturesFromHandles(HandleReference.FromUserNumbers(new double[] { fixture }));
            await SetAttributes(level, hue, saturation, intensity);
        }

        private async Task AddFixtures(double[] fixtures, double level, double hue, double saturation, double intensity) {
            await titan.Selection.Clear();
            await titan.Selection.SelectFixturesFromHandles(HandleReference.FromUserNumbers(fixtures));
            await SetAttributes(level, hue, saturation, intensity);
        }

        private async Task AddGroup(int group, double level, double hue, double saturation, double intensity) {
            await titan.Selection.Clear();
            await titan.Selection.SelectGroupFromHandle(HandleReference.FromUserNumber(group));
            await SetAttributes(level, hue, saturation, intensity);
        }

        private async Task SetAttributes(double level, double hue, double saturation, double intensity) {
            await titan.Programmer.SetDimmerLevel(level);
            await titan.Programmer.SetColourControlHSI(hue, saturation, intensity);
        }

        private async Task StorePlayback(int playbackPage, int playbackIndex) {
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
