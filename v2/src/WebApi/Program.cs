using System.Data;
using Todos.Infra;
using DotNetEnv;

// It loads the project root .env file
Env.Load("../../.env");

var builder = WebApplication.CreateBuilder(args);
BuilderAddServices(builder);

ValidateEnv();
CheckDbConnections();

var app = builder.Build();
AppConfigure(app);
AppUseMiddlewares(app);


app.Run();

static void BuilderAddServices(WebApplicationBuilder builder)
{
    builder.Services.AddCors();
    builder.Services.AddControllers();
}

static void AppConfigure(WebApplication app)
{
    app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    app.MapControllers();
}

static void AppUseMiddlewares(WebApplication app)
{
    TimeItMiddleware(app);
}

static void TimeItMiddleware(WebApplication app)
{
    app.Use(async (ctx, next) => {
        var start = DateTime.UtcNow;
        await next.Invoke(ctx);
        var elapsed = DateTime.UtcNow - start;
        var elapsedInMills = elapsed.TotalMilliseconds;
        Console.WriteLine($"INFO: Took {elapsedInMills} ms to process the {ctx.Request.Path} request.");
    });
}

static void CheckDbConnections()
{
    try {
        IDbConnection writeConnection = DbConnectionManager.GetWriteConnection();
        DbConnectionManager.OpenConnection(writeConnection);
    } catch (Exception e) {
        Console.WriteLine("[ERROR] Could not open the Write Db Connection. " + e.Message);
        Environment.Exit(1);
    }

    try {
        IDbConnection readConnection = DbConnectionManager.GetReadConnection();
        DbConnectionManager.OpenConnection(readConnection);
    } catch (Exception e) {
        Console.WriteLine("[ERROR] Could not open the Read Db Connection. " + e.Message);
        Environment.Exit(1);
    }
}

static void CheckEnvJwtSecret(ref bool isValidEnv)
{
    string? envSecret = System.Environment.GetEnvironmentVariable("JWT_SECRET");
    if (String.IsNullOrWhiteSpace(envSecret)) {
        Console.WriteLine("[ENV_ERROR] No JWT_SECRET found in the enviroment.");
        isValidEnv = false;
    }
}

static void CheckEnvWriteDbConnection(ref bool isValidEnv)
{
    string? writeHost = Environment.GetEnvironmentVariable("WRITE_DB_HOST");
    if (String.IsNullOrWhiteSpace(writeHost)) {
        Console.WriteLine("[ENV_ERROR] No WRITE_DB_HOST found in the enviroment.");
        isValidEnv = false;
    }
    string? writePort = Environment.GetEnvironmentVariable("WRITE_DB_PORT");
    if (String.IsNullOrWhiteSpace(writePort)) {
        Console.WriteLine("[ENV_ERROR] No WRITE_DB_PORT found in the enviroment.");
        isValidEnv = false;
    }
    string? writeUsername = Environment.GetEnvironmentVariable("WRITE_DB_USERNAME");
    if (String.IsNullOrWhiteSpace(writeUsername)) {
        Console.WriteLine("[ENV_ERROR] No WRITE_DB_USERNAME found in the enviroment.");
        isValidEnv = false;
    }
    string? writePassword = Environment.GetEnvironmentVariable("WRITE_DB_PASSWORD");
    if (String.IsNullOrWhiteSpace(writePassword)) {
        Console.WriteLine("[ENV_ERROR] No WRITE_DB_PASSWORD found in the enviroment.");
        isValidEnv = false;
    }
    string? writeDbName = Environment.GetEnvironmentVariable("WRITE_DB_DBNAME");
    if (String.IsNullOrWhiteSpace(writeDbName)) {
        Console.WriteLine("[ENV_ERROR] No WRITE_DB_DBNAME found in the enviroment.");
        isValidEnv = false;
    }
}

static void CheckEnvReadDbConnection(ref bool isValidEnv)
{
    string? readHost = Environment.GetEnvironmentVariable("READ_DB_HOST");
    if (String.IsNullOrWhiteSpace(readHost)) {
        Console.WriteLine("[ENV_ERROR] No READ_DB_HOST found in the enviroment.");
        isValidEnv = false;
    }
    string? readPort = Environment.GetEnvironmentVariable("READ_DB_PORT");
    if (String.IsNullOrWhiteSpace(readPort)) {
        Console.WriteLine("[ENV_ERROR] No READ_DB_PORT found in the enviroment.");
        isValidEnv = false;
    }
    string? readUsername = Environment.GetEnvironmentVariable("READ_DB_USERNAME");
    if (String.IsNullOrWhiteSpace(readUsername)) {
        Console.WriteLine("[ENV_ERROR] No READ_DB_USERNAME found in the enviroment.");
        isValidEnv = false;
    }
    string? readPassword = Environment.GetEnvironmentVariable("READ_DB_PASSWORD");
    if (String.IsNullOrWhiteSpace(readPassword)) {
        Console.WriteLine("[ENV_ERROR] No READ_DB_PASSWORD found in the enviroment.");
        isValidEnv = false;
    }
    string? readDbName = Environment.GetEnvironmentVariable("READ_DB_DBNAME");
    if (String.IsNullOrWhiteSpace(readDbName)) {
        Console.WriteLine("[ENV_ERROR] No READ_DB_DBNAME found in the enviroment.");
        isValidEnv = false;
    }
}

static void ValidateEnv()
{
    Console.WriteLine("[INFO] Looking for enviroment variables at .env file");

    // Using flag method so you can print out all errors before exiting
    bool isValidEnv = true;
    CheckEnvJwtSecret(ref isValidEnv);
    CheckEnvWriteDbConnection(ref isValidEnv);
    CheckEnvReadDbConnection(ref isValidEnv);

    if (!isValidEnv) {
        Console.WriteLine("[ERROR] The environment is not valid to start the application. Exiting...");
        Environment.Exit(1);
    } else {
        Console.WriteLine("[SUCCESS] Environment variables have been validated.");
    }
}
