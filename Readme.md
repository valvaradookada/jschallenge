# 1. Set up application

## 1.1 Start RabbitMq instance

The stock bot need a rabbitmq instance to work, so start an instance in docker with:

`
sudo docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq
`

Challenge.StockBot and Challenge.Api projects are already configured to use a rabbitmq instance in localhost, in the port 15672 ( the default port ) , edit the file if needed.

## 1.2 Start Challenge.Api project

This project is the backend Api for the chat, It should be able to connect to the RabbitMq instance, the launchSettings.json file is already configured to work with the RabbitMq instance from step 1, edit the values if needed. Start the project with Visual Studio or Jetbrains Rider

To run the project from the command line, execute this command ( from within the folder containing Jobsity.Challenge.sln file ) 

`
dotnet run --project ./Challenge.Api/Challenge.Api.csproj --launch-profile Challenge.Api
`

## 1.3 Start Challenge.StockBot project

This project handle the logic to get stock prices. It should be able to connect to the RabbitMq instance, the launchSettings.json file is already configured to work with the RabbitMq instance from step 1, edit the values if needed. Start the project with Visual Studio or Jetbrains Rider

To run the project from the command line, execute this command ( from within the folder containing Jobsity.Challenge.sln file )

`
dotnet run --project ./Challenge.StockBot/Challenge.StockBot.csproj --launch-profile Challenge.StockBot
`

## 1.4 Start Challenge.WebClient project

This project is the chat web client. It should be able to connect to the backend Api, the launchSettings.json file is already configured to reference the backend url. Start the project with Visual Studio or Jetbrains Rider

To run the project from the command line, execute this command ( from within the folder containing Jobsity.Challenge.sln file )

`
dotnet run --project ./Challenge.WebClient/Challenge.WebClient.csproj --launch-profile Challenge.WebClient
`

Navigate this url to access [chat webclient](https://localhost:7180)

# 2. TODO List

- Add business and data type validations to controllers and methods
- Add more Unit testing cases
- Add Authentication
- Centralize logging and monitoring
- Add observability/health checks
- Add Swagger documentation for Api

# 3. Features Implemented

- [X] Allow registered users to log in and talk with other users in a chatroom.
- [X] Allow users to post messages as commands into the chatroom with the following format /stock=stock_code
- [X] Create a decoupled bot that will call an API using the stock_code as a parameter (https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here aapl.us is the  stock_code)
- [X] The bot should parse the received CSV file and then it should send a message back into the chatroom using a message broker like RabbitMQ. The message will be a stock quote using the following format: “APPL.US quote is $93.42 per share”. The post owner will be the bot.
- [X] Have the chat messages ordered by their timestamps and show only the last 50 messages.
- [X] Unit test the functionality you prefer.
# 4. Design Considerations

[Click here to view the design considerations for the solution](https://github.com/valvaradookada/jschallenge/wiki/Design-Considerations)