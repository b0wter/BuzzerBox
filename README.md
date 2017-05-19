Target Framework
================
Be aware that BuzzerBox as well as BuzzerBoxTests currently were created as dot net core apps but are currently targeting the full .net framework 4.6 because some Nuget packages dont support core yet (Azure Notification Hub).
To change the two projects back to pure asp .net core projects change their targets (set in the csproj-file) back to netcoreapp1.1.
