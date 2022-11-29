# 1. Set up application

## 1.1 Start RabbitMq instance

The stock bot need a rabbitmq instance to work, so start an instance in docker with:

`
sudo docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq
`

Challenge.StockBot and Challenge.Api projects are already configured to use a rabbitmq instance in localhost, in the port 15672 ( the default port ) , edit the file if needed.

## 1.2 Start Challenge.Api project

//TODO: Add instructions

## 1.3 Start Challenge.StockBot project

//TODO: Add instructions

## 1.4 Start Challenge.WebClient project

//TODO: Add instructions

# 2. TODO List

