namespace Todos.Infra;

public readonly struct DbConfiguration
{
    public string Host { get; init; }
    public string Port { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public string DbName { get; init; }
}
