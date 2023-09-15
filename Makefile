build:
	dotnet build
start:
	dotnet run --project Host
publish:
	dotnet publish -c Release
dcu:
	sudo docker-compose rm && sudo docker-compose up --build -d
dcd:
	sudo docker-compose down
