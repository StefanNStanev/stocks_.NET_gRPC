# stocks_.NET_gRPC

A small .NET console client that fetches daily market data from a gRPC MarketData service and prints it in a simple table.

## What this does
- Connects to a gRPC server (default: `http://localhost:50051`) and requests daily OHLCV bars for a symbol.
- Prints the returned bars in a neat, column-aligned table.

## Technical notes
- The client uses generated C# types from the project's `.proto` file under the `Marketdata` namespace.
- For local development without TLS the app enables plaintext HTTP/2 support:
  AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true)
  This allows connecting to non-TLS gRPC servers (use TLS in production).

## Build & run
Prerequisites:
- .NET 7+ SDK (or matching target framework used by the project)
- The gRPC server running locally (or update the address)

Build:
- dotnet build

Run:
- dotnet run -- <SYMBOL>
Example:
- dotnet run -- AAPL

The program prints a table with columns: Date, Open, High, Low, Close, Volume.

## Output example
📈 Data for AAPL
Date           Open         High          Low        Close         Volume
--------------------------------------------------------------------------
2025-10-01     145.23      147.00       144.50      146.75         12345678
2025-10-02     146.80      148.20       146.00      147.60         10123456

## Proto & generated code
- Ensure .proto files are compiled into C# (via Grpc.Tools or other tooling) so `Marketdata.MarketDataClient` and message types are available.
- The client code expects a unary RPC `GetDailyData(DailyRequest) -> DailyReply` with a repeated `Bars` collection containing fields like `Date`, `Open`, `High`, `Low`, `Close`, `Volume`.

## Troubleshooting
- "Unable to connect" — check server address and that the server is running.
- TLS errors — either run the server with TLS or keep the plaintext HTTP/2 switch for development.

## License
- Add appropriate license info for your repository.
