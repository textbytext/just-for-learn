﻿using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using System.Text.Json;

/*
 * Get started with Azure Service Bus queues (.NET)
 * https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues
 */

var cancelationToken = new CancellationTokenSource();

var configuration = new ConfigurationBuilder()
	.AddJsonFile(path: "appsettings.Development.json", optional: true)
	.Build();

var connectionString = configuration["AzureServiceBus:ConnectionString"].ToString();
// name of your Service Bus queue
var queueName = configuration["AzureServiceBus:QueueName"].ToString();

Console.WriteLine($@"
connectionString: {connectionString},
queueName: {queueName}
");

var client = new ServiceBusClient(connectionString);

var writer = Task.Run(async () =>
{
	var sender = client.CreateSender(queueName);

	for (var i = 0; i < 10; i++)
	{
		var data = new
		{
			Name = $"name_{i}",
			Value = DateTime.Now,
		};

		var json = JsonSerializer.Serialize(data);

		var message = new ServiceBusMessage(json);
		await sender.SendMessageAsync(message);
		
		Console.WriteLine($"Message {data.Name} was send");

		await Task.Delay(100);
	}

	await sender.DisposeAsync();
});


var reader = Task.Run(async () =>
{
	var processor = client.CreateProcessor(queueName);
	processor.ProcessMessageAsync += async (ProcessMessageEventArgs arg) =>
	{
		var message = arg.Message;

		string body = message.Body.ToString();
		Console.WriteLine($@"Message was read:
Body: {body}");

		await arg.CompleteMessageAsync(message);
	};

	processor.ProcessErrorAsync += (ProcessErrorEventArgs args) =>
	{
		Console.WriteLine("Exception: " + args.Exception.ToString());
		return Task.CompletedTask;
	};

	Console.WriteLine("StartProcessingAsync ...");
	await processor.StartProcessingAsync(cancelationToken.Token);

	while (!cancelationToken.Token.IsCancellationRequested)
	{
		await Task.Delay(100);
	}

	await processor.StopProcessingAsync();	
	await processor.DisposeAsync();
});


Console.WriteLine("Press ENTER!");
Console.ReadLine();

cancelationToken.Cancel();

Task.WaitAll(reader, writer);

await client.DisposeAsync();