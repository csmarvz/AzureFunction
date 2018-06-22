using NAudio.Wave;

public static void Run(Stream myBlob, string name, TraceWriter log)
{
    using (var reader = new WaveFileReader(myBlob))
    {
        log.Info($"{name}.wav\n WaveFormat: {reader.WaveFormat}\n" +
        $" Duration: {reader.TotalTime}");
    }
}
