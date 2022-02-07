up:
	docker-compose up -d
	dotnet ef database update --project src/Data/Data.csproj

down:
	docker-compose down

reset:
	make down
	make up 
