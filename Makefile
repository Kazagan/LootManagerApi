up:
	docker-compose up -d --build
	make update-db

down:
	docker-compose down

reset:
	make down
	make up

update-db:
	dotnet ef database update --project src/Data/Data.csproj
	
verify:
	make reset
	make dotnet-test
    
dotnet-test:
	dotnet test

