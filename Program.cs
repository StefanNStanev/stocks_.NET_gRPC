using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Marketdata; // Generated namespace from your .proto

class Program
{
    static async Task Main(string[] args)
    {
        // Allow plaintext HTTP/2 for local dev to Go server without TLS
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        // Check if the user provided a symbol argument
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run <SYMBOL>");
            Console.WriteLine("Example: dotnet run AAPL");
            return;
        }

        string symbol = args[0].ToUpper();

        using var channel = GrpcChannel.ForAddress("http://localhost:50051");
        var client = new MarketData.MarketDataClient(channel);

        try
        {
            var reply = await client.GetDailyDataAsync(new DailyRequest { Symbol = symbol });
            Console.WriteLine($"📈 Data for {reply.Symbol}");

            // Print table header
            Console.WriteLine("{0,-12} {1,12} {2,12} {3,12} {4,12} {5,14}", "Date", "Open", "High", "Low", "Close", "Volume");
            Console.WriteLine(new string('-', 74));

            foreach (var bar in reply.Bars)
            {
                // Align columns: Date left, prices right with 2 decimals, volume right with no decimals
                Console.WriteLine("{0,-12} {1,12:N2} {2,12:N2} {3,12:N2} {4,12:N2} {5,14:N0}",
                    bar.Date, bar.Open, bar.High, bar.Low, bar.Close, bar.Volume);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error fetching data for {symbol}: {ex.Message}");
        }
    }
}
