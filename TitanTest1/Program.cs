// See https://aka.ms/new-console-template for more information
using System.Linq;
using LXProtocols.AvolitesWebAPI;
using LXProtocols.AvolitesWebAPI.Information;

Console.WriteLine("Avo Test");

var titan = new Titan("localhost", Titan.InteractivePort);

//await titan.Selection.SelectFixturesFromHandles(HandleReference.FromUserNumbers(new double[] { 17 }));
//await titan.Programmer.SetDimmerLevel(100);
//await titan.Programmer.SetColourControlHSI(0, 1, 1);

//await titan.Selection.SelectFixturesFromHandles(HandleReference.FromUserNumbers(new double[] { 18 }));
//await titan.Programmer.SetDimmerLevel(100);
//await titan.Programmer.SetColourControlHSI(60, 1, 1);

//await titan.Selection.SelectFixturesFromHandles(HandleReference.FromUserNumbers(new double[] { 19 }));
//await titan.Programmer.SetDimmerLevel(100);
//await titan.Programmer.SetColourControlHSI(120, 1, 1);

//await titan.Selection.SelectFixturesFromHandles(HandleReference.FromUserNumbers(new double[] { 20 }));
//await titan.Programmer.SetDimmerLevel(100);
//await titan.Programmer.SetColourControlHSI(180, 1, 1);

//await titan.Selection.SelectFixturesFromHandles(HandleReference.FromUserNumbers(new double[] { 21 }));
//await titan.Programmer.SetDimmerLevel(100);
//await titan.Programmer.SetColourControlHSI(240, 1, 1);

//await titan.Selection.SelectFixturesFromHandles(HandleReference.FromUserNumbers(new double[] { 22 }));
//await titan.Programmer.SetDimmerLevel(100);
//await titan.Programmer.SetColourControlHSI(300, 1, 1);

//await titan.Selection.SelectFixturesFromHandles(HandleReference.FromUserNumbers(new double[] { 23 }));
//await titan.Programmer.SetDimmerLevel(100);
//await titan.Programmer.SetColourControlHSI(360, 1, 1);

await titan.Selection.SelectGroupFromHandle(HandleReference.FromUserNumber(11));
await titan.Programmer.SetDimmerLevel(100);
await titan.Programmer.SetColourControlHSI(0, 1, 1);

await titan.Selection.SelectGroupFromHandle(HandleReference.FromUserNumber(12));
await titan.Programmer.SetDimmerLevel(100);
await titan.Programmer.SetColourControlHSI(72, 1, 1);

await titan.Selection.SelectGroupFromHandle(HandleReference.FromUserNumber(13));
await titan.Programmer.SetDimmerLevel(100);
await titan.Programmer.SetColourControlHSI(144, 1, 1);

await titan.Selection.SelectGroupFromHandle(HandleReference.FromUserNumber(14));
await titan.Programmer.SetDimmerLevel(100);
await titan.Programmer.SetColourControlHSI(216, 1, 1);

await titan.Selection.SelectGroupFromHandle(HandleReference.FromUserNumber(15));
await titan.Programmer.SetDimmerLevel(100);
await titan.Programmer.SetColourControlHSI(288, 1, 1);


//await titan.Programmer.SetControlValue(ControlNames.Tilt, null, 0.2);

await titan.Programmer.SetSelectedDimmerxFade(true);

//await titan.Programmer.LocateSelectedFixtures();

int playbackPage = 0;
int playbackIndex = 0;

List<HandleInformation> playbacks = (await titan.Playbacks.GetPlaybacks("PlaybackWindow", playbackPage)).ToList();
if (playbacks.Count == 0 || playbacks[playbackPage].HandleLocation.Index != playbackIndex) {
    int playbackId = await titan.Playbacks.StoreCue("PlaybackWindow", 0);
} else {
    await titan.Playbacks.ReplaceCue(playbacks[playbackPage].TitanId);
}

await titan.Programmer.ClearAll();


//await Task.Delay(3000);
await titan.Selection.Clear();

