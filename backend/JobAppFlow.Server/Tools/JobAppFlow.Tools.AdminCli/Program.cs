
using JobAppFlow.Tools.AdminCli;
using JobAppFlow.Tools.AdminCli.Services;

var app = new AdminCliApp(new AdminDbCommandProcessor());
return await app.RunAsync(args);
