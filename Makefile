

build:
	docker-compose up -d
	dotnet ef database update
