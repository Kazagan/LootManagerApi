up:
	docker-compose up -d
	make update-db

down:
	docker-compose down

#migrate:
#	dotnet ef migrations add $1 --project src/Data/Data.csproj
#
#migrate-rollback:
#	dotnet ef migrations remove --project src/Data 

reset:
	make down
	make up

update-db:
	dotnet ef database update --project src/Data/Data.csproj