using Project_HafizA.Business;
using Whisper.net;
using Whisper.net.Ggml;

namespace Project_HafizA.Business
{
    public class SesService : ISesService
    {
        private readonly string _modelPath;

        public SesService(string modelPath)
        {
            _modelPath = modelPath;
        }

        public async Task<string> SesiMetneÇevir(byte[] sesVerisi, string uzanti = "wav")
        {
            var geciciGelen = Path.GetTempFileName() + "." + uzanti;
            var geciciWav = Path.GetTempFileName() + ".wav";

            try
            {
                await File.WriteAllBytesAsync(geciciGelen, sesVerisi);

                // WAV değilse FFmpeg ile 16kHz mono WAV'a dönüştür
                if (uzanti != "wav")
                {
                    var ffmpeg = new System.Diagnostics.Process();
                    ffmpeg.StartInfo.FileName = @"C:\ffmpeg\bin\ffmpeg.exe";
                    ffmpeg.StartInfo.Arguments = $"-y -i \"{geciciGelen}\" -ar 16000 -ac 1 -f wav \"{geciciWav}\"";
                    ffmpeg.StartInfo.RedirectStandardError = true;
                    ffmpeg.StartInfo.UseShellExecute = false;
                    ffmpeg.StartInfo.CreateNoWindow = true;
                    ffmpeg.Start();
                    await ffmpeg.WaitForExitAsync();

                    if (!File.Exists(geciciWav) || new FileInfo(geciciWav).Length == 0)
                        throw new Exception("FFmpeg dönüştürme başarısız oldu.");
                }
                else
                {
                    geciciWav = geciciGelen;
                }

                using var whisperFactory = WhisperFactory.FromPath(_modelPath);
                using var processor = whisperFactory.CreateBuilder()
                    .WithLanguage("ar")
                    .WithPrompt("بسم الله الرحمن الرحيم")
                    .Build();

                var sonuc = new List<string>();
                using var dosyaStream = File.OpenRead(geciciWav);
                await foreach (var segment in processor.ProcessAsync(dosyaStream))
                {
                    sonuc.Add(segment.Text);
                }

                return string.Join(" ", sonuc).Trim();
            }
            finally
            {
                if (File.Exists(geciciGelen)) File.Delete(geciciGelen);
                if (geciciWav != geciciGelen && File.Exists(geciciWav)) File.Delete(geciciWav);
            }
        }
    }
}