migrate:
	@dotnet ef database update -s ./Host/ -p ./Entities/

migrate-add:
	@dotnet ef migrations add $(name) -s ./Host/ -p ./Entities/

migrate-upd:
	@dotnet ef database update $(name) -s ./Host/ -p ./Entities/

migrate-rm:
	@dotnet ef migrations remove -s ./Host/ -p ./Entities/